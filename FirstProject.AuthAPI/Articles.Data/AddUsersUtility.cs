using FirstProject.AuthAPI.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;

namespace FirstProject.AuthAPI.Articles.Data
{
    public class AddUsersUtility
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ArticlesDBContext _articlesDbContext;

        public AddUsersUtility(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ArticlesDBContext articlesDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _articlesDbContext = articlesDbContext;
        }

        public void CreateUsersFromAuthors()
        {
            foreach (var author in _articlesDbContext.Authors)
            {
                if (_userManager.Users.FirstOrDefault(u => u.UserName == author.NickName) != null)
                {
                    continue;
                }
                ApplicationUser newUser = new ApplicationUser()
                {
                    //Id = Guid.NewGuid().ToString(),
                    //SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = $"{author.NickName}@gmail.com",
                    Email = $"{author.NickName}@gmail.com",
                    EmailConfirmed = true,
                    //PhoneNumber = "111111111111",
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                };


                var result = _userManager.CreateAsync(newUser, "Admin123*").GetAwaiter().GetResult();
                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(newUser, Config.User).GetAwaiter().GetResult();
                    _userManager.AddClaimsAsync(newUser, new Claim[] {
                           new Claim(JwtClaimTypes.Name,newUser.FirstName+" "+ newUser.LastName),
                           new Claim(JwtClaimTypes.GivenName,newUser.FirstName),
                           new Claim(JwtClaimTypes.FamilyName,newUser.LastName),
                           new Claim(JwtClaimTypes.Role,Config.User),
                       });
                }

                var resUser = _userManager.Users.FirstOrDefault(u => u.UserName == author.NickName);
                var articleToUpdate = _articlesDbContext.Articles.FirstOrDefault(a => a.AuthorId == author.Id);
                articleToUpdate.AuthorId = System.Guid.Parse(resUser.Id);
                _articlesDbContext.Articles.Update(articleToUpdate);
                _articlesDbContext.SaveChanges();
            }
        }
    }
}
