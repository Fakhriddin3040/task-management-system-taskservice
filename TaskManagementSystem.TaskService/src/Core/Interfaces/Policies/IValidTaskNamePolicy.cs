namespace TaskManagementSystem.TaskService.Core.Interfaces.Policies;


public interface IValidTaskNamePolicy
{
    /// <summary>
    /// Validates the task description.
    /// </summary>
    /// <param name="description">The task description to validate.</param>
    /// <returns>True if the description is valid, otherwise false.</returns>
    bool IsValid(string description);
}
