using MediatR;
using TaskManagementSystem.TaskService.Application.Queries.Queries;
using TaskManagementSystem.TaskService.Application.Queries.Results;
using TaskManagementSystem.TaskService.Core.Interfaces;

namespace TaskManagementSystem.TaskService.Application.Queries.Handlers;


public class GetByBoardIdQueryHandler : IRequestHandler<GetByBoardIdQuery, GetByBoardIdQueryResult>
{
    private readonly ITaskRepository _taskRepository;

    public GetByBoardIdQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<GetByBoardIdQueryResult> Handle(GetByBoardIdQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.GetAllByBoardIdAsync(boardId: request.BoardId, cancellationToken);

        return GetByBoardIdQueryResult.FromTasks(tasks);
    }
}
