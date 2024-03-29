﻿using HabrParser.Interfaces;
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
                await _context.AddAsync(comment, cts);

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

        public async Task<int> SaveChanges(CancellationToken cts)
        {
            return await _context.SaveChangesAsync(cts);
        }
    }
}
