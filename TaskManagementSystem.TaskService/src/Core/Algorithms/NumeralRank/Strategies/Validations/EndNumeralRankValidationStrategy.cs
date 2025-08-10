using System.Linq.Expressions;
using TaskManagementSystem.TaskService.Core.Aggregates;
using TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.TaskService.Core.Interfaces;

namespace TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Strategies.Validations;


public class EndNumeralRankValidationStrategy : INumeralRankValidationStrategy
{
    private readonly ITaskRepository _taskRepository;

    public EndNumeralRankValidationStrategy(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<bool> ValidateAsync(Guid boardId, NumeralRankContext context, CancellationToken cancellationToken)
    {
        Expression<Func<TaskAggregate, bool>> filter = col => col.Rank >= context.PreviousRank;

        var columns = (await _taskRepository.FilterAsync(
            taskBoardId: boardId,
            predicate: filter,
            cancellationToken: cancellationToken
        )).ToList();

        return columns.Count == 1;
    }

    public bool CanHandle(NumeralRankContext rankContext)
    {
        return rankContext.IsToEnd;
    }
}
