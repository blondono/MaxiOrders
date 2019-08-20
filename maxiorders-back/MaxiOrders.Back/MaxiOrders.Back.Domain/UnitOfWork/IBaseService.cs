using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MaxiOrders.Back.Domain.UnitOfWork
{
    public interface IBaseService<TDTO>
        where TDTO : class
    {

        IEnumerable<TDTO> GetAll();
        TDTO Create(TDTO dto);
        void Delete(object id);
        void Update(object id, TDTO dto);
        Task CreateAsync(TDTO dto);
        Task DeleteAsync(object id);
        Task UpdateAsync(object id, TDTO dto);
        TDTO FindById(object Id);
        object ExecuteKendoResult(object request);


    }
}
