using System.Linq.Expressions;
using TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.TaskService.Core.Interfaces.Repository;
using TaskManagementSystem.TaskService.Core.Models;

namespace TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Strategies.Validations;


public class EndNumeralRankValidationStrategy : INumeralRankValidationStrategy
{
    // public EndNumeralRankValidationStrategy(ITaskBoardRepository boardRepository)
    // {
    //     _boardRepository = boardRepository;
    // }

    public async Task<bool> ValidateAsync(Guid boardId, NumeralRankContext context, CancellationToken cancellationToken)
    {
        Expression<Func<TaskBoardColumnModel, bool>> filter = col => col.Order >= context.PreviousRank;

        var columns = (await _boardRepository.FilterColumnsAsync(
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
