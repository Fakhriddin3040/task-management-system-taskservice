namespace TaskManagementSystem.TaskService.Core.Algorithms.NumeralRank.Interfaces;


public interface INumeralRankStrategySelector
{
    INumeralRankStrategy GetStrategy(NumeralRankContext context);
}
