using FirstProject.AuthAPI.Data;
using FirstProject.AuthAPI.Models;
using Org.BouncyCastle.Crypto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.AuthAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {
            var users = _db.Users.ToList();
            return users;
        }
    }
}
