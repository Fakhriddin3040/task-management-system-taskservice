using MediatR;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.TaskService.Application.Queries.Queries;
using TaskManagementSystem.TaskService.Application.Queries.Results;
using TaskManagementSystem.TaskService.Core.Interfaces;

namespace TaskManagementSystem.TaskService.Application.Queries.Handlers;


public class GetDetailedQueryHandler : IRequestHandler<GetDetailedQuery, GetDetailedQueryResult>
{
    private readonly ITaskRepository _taskRepository;

    public GetDetailedQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<GetDetailedQueryResult> Handle(GetDetailedQuery request, CancellationToken cancellationToken)
    {
        var result = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);

        if (result == null)
        {
            throw AppException.NotFound();
        }

        return GetDetailedQueryResult.FromTask(result);
    }
}
