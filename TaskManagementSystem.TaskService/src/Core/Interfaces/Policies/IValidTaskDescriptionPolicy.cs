namespace TaskManagementSystem.TaskService.Core.Interfaces.Policies;


public interface IValidTaskDescriptionPolicy
{
    /// <summary>
    /// Validates the task description.
    /// </summary>
    /// <param name="title">The task description to validate.</param>
    /// <returns>True if the description is valid, otherwise false.</returns>
    bool IsValid(string title);
}
