using FirstProject.Messages;
using FirstProject.NotificationAPI.Producers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject.NotificationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly Notifier notifier;

        public NotificationsController(Notifier notifier)
        {
            this.notifier=notifier;
        }

         [HttpPost]
         
         public Task<bool> ModeratorRequested(ModeratorRequested notification)
         {
            var result = notifier.PublishModeratorRequested(notification);
            return result;          
         }

        [HttpPost]
        [Route("/liked")]

        public Task<bool> ArticleLiked(ArticleLiked notification)
        {
            
            var result = notifier.PublishArticleLiked(notification);
            return result;
        }

        [HttpPost]
        [Route("/disliked")]

        public Task<bool> ArticleDisliked(ArticleDisliked notification)
        {
            var result = notifier.PublishArticleDisliked(notification); 
            return result;
        }


        [HttpPost]
        [Route("/commented")]

        public Task<bool> ArticleCommented(ArticleCommented notification)
        {
            var result = notifier.PublishArticleCommented(notification);
            return result;
        }







    }
}
