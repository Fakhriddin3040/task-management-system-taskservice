using MediatR;
using TaskManagementSystem.SharedLib.Handlers;
using TaskManagementSystem.TaskService.Application.Commands.Results;

namespace TaskManagementSystem.TaskService.Application.Commands.Commands;


public readonly struct TaskCreateCommand(
    Guid boardId,
    Guid assignedToId,
    Guid columnId,
    string title,
    string description,
    long deadline)
    : IRequest<Result<TaskCreateResult>>
{
    public Guid BoardId { get; } = boardId;
    public Guid AssignedToId { get; } = assignedToId;
    public Guid ColumnId { get; } = columnId;
    public string Title { get; }= title;
    public string Description { get; } = description;
    public long Deadline { get; } = deadline;

}
