using TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.TaskService.Core.Interfaces.Repository;

namespace TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Strategies.Validations;


public class FirstNumeralRankValidationStrategy : INumeralRankValidationStrategy
{
    private readonly ITaskBoardRepository _boardRepository;

    public FirstNumeralRankValidationStrategy(ITaskBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<bool> ValidateAsync(Guid boardId, NumeralRankContext context, CancellationToken cancellationToken)
    {
        return await _boardRepository.HasAnyColumnAsync(boardId, cancellationToken);
    }

    public bool CanHandle(NumeralRankContext rankContext)
    {
        return rankContext.IsFirstRank;
    }
}
