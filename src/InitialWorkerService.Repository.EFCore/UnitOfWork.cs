using InitialWorkerService.Domain;
using InitialWorkerService.Domain.Logs;
using InitialWorkerService.Repository.EFCore.Logs;

namespace InitialWorkerService.Repository.EFCore
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext dbContext;

        private ILogsRepository logs;

        public UnitOfWork(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #region Repositories

        public ILogsRepository Logs => logs ??= new LogsRepository(dbContext);

        #endregion

        public Task<int> Complete() => dbContext.SaveChangesAsync();
    }
}
