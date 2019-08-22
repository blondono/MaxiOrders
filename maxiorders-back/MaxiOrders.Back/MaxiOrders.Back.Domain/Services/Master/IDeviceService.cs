using AutoMapper.Configuration;
using MaxiOrders.Back.Domain.Entities;
using MaxiOrders.Back.Domain.Entities.Models.Response;
using MaxiOrders.Back.Domain.UnitOfWork;
using System;
using MaxiOrders.Back.Common.Enums;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MaxiOrders.Back.Domain.Services.Master
{
    public interface IDeviceService
    {
        Task<IEnumerable<Device>> Get();
        Task<Device> Get(long id);
        Task<Device> Add(Device device);
        void Update(Device device);
        void Delete(long id);
    }
    public class DeviceService : IDeviceService
    {
        private readonly IDBMaxiOrdersRepositories _iDBMaxiOrdersRepositories;
        public DeviceService(IDBMaxiOrdersRepositories iDBMaxiOrdersRepositories)
        {
            _iDBMaxiOrdersRepositories = iDBMaxiOrdersRepositories;
        }

        public virtual async Task<IEnumerable<Device>> Get()
        {
            return _iDBMaxiOrdersRepositories.Devices.GetAll();
        }

        public virtual async Task<Device> Get(long id)
        {
            return _iDBMaxiOrdersRepositories.Devices.Get(x => x.IdDevice == id);
        }

        public virtual async Task<Device> Add(Device device)
        {
            _iDBMaxiOrdersRepositories.Devices.Add(device);
            _iDBMaxiOrdersRepositories.Commit();
            return device;
        }

        public virtual async void Update(Device device)
        {
            _iDBMaxiOrdersRepositories.Devices.Update(device);
            _iDBMaxiOrdersRepositories.Commit();
        }

        public virtual async void Delete(long id)
        {
            _iDBMaxiOrdersRepositories.Devices.Delete(id);
            _iDBMaxiOrdersRepositories.Commit();
        }
    }
}
