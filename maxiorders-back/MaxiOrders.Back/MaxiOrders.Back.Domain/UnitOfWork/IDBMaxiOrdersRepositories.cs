using MaxiOrders.Back.Common;
using MaxiOrders.Back.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MaxiOrders.Back.Domain.UnitOfWork
{
    public interface IDBMaxiOrdersRepositories : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// Repositorio para Company.
        /// </summary>
        IRepository<Company> Companies { get; }

        /// <summary>
        /// Registra auditoría.
        /// </summary>
        /// <param name="actiosAudit"></param>
        /// <returns></returns>
        //Task<bool> SaveAudit(ActionsAudit actiosAudit);

        /// <summary>
        /// Información de Device.
        /// </summary>
        IRepository<Device> Devices { get; }

        /// <summary>
        /// Repositorio para la tabla HeadQuarter.
        /// </summary>
        IRepository<HeadQuarter> HeadQuarters { get; }

        /// <summary>
        /// Repositorio para la tabla User.
        /// </summary>
        IRepository<User> Users { get; }
    }
}
