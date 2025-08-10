using MediatR;

namespace TaskManagementSystem.TaskService.Application.Commands.Commands;


public readonly struct TaskDeleteCommand(Guid id) : IRequest
{
    public Guid Id { get; } = id;
}
