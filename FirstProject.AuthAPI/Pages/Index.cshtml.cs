using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProject.AuthAPI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public RedirectResult OnGet()
        {
            if ((bool)!HttpContext.User?.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Login");
            }
            return Redirect("/Identity/Account/Manage/Index");
        }
    }
}
