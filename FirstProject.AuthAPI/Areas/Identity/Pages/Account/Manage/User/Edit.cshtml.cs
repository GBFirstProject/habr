using FirstProject.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstProject.AuthAPI.Areas.Identity.Pages.Account.Manage.User
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public ApplicationUser User { get; set; }
        public IList<string> Role { get; set; }
        public IList<string> UserRoles = new List<string>()
        {
            Config.User, Config.Moderator, Config.Admin
        };

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User = await _userManager.FindByIdAsync(id);
            Role = await _userManager.GetRolesAsync(User);

            if (User == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(List<string> roles)
        {
            
            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                User = await _userManager.FindByIdAsync(User.Id);
                Role = await _userManager.GetRolesAsync(User);
                
                foreach(var role in UserRoles)
                {
                    if(Role.Contains(role) && !roles.Contains(role))
                    {
                        await _userManager.RemoveFromRoleAsync(User, role);
                    }else if(!Role.Contains(role) && roles.Contains(role))
                    {
                        await _userManager.AddToRoleAsync(User, role);
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return RedirectToPage("../UsersManager");
        }
    }
}
