using TaskManagementSystem.TaskService.Core.Interfaces.Policies;

namespace TaskManagementSystem.TaskService.Infrastructure.Policies;


public class ValidTaskDescriptionPolicy : IValidTaskDescriptionPolicy
{
    private const int MaxLength = 500;

    public bool IsValid(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return false;
        }

        return title.Length <= MaxLength;
    }
}
