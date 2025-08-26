using MediatR;
using TaskManagementSystem.TaskService.Application.DTO;
using TaskManagementSystem.TaskService.Application.Queries.Results;

namespace TaskManagementSystem.TaskService.Application.Queries.Queries;


public readonly struct GetByBoardIdQuery(Guid boardId) : IRequest<GetByBoardIdQueryResult>
{
    public Guid BoardId { get; } = boardId;
}
