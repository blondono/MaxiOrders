using AutoMapper.Configuration;
using MaxiOrders.Back.Domain.Entities;
using MaxiOrders.Back.Domain.Entities.Models.Response;
using MaxiOrders.Back.Domain.UnitOfWork;
using System;
using MaxiOrders.Back.Common.Enums;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MaxiOrders.Back.Domain.Services.Companies
{
    public interface IHeadQuarterService
    {
        Task<IEnumerable<HeadQuarter>> Get();
        Task<HeadQuarter> Get(long id);
        Task<HeadQuarter> Add(HeadQuarter HeadQuarter);
        void Update(HeadQuarter HeadQuarter);
        void Delete(long id);
    }
    public class HeadQuarterService : IHeadQuarterService
    {
        private readonly IDBMaxiOrdersRepositories _iDBMaxiOrdersRepositories;
        public HeadQuarterService(IDBMaxiOrdersRepositories iDBMaxiOrdersRepositories)
        {
            _iDBMaxiOrdersRepositories = iDBMaxiOrdersRepositories;
        }

        public virtual async Task<IEnumerable<HeadQuarter>> Get()
        {
            return _iDBMaxiOrdersRepositories.HeadQuarters.GetAll();
        }

        public virtual async Task<HeadQuarter> Get(long id)
        {
            return _iDBMaxiOrdersRepositories.HeadQuarters.Get(x => x.IdHeadQuarter == id);
        }

        public virtual async Task<HeadQuarter> Add(HeadQuarter HeadQuarter)
        {
            _iDBMaxiOrdersRepositories.HeadQuarters.Add(HeadQuarter);
            _iDBMaxiOrdersRepositories.Commit();
            return HeadQuarter;
        }

        public virtual async void Update(HeadQuarter HeadQuarter)
        {
            _iDBMaxiOrdersRepositories.HeadQuarters.Update(HeadQuarter);
            _iDBMaxiOrdersRepositories.Commit();
        }

        public virtual async void Delete(long id)
        {
            _iDBMaxiOrdersRepositories.Companies.Delete(id);
            _iDBMaxiOrdersRepositories.Commit();
        }
    }
}
