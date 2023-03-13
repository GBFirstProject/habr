using FirstProject.Messages;
using FirstProject.NotificationAPI.Producers;

using Microsoft.AspNetCore.Authorization;
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
         [Route("moderatorRequested")]
         
         public  async Task<bool> ModeratorRequested(ModeratorRequested notification)
         {
            var result = await notifier.PublishModeratorRequested(notification);
            return result;          
         }

        [HttpPost]
        [Route("liked")]

        public async Task<bool> ArticleLiked(ArticleLiked notification)
        {
            
            var result = await notifier.PublishArticleLiked(notification);
            return result;
        }

        [HttpPost]
        [Route("disliked")]

        public async Task<bool> ArticleDisliked(ArticleDisliked notification)
        {
            var result = await notifier.PublishArticleDisliked(notification); 
            return result;
        }


        [HttpPost]
        [Route("commented")]

        public async Task<bool> ArticleCommented(ArticleCommented notification)
        {
            var result =await notifier.PublishArticleCommented(notification);
            return result;
        }

        [HttpPost]
        [Route("approved")]

        public async Task<bool> ArticleApproved(ArticleApproved notification)
        {
            var result = await notifier.PublishArticleApproved(notification);
            return result;
        }


        [HttpPost]
        [Route("rejected")]

        public async Task<bool> ArticleRejected(ArticleRejected notification)
        {
            var result = await notifier.PublishArticleRejected(notification);
            return result;
        }





    }
}
