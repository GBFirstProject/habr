using FirstProject.AuthAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstProject.AuthAPI.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetUsers();
    }
}
