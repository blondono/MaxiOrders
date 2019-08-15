using MaxiOrders.Back.Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MaxiOrders.Back.Infrastructure.Repository.Repositories
{   
    public class UnitOfWork<TContext> : Disposable, IUnitOfWork
    where TContext : DbContext, new()
    {
        public virtual int Commit()
        {
            return DataContext.SaveChanges();
        }

        public virtual Task<int> CommitAsync()
        {
            return DataContext.SaveChangesAsync();
        }

        //protected readonly DbContext DataContext;
        private DbContext DataContext;
        public DbContext GetDatabase()
        {
            return DataContext ?? (DataContext = new TContext());
            //return DataContext != null && DataContext.Database.GetDbConnection().State == ConnectionState.Open ? DataContext : (DataContext = new TContext());
        }

        /*public UnitOfWork()
        {
            DataContext = new TContext();
            var optionsBuilder = new DbContextOptionsBuilder<DBContext>();
            optionsBuilder.UseSqlServer("");
            DataContext = new DBContext(optionsBuilder.Options);
        }*/

        protected override void DisposeCore()
        {
            if (DataContext != null)
                DataContext.Dispose();
        }

        public void Rollback()
        {
            DataContext
                .ChangeTracker
                .Entries()
                .ToList()
                .ForEach(x => x.Reload());
        }
    }
}