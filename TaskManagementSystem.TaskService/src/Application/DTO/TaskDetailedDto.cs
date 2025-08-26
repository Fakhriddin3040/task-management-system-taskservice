namespace TaskManagementSystem.TaskService.Application.DTO;

public readonly struct TaskDetailedDto
{
    public Guid Id { get; init; }
    public Guid BoardId { get; init; }
    public Guid ColumnId { get; init; }
    public Guid AssignedToId { get; init; }
    public Guid CreatedById { get; init; }
    public Guid UpdatedById { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public long Deadline { get; init; }
    public long CreatedAt { get; init; }
    public long UpdatedAt { get; init; }
}
