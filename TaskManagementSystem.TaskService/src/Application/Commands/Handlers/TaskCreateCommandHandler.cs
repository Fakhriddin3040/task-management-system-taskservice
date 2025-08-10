using MediatR;
using TaskManagementSystem.SharedLib.Abstractions.Interfaces;
using TaskManagementSystem.SharedLib.Extensions;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.SharedLib.Providers.Interfaces;
using TaskManagementSystem.TaskService.Application.Commands.Commands;
using TaskManagementSystem.TaskService.Application.Commands.Results;
using TaskManagementSystem.TaskService.Core.Aggregates;
using TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.TaskService.Core.Interfaces;
using TaskManagementSystem.TaskService.Core.Interfaces.Policies;
using TaskManagementSystem.TaskService.Core.Services.NumeralRank;
using ExecutionContext = TaskManagementSystem.SharedLib.DTO.ExecutionContext;

namespace TaskManagementSystem.TaskService.Application.Commands.Handlers;


public class TaskCreateCommandHandler : IRequestHandler<TaskCreateCommand, Result<TaskCreateResult>>
{
    private readonly ITaskRepository _taskRepository;
    private readonly ILogger<TaskCreateCommandHandler> _logger;
    private readonly IDateTimeService _dateTimeService;
    private readonly INumeralRankStrategySelector _numeralRankStrategySelector;
    private readonly IValidTaskTitlePolicy _titlePolicy;
    private readonly IValidTaskDescriptionPolicy _descriptionPolicy;
    private readonly ExecutionContext _executionContext;
    private readonly GetLatestTaskRankService _getLatestTaskRankService;

    public TaskCreateCommandHandler(
        ITaskRepository taskRepository,
        ILogger<TaskCreateCommandHandler> logger,
        IDateTimeService dateTimeService,
        INumeralRankStrategySelector numeralRankStrategySelector,
        INumeralRankValidationStrategySelector numeralRankValidationStrategySelector,
        IValidTaskTitlePolicy titlePolicy,
        IValidTaskDescriptionPolicy descriptionPolicy,
        IExecutionContextProvider executionContextProvider,
        GetLatestTaskRankService getLatestTaskRankService)
    {
        _taskRepository = taskRepository;
        _logger = logger;
        _dateTimeService = dateTimeService;
        _numeralRankStrategySelector = numeralRankStrategySelector;
        _titlePolicy = titlePolicy;
        _descriptionPolicy = descriptionPolicy;
        _getLatestTaskRankService = getLatestTaskRankService;
        _executionContext = executionContextProvider.GetContext();
    }

    public async Task<Result<TaskCreateResult>> Handle(TaskCreateCommand request, CancellationToken cancellationToken)
    {
        var result = await TaskAggregate.Create(
            title: request.Title,
            description: request.Description,
            boardId: request.BoardId,
            columnId: request.ColumnId,
            assignedToId: request.AssignedToId,
            deadLine: request.Deadline,
            createdById: _executionContext.User.Id,
            cancellationToken: cancellationToken,
            dateTimeService: _dateTimeService,
            titlePolicy: _titlePolicy,
            descriptionPolicy: _descriptionPolicy,
            getLatestTaskRankService: _getLatestTaskRankService
        );

        if (result.IsFailure)
        {
            _logger.LogError("Task creation failed: {Error}. Details in JSON format: ", result.ErrorDetailsToJson());
            return Result<TaskCreateResult>.Failure(result.ErrorDetails);
        }

        var task = result.Value;

        await _taskRepository.CreateAsync(task, cancellationToken);

        _logger.LogInformation("Task created successfully with ID: {TaskId}", task.Id);

        return Result<TaskCreateResult>.Success(
            new(id: task.Id)
        );
    }
}
