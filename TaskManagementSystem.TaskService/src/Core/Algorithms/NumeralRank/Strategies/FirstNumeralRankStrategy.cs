using TaskManagementSystem.SharedLib.Algorithms.NumeralRank;
using TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Interfaces;

namespace TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Strategies;


public class FirstNumeralRankStrategy : INumeralRankStrategy
{
    public NumeralRankResult GenerateRank(NumeralRankContext context)
    {
        return new(
            rank: NumeralRankOptions.Default
        );
    }

    public bool CanHandle(NumeralRankContext context)
    {
        return context.IsFirstRank;
    }
}
