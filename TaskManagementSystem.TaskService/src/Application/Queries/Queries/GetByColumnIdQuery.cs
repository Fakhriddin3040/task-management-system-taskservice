using MediatR;
using TaskManagementSystem.TaskService.Application.DTO;
using TaskManagementSystem.TaskService.Application.Queries.Results;

namespace TaskManagementSystem.TaskService.Application.Queries.Queries;

public record GetByColumnIdQuery(Guid ColumnId) : IRequest<GetByColumnIdQueryResult>;
