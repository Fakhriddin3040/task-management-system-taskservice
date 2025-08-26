using MediatR;
using TaskManagementSystem.TaskService.Application.Queries.Queries;
using TaskManagementSystem.TaskService.Application.Queries.Results;
using TaskManagementSystem.TaskService.Core.Interfaces;

namespace TaskManagementSystem.TaskService.Application.Queries.Handlers;


public class GetByColumnIdQueryHandler : IRequestHandler<GetByColumnIdQuery, GetByColumnIdQueryResult>
{
    private readonly ITaskRepository _taskRepository;

    public GetByColumnIdQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<GetByColumnIdQueryResult> Handle(GetByColumnIdQuery request, CancellationToken cancellationToken)
    {
        var results = await _taskRepository.GetAllByColumnIdAsync(columnId: request.ColumnId, cancellationToken);

        return GetByColumnIdQueryResult.FromTasks(results);
    }
}
