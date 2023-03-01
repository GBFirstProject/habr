using Duende.Bff;



using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FirstProject.Web.Pages
{

   [BffApi]
   public class CreateArticleModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly string _articleAPIUrl;

        public CreateArticleModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
           _httpClientFactory= httpClientFactory;
            _articleAPIUrl = configuration.GetSection("ServicesUrl")["ArticlesAPI"];
        }       
       public void OnGet()
       {
       
       } 
       public async Task OnPostAsync([FromForm] string title, bool commentsEnabled, string textHtml) 
       { 
       //     var client = _httpClientFactory.CreateClient();
       //     var content = JsonContent.Create(new CreateArticleRequest()
       //     { 
       //     Title = title,
       //     CommentsEnabled = commentsEnabled,
       //     TextHtml = textHtml   
       //     });
       //     var request = new HttpRequestMessage(HttpMethod.Post, _articleAPIUrl + "/articles" +"/add-article");
       //     request.Content = content;
       //     await client.SendAsync(request);
         
       }
    }
}
