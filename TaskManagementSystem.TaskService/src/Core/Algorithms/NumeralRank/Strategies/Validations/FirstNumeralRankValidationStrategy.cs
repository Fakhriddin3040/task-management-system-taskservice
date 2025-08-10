using TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.TaskService.Core.Interfaces;

namespace TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Strategies.Validations;


public class FirstNumeralRankValidationStrategy : INumeralRankValidationStrategy
{
    private readonly ITaskRepository _taskRepository;

    public FirstNumeralRankValidationStrategy(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<bool> ValidateAsync(Guid boardId, NumeralRankContext context, CancellationToken cancellationToken)
    {
        return ! await _taskRepository.ExistsAsync(
            t => t.Id == boardId, cancellationToken
        );
    }

    public bool CanHandle(NumeralRankContext rankContext)
    {
        return rankContext.IsFirstRank;
    }
}
