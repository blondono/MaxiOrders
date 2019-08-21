using AutoMapper.Configuration;
using MaxiOrders.Back.Domain.Entities;
using MaxiOrders.Back.Domain.Entities.Models.Response;
using MaxiOrders.Back.Domain.UnitOfWork;
using System;
using MaxiOrders.Back.Common.Enums;
using System.Threading.Tasks;

namespace MaxiOrders.Back.Domain.Services.Master
{
    public interface IDeviceService
    {
        Task<Response<Device>> Add(Device device);
        Task<Response<Device>> Get();
        Task<Response<Device>> Get(long id);
        Task<Response<Device>> Update(Device device);
        Task<Response<Device>> Delete(long id);
    }
    public class DeviceService : IDeviceService
    {
        private readonly IDBMaxiOrdersRepositories _iDBMaxiOrdersRepositories;
        public DeviceService(IDBMaxiOrdersRepositories iDBMaxiOrdersRepositories)
        {
            _iDBMaxiOrdersRepositories = iDBMaxiOrdersRepositories;
        }

        public virtual async Task<Response<Device>> Get()
        {

            Response<Device> response = new Response<Device>();
            try
            {
                response.Code = EnumResponseCode.OK.GetHashCode();
                response.Message = EnumResponseCode.OK.ToString();
                response.List = _iDBMaxiOrdersRepositories.Devices.GetAll();
            }
            catch (Exception ex)
            {
                response.Code = EnumResponseCode.ServerError.GetHashCode();
                response.Message = "Error consultando la lista de equipos";
            }
            return response;
        }

        public virtual async Task<Response<Device>> Get(long id)
        {

            Response<Device> response = new Response<Device>();
            try
            {
                response.Code = EnumResponseCode.OK.GetHashCode();
                response.Message = EnumResponseCode.OK.ToString();
                response.Content = _iDBMaxiOrdersRepositories.Devices.Get(x => x.IdDevice == id);
            }
            catch (Exception ex)
            {
                response.Code = EnumResponseCode.ServerError.GetHashCode();
                response.Message = "Error consultando la lista de equipos";
            }
            return response;
        }

        public virtual async Task<Response<Device>> Add(Device device)
        {
            Response<Device> response = new Response<Device>();
            try
            {
                _iDBMaxiOrdersRepositories.Devices.Add(device);
                _iDBMaxiOrdersRepositories.Commit();
                response.Code = EnumResponseCode.OK.GetHashCode();
                response.Message = EnumResponseCode.OK.ToString();
                response.Content = null;
            }
            catch (Exception ex)
            {
                response.Code = EnumResponseCode.ServerError.GetHashCode();
                response.Message = "Error al guardar el equipo";
            }
            return response;
        }

        public virtual async Task<Response<Device>> Update(Device device)
        {
            Response<Device> response = new Response<Device>();
            try
            {
                _iDBMaxiOrdersRepositories.Devices.Update(device);
                _iDBMaxiOrdersRepositories.Commit();
                response.Code = EnumResponseCode.OK.GetHashCode();
                response.Message = EnumResponseCode.OK.ToString();
                response.Content = null;
            }
            catch (Exception ex)
            {
                response.Code = EnumResponseCode.ServerError.GetHashCode();
                response.Message = "Error al actualizar el equipo";
            }
            return response;
        }

        public virtual async Task<Response<Device>> Delete(long id)
        {
            Response<Device> response = new Response<Device>();
            try
            {
                _iDBMaxiOrdersRepositories.Devices.Delete(id);
                _iDBMaxiOrdersRepositories.Commit();
                response.Code = EnumResponseCode.OK.GetHashCode();
                response.Message = EnumResponseCode.OK.ToString();
                response.Content = null;
            }
            catch (Exception ex)
            {
                response.Code = EnumResponseCode.ServerError.GetHashCode();
                response.Message = "Error al eliminar el equipo";
            }
            return response;
        }
    }
}
