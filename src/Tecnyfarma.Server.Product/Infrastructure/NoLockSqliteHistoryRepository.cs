using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Sqlite.Migrations.Internal;

namespace Tecnyfarma.Server.Product.Infrastructure;

public sealed class NoLockSqliteHistoryRepository : SqliteHistoryRepository
{
    public NoLockSqliteHistoryRepository(HistoryRepositoryDependencies dependencies) : base(dependencies)
    {
    }

    public override IMigrationsDatabaseLock AcquireDatabaseLock() => new NoOpMigrationsLock(this);

    public override Task<IMigrationsDatabaseLock> AcquireDatabaseLockAsync(
        CancellationToken cancellationToken = default)
        => Task.FromResult<IMigrationsDatabaseLock>(new NoOpMigrationsLock(this));

    private sealed class NoOpMigrationsLock(HistoryRepository historyRepository) : IMigrationsDatabaseLock
    {
        public IHistoryRepository HistoryRepository { get; } = historyRepository;

        public void Dispose()
        {
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}