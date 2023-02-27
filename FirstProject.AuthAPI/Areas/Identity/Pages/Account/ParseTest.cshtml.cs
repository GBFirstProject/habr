using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.Threading;

namespace FirstProject.AuthAPI.Areas.Identity.Pages.Account
{
    public class ParseTestModel : PageModel
    {
        private static int _progress;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<JsonResult> OnPostParseAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            _progress = 0;

            for (int i = 0; i < 100; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Perform some processing here
                await Task.Delay(1000);

                _progress = i + 1;
            }

            return new JsonResult(new { message = "Parsing complete" });
        }

        public JsonResult OnGetGetProgress()
        {
            return new JsonResult(new { percentComplete = _progress });
        }
    }
}
