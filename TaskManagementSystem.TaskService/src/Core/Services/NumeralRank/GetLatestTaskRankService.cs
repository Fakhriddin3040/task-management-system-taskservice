using TaskManagementSystem.SharedLib.Algorithms.NumeralRank;
using TaskManagementSystem.SharedLib.Exceptions;
using TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Interfaces;
using TaskManagementSystem.TaskService.Core.Interfaces;
using NumeralRankContext = TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.NumeralRankContext;

namespace TaskManagementSystem.TaskService.Core.Services.NumeralRank;


public class GetLatestTaskRankService
{
    private readonly ITaskRepository _taskRepository;
    private readonly INumeralRankStrategySelector _rankStrategySelector;

    public GetLatestTaskRankService(
        ITaskRepository taskRepository,
        INumeralRankStrategySelector rankStrategySelector)
    {
        _taskRepository = taskRepository;
        _rankStrategySelector = rankStrategySelector;
    }

    public async Task<long> GetLatestRankAsync(Guid boardId, CancellationToken cancellationToken)
    {
        var latestRank = await _taskRepository.GetLatestRankAsync(boardId, cancellationToken);

        var rankContext = new NumeralRankContext(
            previousRank: latestRank ?? NumeralRankOptions.Empty,
            nextRank: NumeralRankOptions.Empty);

        var rankingStrategy = _rankStrategySelector.GetStrategy(context: rankContext);

        var newRank = rankingStrategy.GenerateRank(context: rankContext);

        if (newRank.NeedToReorder)
        {
            throw new NotImplementedException();
        }

        if (!newRank.IsValid)
        {
            throw new AppUnexpectedException("Generated rank is not valid.");
        }

        return newRank.Rank;
    }
}
