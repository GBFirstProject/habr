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
    public class BlockingModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public BlockingModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public ApplicationUser User { get; set; }
        public bool IsBlocked { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User = await _userManager.FindByIdAsync(id);
            IsBlocked = User.IsBlocked;

            if (User == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool isBlocked)
        {
            
            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            User = await _userManager.FindByIdAsync(User.Id);
            User.IsBlocked = isBlocked;
            IdentityResult result = await _userManager.UpdateAsync(User);

            if (result.Succeeded)
            {
                return RedirectToPage("../UsersManager");
            }
            return Page();
        }
    }
}
