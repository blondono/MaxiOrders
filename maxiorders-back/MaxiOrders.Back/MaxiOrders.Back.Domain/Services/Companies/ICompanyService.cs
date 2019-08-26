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
    public interface ICompanyService
    {
        Task<IEnumerable<Company>> Get();
        Task<Company> Get(long id);
        Task<Company> Add(Company Company);
        void Update(Company Company);
        void Delete(long id);
    }
    public class CompanyService : ICompanyService
    {
        private readonly IDBMaxiOrdersRepositories _iDBMaxiOrdersRepositories;
        public CompanyService(IDBMaxiOrdersRepositories iDBMaxiOrdersRepositories)
        {
            _iDBMaxiOrdersRepositories = iDBMaxiOrdersRepositories;
        }

        public virtual async Task<IEnumerable<Company>> Get()
        {
            return _iDBMaxiOrdersRepositories.Companies.GetAll();
        }

        public virtual async Task<Company> Get(long id)
        {
            return _iDBMaxiOrdersRepositories.Companies.Get(x => x.IdCompany == id);
        }

        public virtual async Task<Company> Add(Company Company)
        {
            _iDBMaxiOrdersRepositories.Companies.Add(Company);
            _iDBMaxiOrdersRepositories.Commit();
            return Company;
        }

        public virtual async void Update(Company Company)
        {
            _iDBMaxiOrdersRepositories.Companies.Update(Company);
            _iDBMaxiOrdersRepositories.Commit();
        }

        public virtual async void Delete(long id)
        {
            _iDBMaxiOrdersRepositories.Companies.Delete(id);
            _iDBMaxiOrdersRepositories.Commit();
        }
    }
}
