using System.Collections.Concurrent;
using System.Linq.Expressions;
using TaskManagementSystem.SharedLib.Algorithms.NumeralRank;
using TaskManagementSystem.TaskService.Core.Aggregates;
using TaskManagementSystem.TaskService.Core.Interfaces;

namespace TaskManagementSystem.TaskService.Tests.Mocks.Repositories
{
    public sealed class InMemoryTaskRepository : ITaskRepository
    {
        private readonly ConcurrentDictionary<Guid, TaskAggregate> _tasks = new();

        public InMemoryTaskRepository() { }

        public InMemoryTaskRepository(IEnumerable<TaskAggregate> seed)
        {
            foreach (var t in seed)
                _tasks[t.Id] = t; // без копии
        }

        // ------------------- Reads -------------------

        public Task<List<TaskAggregate>> GetAllByBoardIdAsync(Guid boardId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var list = _tasks.Values
                .Where(t => t.BoardId == boardId)
                .ToList();

            return Task.FromResult(list);
        }

        public Task<List<TaskAggregate>> GetAllByColumnIdAsync(Guid columnId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var list = _tasks.Values
                .Where(t => t.ColumnId == columnId)
                .ToList();

            return Task.FromResult(list);
        }

        public Task<TaskAggregate?> GetByIdAsync(Guid taskId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _tasks.TryGetValue(taskId, out var found);
            return Task.FromResult(found);
        }

        public Task<List<TaskAggregate>> FilterAsync(
            Guid taskBoardId,
            Expression<Func<TaskAggregate, bool>> predicate,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var compiled = predicate.Compile();

            var list = _tasks.Values
                .Where(t => t.BoardId == taskBoardId)
                .Where(compiled)
                .ToList();

            return Task.FromResult(list);
        }

        public Task<bool> ExistsAsync(Expression<Func<TaskAggregate, bool>> predicate, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var compiled = predicate.Compile();
            var exists = _tasks.Values.Any(compiled);
            return Task.FromResult(exists);
        }

        public Task<long?> GetLatestRankAsync(Guid boardId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var ranks = _tasks.Values
                .Where(t => t.BoardId == boardId)
                .Select(t => (long?)t.Rank);

            var max = ranks.Any() ? ranks.Max() : null;

            // как в твоём EF-репозитории
            return Task.FromResult(max);
        }

        // ------------------- Writes -------------------

        public Task CreateAsync(TaskAggregate task, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (task.Id == Guid.Empty)
                throw new InvalidOperationException("TaskAggregate.Id must be non-empty.");

            if (!_tasks.TryAdd(task.Id, task))
                throw new InvalidOperationException($"Task with Id {task.Id} already exists.");

            return Task.CompletedTask;
        }

        public void Update(TaskAggregate task)
        {
            if (task.Id == Guid.Empty)
                throw new InvalidOperationException("TaskAggregate.Id must be non-empty.");

            _tasks.AddOrUpdate(task.Id, task, (_, __) => task);
        }

        public Task<int> DeleteAsync(Guid taskId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var removed = _tasks.TryRemove(taskId, out _);
            return Task.FromResult(removed ? 1 : 0);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(0); // in-memory no-op
        }

        // Утилиты для тестов
        public void Seed(params TaskAggregate[] tasks)
        {
            foreach (var t in tasks)
                _tasks[t.Id] = t;
        }

        public IReadOnlyCollection<TaskAggregate> Snapshot() =>
            _tasks.Values.ToList().AsReadOnly();
    }
}
