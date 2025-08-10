using MediatR;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.TaskService.Application.Commands.Commands;
using TaskManagementSystem.TaskService.Core.Interfaces;

namespace TaskManagementSystem.TaskService.Application.Commands.Handlers;


public class TaskDeleteCommandHandler : IRequestHandler<TaskDeleteCommand>
{
    private readonly ITaskRepository _taskRepository;
    private readonly ILogger<TaskDeleteCommandHandler> _logger;

    public TaskDeleteCommandHandler(
        ITaskRepository taskRepository,
        ILogger<TaskDeleteCommandHandler> logger)
    {
        _taskRepository = taskRepository;
        _logger = logger;
    }

    public async Task Handle(TaskDeleteCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);

        if (task == null)
        {
            _logger.LogError("Task with ID {Id} not found", request.Id);
            throw AppException.NotFound();
        }

        _logger.LogInformation("Deleting task with ID {Id}", request.Id);
        task.Delete();
        await _taskRepository.DeleteAsync(request.Id, cancellationToken);

        _logger.LogInformation("Task with ID {Id} deleted successfully", request.Id);
    }
}
