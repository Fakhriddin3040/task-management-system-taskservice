using MediatR;
using TaskManagementSystem.SharedLib.Abstractions.Interfaces;
using TaskManagementSystem.SharedLib.Algorithms.NumeralRank;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.SharedLib.Providers.Interfaces;
using TaskManagementSystem.TaskService.Application.Commands.Commands;
using TaskManagementSystem.TaskService.Core.Interfaces;
using TaskManagementSystem.TaskService.Core.Services.NumeralRank;
using ExecutionContext = TaskManagementSystem.SharedLib.DTO.ExecutionContext;
using NumeralRankContext = TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.NumeralRankContext;

namespace TaskManagementSystem.TaskService.Application.Commands.Handlers;


public class TaskMoveCommandHandler : IRequestHandler<TaskMoveCommand>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IDateTimeService _dateTimeService;
    private readonly ILogger<TaskMoveCommandHandler> _logger;
    private readonly NumeralRankGenerationService _numeralRankGenerationService;
    private readonly ExecutionContext _executionContext;

    public TaskMoveCommandHandler(
        ITaskRepository taskRepository,
        IDateTimeService dateTimeService,
        ILogger<TaskMoveCommandHandler> logger,
        NumeralRankGenerationService numeralRankGenerationService,
        IExecutionContextProvider executionContextProvider)
    {
        _taskRepository = taskRepository;
        _dateTimeService = dateTimeService;
        _logger = logger;
        _numeralRankGenerationService = numeralRankGenerationService;
        _executionContext = executionContextProvider.GetContext();
    }

    public async Task Handle(TaskMoveCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Task with ID {Id} has been processed", request.Id);
        var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);

        if (task == null)
        {
            _logger.LogError("Task with ID {Id} not found", request.Id);
            throw AppException.NotFound();
        }

        var rankContext = await GetRankContextAsync(task.BoardId, request, cancellationToken);

        await task.MoveAsync(
            columnId: request.ColumnId,
            updatedById: _executionContext.User.Id,
            rankContext: rankContext,
            numeralRankGenerationService: _numeralRankGenerationService,
            cancellationToken: cancellationToken,
            dateTimeService: _dateTimeService
        );

        _taskRepository.Update(task);

        var result = await _taskRepository.SaveChangesAsync(cancellationToken);

        if (result != 1)
        {
            _logger.LogError("Failed to move task with ID {Id}", request.Id);
            throw new AppUnexpectedException();
        }
        _logger.LogInformation("Task with ID {Id} has been moved", request.Id);
    }

    private async Task<NumeralRankContext> GetRankContextAsync(
        Guid boardId,
        TaskMoveCommand request,
        CancellationToken cancellationToken)
    {
        var tasks = (await _taskRepository.FilterAsync(
            taskBoardId: boardId,
            t => t.Id == request.PreviousTaskId || t.Id == request.NextTaskId,
            cancellationToken: cancellationToken
        )).OrderBy(t => t.Rank).ToList();

        return new(
            previousRank: tasks.FirstOrDefault(t => t.Id == request.PreviousTaskId)?.Rank ?? NumeralRankOptions.Empty,
            nextRank: tasks.FirstOrDefault(t => t.Id == request.NextTaskId)?.Rank ?? NumeralRankOptions.Empty
        );
    }
}
