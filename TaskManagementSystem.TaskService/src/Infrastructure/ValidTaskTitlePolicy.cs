using TaskManagementSystem.TaskService.Core.Interfaces.Policies;

namespace TaskManagementSystem.TaskService.Infrastructure.Policies;


public class ValidTaskTitlePolicy : IValidTaskTitlePolicy
{
    public const int MaxLength = 100;

    public bool IsValid(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return false;
        }

        return title.Length <= MaxLength;
    }
}
