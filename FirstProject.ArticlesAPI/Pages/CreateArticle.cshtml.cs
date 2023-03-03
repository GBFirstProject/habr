using FirstProject.ArticlesAPI.Models.Requests;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FirstProject.Web.Pages
{
   [Authorize]
   public class CreateArticleModel : PageModel
    {
       
       public void OnGet()
       {
       
       }

      public void OnPost()        
       {
                  
       }
    }
}
