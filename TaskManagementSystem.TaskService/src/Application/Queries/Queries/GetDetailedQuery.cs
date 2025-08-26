using MediatR;
using TaskManagementSystem.TaskService.Application.Queries.Results;

namespace TaskManagementSystem.TaskService.Application.Queries.Queries;


public readonly struct GetDetailedQuery(Guid id) : IRequest<GetDetailedQueryResult>
{
    public Guid Id { get; } = id;
}
