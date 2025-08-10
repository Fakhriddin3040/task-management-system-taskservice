using MediatR;
using TaskManagementSystem.SharedLib.Handlers;
using Unit = TaskManagementSystem.SharedLib.Structs.Unit;

namespace TaskManagementSystem.TaskService.Application.Commands.Commands;


public readonly struct TaskUpdateCommand(
    Guid id,
    string title,
    string description,
    Guid assignedToId,
    long deadline
) : IRequest<Result<Unit>>
{
    public Guid Id { get; } = id;
    public string Title { get; } = title;
    public string Description { get; } = description;
    public Guid AssignedToId { get; } = assignedToId;
    public long Deadline { get; } = deadline;
}
