﻿using FirstProject.CommentsAPI.Interfaces;
using FirstProject.CommentsAPI.Models.DTO;
using FirstProject.CommentsAPI.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.CommentsAPI.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly CommentsDbContext _context;
        private readonly IMapper _mapper;

        public CommentsRepository(CommentsDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CommentDTO> CreateComment(CommentDTO comment, CancellationToken cts)
        {
            try
            {
                var entry = _mapper.Map<Comment>(comment);

                await _context.AddAsync(entry, cts);

                await _context.SaveChangesAsync(cts);

                return _mapper.Map<CommentDTO>(entry);
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsByArticleId(Guid articleId, int index, int count, CancellationToken cts)
        {
            try
            {
                var entries = await _context.Comments.Where(s => s.ArticleId == articleId).ToListAsync(cts);
                entries.Sort((a, b) => b.CreatedAt.CompareTo(a.CreatedAt));

                if (index + count > entries.Count)
                {
                    var result_entries = entries.GetRange(index, entries.Count - index);
                    return _mapper.Map<IEnumerable<CommentDTO>>(result_entries);
                }
                else
                {
                    var result_entries = entries.GetRange(index, count);
                    return _mapper.Map<IEnumerable<CommentDTO>>(result_entries);
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> GetCommentsCountByArticleId(Guid articleId, CancellationToken cts)
        {
            try
            {
                var entries = await _context.Comments.Where(s => s.ArticleId == articleId).ToListAsync(cts);

                return entries.Count;
            }
            catch
            {
                throw;
            }
        }

        public async Task<CommentDTO> UpdateComment(CommentDTO comment, CancellationToken cts)
        {
            try
            {
                var entry = await _context.Comments.FirstOrDefaultAsync(s => s.Id == comment.Id, cts);
                if (entry == null)
                {
                    return await CreateComment(comment, cts);
                }

                _context.Comments.Update(_mapper.Map<Comment>(comment));

                await _context.SaveChangesAsync(cts);

                return _mapper.Map<CommentDTO>(entry);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteComment(Guid id, CancellationToken cts)
        {
            try
            {
                var entry = await _context.Comments.FirstOrDefaultAsync(s => s.Id == id, cts);
                if (entry == null)
                {
                    return false;
                }

                _context.Comments.Remove(entry);

                await _context.SaveChangesAsync(cts);

                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}