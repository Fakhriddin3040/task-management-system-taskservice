using System.Linq.Expressions;
using TaskManagementSystem.TaskService.Core.Aggregates;
using TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.TaskService.Core.Interfaces;

namespace TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Strategies.Validations;


public class TopNumeralRankValidationStrategy : INumeralRankValidationStrategy
{
    private readonly ITaskRepository _taskRepository;

    public TopNumeralRankValidationStrategy(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<bool> ValidateAsync(Guid boardId, NumeralRankContext context, CancellationToken cancellationToken)
    {
        Expression<Func<TaskAggregate, bool>> filter = column => column.Rank <= context.NextRank;

        var columns = (await _taskRepository.FilterAsync(
            taskBoardId: boardId,
            predicate: filter,
            cancellationToken: cancellationToken
        )).ToList();

        return columns.Count == 1;
    }
    public bool CanHandle(NumeralRankContext rankContext)
    {
        return rankContext.IsToTop;
    }
}
