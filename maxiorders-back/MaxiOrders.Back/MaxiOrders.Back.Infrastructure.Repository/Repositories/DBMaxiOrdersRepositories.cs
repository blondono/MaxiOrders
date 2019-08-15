using MaxiOrders.Back.Common;
using MaxiOrders.Back.Domain.Entities;
using MaxiOrders.Back.Domain.UnitOfWork;
using MaxiOrders.Back.Infrastructure.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaxiOrders.Back.Infrastructure.Repository.Repositories
{
    public class DBMaxiOrdersRepositories : UnitOfWork<DBMaxiOrdersContext>, IDBMaxiOrdersRepositories
    {
        IRepository<Company> companies;
        IRepository<Device> devices;
        IRepository<HeadQuarter> headquarters;
        IRepository<User> users;

        /// <summary>
        /// Pedidos.
        /// </summary>
        public IRepository<Company> Companies => companies ?? (companies = new Repository<Company>(GetDatabase()));

        /// <summary>
        /// RegistroPedidos
        /// </summary>
        public IRepository<Device> Devices => devices ?? (devices = new Repository<Device>(GetDatabase()));

        /// <summary>
        /// Dependencias.
        /// </summary>
        public IRepository<HeadQuarter> HeadQuarters => headquarters ?? (headquarters = new Repository<HeadQuarter>(GetDatabase()));

        /// <summary>
        /// Información clientes.
        /// </summary>
        public IRepository<User> Users => users ?? (users = new Repository<User>(GetDatabase()));

  
        /// <summary>
        /// Guarda en la tabla de Auditoría.
        /// </summary>
        /// <param name="actiosAudit"></param>
        /// <returns></returns>
        //public async Task<bool> SaveAudit(ActionsAudit actiosAudit)
        //{
        //    var db = GetDatabase();
        //    var add = await db.AddAsync(actiosAudit);
        //    return add.State == Microsoft.EntityFrameworkCore.EntityState.Added;
        //}
    }
}
