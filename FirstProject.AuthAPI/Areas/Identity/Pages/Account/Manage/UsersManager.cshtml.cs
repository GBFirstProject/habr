using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FirstProject.AuthAPI.Models;
using FirstProject.AuthAPI.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FirstProject.AuthAPI.Areas.Identity.Pages.Account.Manage
{
    public class UsersManagerModel : PageModel
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserRepository _userRepository;
        public List<ApplicationUser> Users;

        public UsersManagerModel(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IUserRepository userRepository) 
        {
            //_userManager = userManager;
            //_signInManager = signInManager;
            //_roleManager = roleManager;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Users = (List<ApplicationUser>)await _userRepository.GetUsers();
            return Page();
        }
    }
}
