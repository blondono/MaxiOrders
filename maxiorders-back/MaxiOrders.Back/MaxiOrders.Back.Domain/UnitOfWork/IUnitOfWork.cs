using System.Threading.Tasks;

namespace MaxiOrders.Back.Domain.UnitOfWork
{
    public interface IUnitOfWork
    {
        int Commit();
        Task<int> CommitAsync();
        void Rollback();
    }
}
