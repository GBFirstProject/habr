using HabrParser.Models.APIComments;
using Microsoft.EntityFrameworkCore;

namespace HabrParser.Database.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly CommentsDbContext _context;

        public CommentsRepository(CommentsDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateComment(Comment comment, CancellationToken cts)
        {
            try
            {
                //if (comment == null)
                //{
                //    throw new ArgumentNullException("Comment was null");
                //}

                //if (comment.UserId == Guid.Empty)
                //{
                //    throw new ArgumentException("User Id was empty");
                //}

                //if (comment.ArticleId == Guid.Empty)
                //{
                //    throw new ArgumentException("Article Id was empty");
                //}

                //if (comment.Content == string.Empty)
                //{
                //    throw new ArgumentException("Content was empty");
                //}

                if(comment.UserId==Guid.Empty)
                {

                }

                await _context.AddAsync(comment, cts);

                await _context.SaveChangesAsync(cts);

                return comment;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> CommentsAlreadyExists(Guid articleId, CancellationToken cts)
        {
            return await _context.Comments.AnyAsync(s => s.ArticleId == articleId, cts);
        }
    }
}
