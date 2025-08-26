using TaskManagementSystem.TaskService.Core.Models;

namespace TaskManagementSystem.TaskService.Application.Queries.Results;


public readonly struct GetDetailedQueryResult
{
    public Guid Id { get; init; }
    public Guid BoardId { get; init; }
    public Guid ColumnId { get; init; }
    public Guid AssignedToId { get; init; }
    public Guid CreatedById { get; init; }
    public Guid UpdatedById { get; init; }
    public long Rank { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public long Deadline { get; init; }
    public long CreatedAt { get; init; }
    public long UpdatedAt { get; init; }

    public static GetDetailedQueryResult FromTask(TaskModel task)
    {
        return new GetDetailedQueryResult {
            Id = task.Id,
            BoardId = task.BoardId,
            ColumnId = task.ColumnId,
            AssignedToId = task.AssignedToId,
            CreatedAt = task.Timestamps.CreatedAt,
            Rank = task.Rank,
            UpdatedAt = task.Timestamps.UpdatedAt,
            CreatedById = task.AuthorInfo.CreatedById,
            UpdatedById = task.AuthorInfo.UpdatedById,
            Title = task.Title,
            Description = task.Description,
            Deadline = task.Deadline
        };
    }
}
