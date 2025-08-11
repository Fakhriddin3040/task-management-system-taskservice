using MediatR;
using TaskManagementSystem.SharedLib.Abstractions.Interfaces;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.SharedLib.Extensions;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.SharedLib.Providers.Interfaces;
using TaskManagementSystem.TaskService.Application.Commands.Commands;
using TaskManagementSystem.TaskService.Core.Interfaces;
using TaskManagementSystem.TaskService.Core.Interfaces.Policies;
using ExecutionContext = TaskManagementSystem.SharedLib.DTO.ExecutionContext;
using Unit = TaskManagementSystem.SharedLib.Structs.Unit;

namespace TaskManagementSystem.TaskService.Application.Commands.Handlers;


public class TaskUpdateCommandHandler : IRequestHandler<TaskUpdateCommand, Result<Unit>>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IDateTimeService _dateTimeService;
    private readonly IValidTaskTitlePolicy _titlePolicy;
    private readonly IValidTaskDescriptionPolicy _descriptionPolicy;
    private ILogger<TaskUpdateCommandHandler> _logger;
    private readonly ExecutionContext _executionContext;

    public TaskUpdateCommandHandler(
        ITaskRepository taskRepository,
        IDateTimeService dateTimeService,
        IValidTaskTitlePolicy titlePolicy,
        IValidTaskDescriptionPolicy descriptionPolicy,
        ILogger<TaskUpdateCommandHandler> logger,
        IExecutionContextProvider executionContextProvider)
    {
        _taskRepository = taskRepository;
        _dateTimeService = dateTimeService;
        _titlePolicy = titlePolicy;
        _descriptionPolicy = descriptionPolicy;
        _logger = logger;
        _executionContext = executionContextProvider.GetContext();
    }

    public async Task<Result<Unit>> Handle(TaskUpdateCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);

        if (task == null)
        {
            throw AppException.NotFound();
        }

        var result = task.Update(
            title : request.Title,
            description : request.Description,
            assignedToId: request.AssignedToId,
            deadLine: request.Deadline,
            updatedById: _executionContext.User.Id,
            dateTimeService: _dateTimeService,
            cancellationToken: cancellationToken,
            titlePolicy: _titlePolicy,
            descriptionPolicy: _descriptionPolicy
        );

        if (result.IsFailure)
        {
            _logger.LogError("Task update failed. Details in json format: {Error}", result.ErrorDetailsToJson());
            return Result<Unit>.Failure(result.ErrorDetails);
        }

        _taskRepository.Update(task);
        var saveResult = await _taskRepository.SaveChangesAsync(cancellationToken);

        if (saveResult != 1)
        {
            throw new AppUnexpectedException();
        }

        return Result<Unit>.Success(Unit.Value);
    }
}
