namespace TaskManagementSystem.TaskService.Application.Commands.Results;


public readonly struct TaskCreateResult(Guid id)
{
    public Guid Id { get; } = id;
}
