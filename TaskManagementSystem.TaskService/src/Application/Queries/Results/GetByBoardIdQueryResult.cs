using TaskManagementSystem.TaskService.Application.DTO;
using TaskManagementSystem.TaskService.Core.Models;

namespace TaskManagementSystem.TaskService.Application.Queries.Results;


public readonly struct GetByBoardIdQueryResult(IEnumerable<GetByBoardIdDto> tasks)
{
    public IEnumerable<GetByBoardIdDto> Tasks { get; } = tasks;

    public static GetByBoardIdQueryResult FromTasks(IEnumerable<TaskModel> tasks)
    {
        return new GetByBoardIdQueryResult(
            tasks.Select(t => new GetByBoardIdDto(
                id: t.Id,
                title: t.Title,
                columnId: t.ColumnId,
                assignedToId: t.AssignedToId,
                rank: t.Rank
            ))
        );
    }
}
