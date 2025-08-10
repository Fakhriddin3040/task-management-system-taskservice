using System.Linq.Expressions;
using TaskManagementSystem.TaskService.Core.Aggregates;
using TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.TaskService.Core.Interfaces;

namespace TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Strategies.Validations;


public class BetweenNumeralRankValidationStrategy : INumeralRankValidationStrategy
{
    private readonly ITaskRepository _taskRepository;

    public BetweenNumeralRankValidationStrategy(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
    }

    public async Task<bool> ValidateAsync(Guid boardId, NumeralRankContext context, CancellationToken cancellationToken)
    {
        Expression<Func<TaskAggregate, bool>> predicate = c => c.Rank >= context.PreviousRank && c.Rank <= context.NextRank;

        IEnumerable<TaskAggregate> columns = (await _taskRepository.FilterAsync(
            taskBoardId: boardId,
            predicate: predicate,
            cancellationToken: cancellationToken
        )).ToList();

        if (columns.Count() != 2)
        {
            return false;
        }

        var orderedColumns = columns.OrderBy(c => c.Rank).ToList();

        if (orderedColumns[0].Rank != context.PreviousRank ||
            orderedColumns[1].Rank != context.NextRank ||
            orderedColumns[0].Rank > orderedColumns[1].Rank ||
            orderedColumns[0].Rank == orderedColumns[1].Rank)
        {
            return false;
        }

        return true;
    }
    public bool CanHandle(NumeralRankContext rankContext)
    {
        return rankContext.IsBetween;
    }
}
