namespace TaskManagementSystem.TaskService.Application.DTO;


public readonly struct GetByBoardIdDto(
    Guid id,
    Guid columnId,
    Guid assignedToId,
    long rank,
    string title)
{
    public Guid Id { get; } = id;
    public Guid ColumnId { get; } = columnId;
    public Guid AssignedToId { get; } = assignedToId;
    public long Rank { get; } = rank;
    public string Title { get; } = title;
}
