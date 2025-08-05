using TaskManagementSystem.AuthService.Core.ValueObjects;
using TaskManagementSystem.SharedLib.ValueObjects;
using TaskManagementSystem.TaskService.Core.Aggregates;

namespace TaskManagementSystem.TaskService.Core.Models;


public class TaskModel
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; }
    public Guid? AssignedTo { get; set; }
    public Guid ColumnId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public long Rank { get; set; }
    public DateTime Deadline { get; set; }
    public AuthorInfo AuthorInfo { get; set; }
    public Timestamps Timestamps { get; set; }
}
