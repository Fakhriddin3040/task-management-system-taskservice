using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.SharedLib.Algorithms.NumeralRank;
using TaskManagementSystem.TaskService.Core.Aggregates;
using TaskManagementSystem.TaskService.Core.Interfaces;
using TaskManagementSystem.TaskService.Infrastructure.DataAccess.ORM;

namespace TaskManagementSystem.TaskService.Infrastructure.DataAccess.Repositories;


public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskAggregate>> GetAllByBoardIdAsync(
        Guid boardId,
        CancellationToken cancellationToken
        )
    {
        return await _context.Tasks
            .Where(task => task.BoardId == boardId)
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<TaskAggregate>> GetAllByColumnIdAsync(Guid columnId, CancellationToken cancellationToken)
    {
        return await _context.Tasks
            .Where(task => task.ColumnId == columnId)
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<TaskAggregate?> GetByIdAsync(Guid taskId, CancellationToken cancellationToken)
    {
        return await _context.Tasks
            .Where(task => task.Id == taskId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public async Task CreateAsync(TaskAggregate task, CancellationToken cancellationToken)
    {
        await _context.Tasks.AddAsync(task, cancellationToken);
    }

    public async Task<long?> GetLatestRankAsync(Guid boardId, CancellationToken cancellationToken)
    {
        return await _context.Tasks
            .Where(task => task.BoardId == boardId)
            .Select(task => (long?)task.Rank)
            .MaxAsync(cancellationToken: cancellationToken) ?? NumeralRankOptions.Empty;
    }

    public async Task<List<TaskAggregate>> FilterAsync(Guid taskBoardId, Expression<Func<TaskAggregate, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _context.Tasks
            .Where(task => task.BoardId == taskBoardId)
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public void Update(TaskAggregate task)
    {
        _context.Tasks.Update(task);
    }

    public async Task<int> DeleteAsync(Guid taskId, CancellationToken cancellationToken)
    {
        var result = await _context.Tasks
            .Where(task => task.Id == taskId)
            .ExecuteDeleteAsync(cancellationToken);

        return result;
    }
    public async Task<bool> ExistsAsync(Expression<Func<TaskAggregate, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _context.Tasks
            .AnyAsync(predicate, cancellationToken);
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
