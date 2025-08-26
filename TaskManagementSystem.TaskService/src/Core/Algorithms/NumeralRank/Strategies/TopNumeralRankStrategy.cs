using TaskManagementSystem.SharedLib.Algorithms.NumeralRank;
using TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Interfaces;

namespace TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Strategies;


public class TopNumeralRankStrategy : INumeralRankStrategy
{
    public NumeralRankResult GenerateRank(NumeralRankContext context)
    {
        var needReorder = context.NextRank / 2 < NumeralRankOptions.MinGap;

        return needReorder
            ? NumeralRankResult.ForReorder()
            : new(context.NextRank / 2);
    }

    public bool CanHandle(NumeralRankContext context)
    {
        return context.IsToTop;
    }
}
