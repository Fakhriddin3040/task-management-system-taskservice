using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank;
using TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Interfaces;

namespace TaskManagementSystem.TaskService.Core.Services.NumeralRank;


public class NumeralRankGenerationService
{
    private readonly INumeralRankValidationStrategySelector _validationStrategySelector;
    private readonly INumeralRankStrategySelector _rankStrategySelector;

    public NumeralRankGenerationService(
        INumeralRankValidationStrategySelector validationStrategySelector,
        INumeralRankStrategySelector rankStrategySelector)
    {
        _validationStrategySelector = validationStrategySelector;
        _rankStrategySelector = rankStrategySelector;
    }

    public async Task<long> GetNewRankAsync(
        Guid boardId,
        NumeralRankContext context,
        CancellationToken cancellationToken)
    {
        await ValidateAsync(boardId, context, cancellationToken);

        var rankingStrategy = _rankStrategySelector.GetStrategy(context: context);
        var newRank = rankingStrategy.GenerateRank(context: context);

        if (newRank.NeedToReorder)
        {
            throw new NotImplementedException("Reordering is not implemented yet.");
        }
        if (!newRank.IsValid)
        {
            throw new AppUnexpectedException("Generated rank is not valid.");
        }

        return newRank.Rank;
    }

    public async Task ValidateAsync(
        Guid boardId,
        NumeralRankContext context,
        CancellationToken cancellationToken)
    {
        var validationStrategy = _validationStrategySelector.GetValidationStrategy(
            context: context);

        await validationStrategy.ValidateAsync(boardId: boardId, context: context, cancellationToken: cancellationToken);
    }
}
