using MediatR;
using TaskManagementSystem.SharedLib.Handlers;
using Unit = TaskManagementSystem.SharedLib.Structs.Unit;

namespace TaskManagementSystem.TaskService.Application.Commands.Commands;


public readonly struct TaskMoveCommand(
    Guid id,
    Guid previousTaskId,
    Guid nextTaskId,
    Guid columnId
) : IRequest
{
    public Guid Id { get; } = id;
    public Guid PreviousTaskId { get; } = previousTaskId;
    public Guid NextTaskId { get; } = nextTaskId;
    public Guid ColumnId { get; } = columnId;
}
