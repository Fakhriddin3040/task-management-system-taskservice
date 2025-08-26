namespace TaskManagementSystem.TaskService.Application.DTO;


public readonly struct TaskDto(
    Guid Id,
    Guid ColumnId,
    Guid AssignedToId,
    string Title);
