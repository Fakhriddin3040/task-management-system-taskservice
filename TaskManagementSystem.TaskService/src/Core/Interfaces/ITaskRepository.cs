using System.Linq.Expressions;
using TaskManagementSystem.TaskService.Core.Aggregates;

namespace TaskManagementSystem.TaskService.Core.Interfaces;


public interface ITaskRepository
{
    Task<IEnumerable<TaskAggregate>> GetAllByBoardIdAsync(
        Guid boardId,
        CancellationToken cancellationToken
        );
    Task<IEnumerable<TaskAggregate>> GetAllByColumnIdAsync(
        Guid columnId,
        CancellationToken cancellationToken
        );
    Task<TaskAggregate> GetByIdAsync(Guid taskId, CancellationToken cancellationToken);
    Task CreateAsync(
        TaskAggregate task,
        CancellationToken cancellationToken
        );
    Task<long?> GetLatestRankAsync(
        Guid boardId,
        CancellationToken cancellationToken
        );
    void Update(TaskAggregate task);
    Task DeleteAsync(Guid taskId, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Expression<Func<TaskAggregate, bool>> predicate, CancellationToken cancellationToken);
    Task<bool> HasAnyColumnAsync(Guid taskId, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
