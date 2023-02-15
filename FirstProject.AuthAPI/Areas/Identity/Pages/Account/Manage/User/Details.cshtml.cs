using FirstProject.AuthAPI.Data;
using FirstProject.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstProject.AuthAPI.Areas.Identity.Pages.Account.Manage.User
{
    public class DetailsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailsModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public ApplicationUser User { get; set; }
        public IList<string> Role { get; set; } = new List<string>();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }


            User = await _userManager.FindByIdAsync(id);

            Role = _userManager.GetRolesAsync(User).Result;

            if (User == null)
            {
                return NotFound();
            }


            return Page();
        }
    }
}
