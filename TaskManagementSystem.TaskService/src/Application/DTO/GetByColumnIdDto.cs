namespace TaskManagementSystem.TaskService.Application.DTO;


public readonly struct GetByColumnIdDto(
    Guid id,
    Guid assignedToId,
    string title,
    long rank)
{
    public Guid Id { get; } = id;
    public Guid AssignedToId { get; } = assignedToId;
    public string Title { get; } = title;
    public long Rank { get; } = rank;
}
