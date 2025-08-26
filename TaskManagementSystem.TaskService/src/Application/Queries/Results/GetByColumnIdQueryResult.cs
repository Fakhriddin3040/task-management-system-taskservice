using TaskManagementSystem.TaskService.Application.DTO;
using TaskManagementSystem.TaskService.Core.Models;

namespace TaskManagementSystem.TaskService.Application.Queries.Results;


public readonly struct GetByColumnIdQueryResult(IEnumerable<GetByColumnIdDto> tasks)
{
    public IEnumerable<GetByColumnIdDto> Tasks { get; } = tasks;

    public static GetByColumnIdQueryResult FromTasks(IEnumerable<TaskModel> tasks)
    {
        return new GetByColumnIdQueryResult(
            tasks.Select(t => new GetByColumnIdDto(
                id: t.Id,
                assignedToId: t.AssignedToId,
                title: t.Title,
                rank: t.Rank)
            )
        );
    }
}
