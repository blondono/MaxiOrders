using MaxiOrders.Back.Domain.UnitOfWork;
using Microsoft.Extensions.Configuration;

namespace MaxiOrders.Back.Domain.Services.Users
{
    public interface IUserService
    {
    }

    public class UserService : IUserService
    {
        private readonly IDBMaxiOrdersRepositories _iDBMaxiOrdersRepositories;
        private readonly IConfiguration _configuration;
        public UserService(IDBMaxiOrdersRepositories iDBMaxiOrdersRepositories,
            IConfiguration configuration)
        {
            _iDBMaxiOrdersRepositories = iDBMaxiOrdersRepositories;
            _configuration = configuration;
        }
    }
}
