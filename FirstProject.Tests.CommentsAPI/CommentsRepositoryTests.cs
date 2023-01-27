using AutoMapper;
using EntityFrameworkCore.Testing.Moq;
using FirstProject.CommentsAPI;
using FirstProject.CommentsAPI.Interfaces;
using FirstProject.CommentsAPI.Models;
using FirstProject.CommentsAPI.Models.DTO;
using FirstProject.CommentsAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Tests.CommentsAPI
{
    public class CommentsRepositoryTests
    {
        private IMapper _mapper;
        private CommentsDbContext _context;
        private ICommentsRepository _repository;

        [SetUp]
        public void Setup()
        {
            _mapper = new MapperConfiguration(mc => mc.RegisterMaps()).CreateMapper();

            var dbContextOptions = new DbContextOptionsBuilder<CommentsDbContext>().UseInMemoryDatabase("test").Options;
            _context = Create.MockedDbContextFor<CommentsDbContext>(dbContextOptions);

            _repository = new CommentsRepository(_context, _mapper);
        }

        [TearDown]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }

        [Test]
        public async Task CreateComment_NormalCreate_ReturnsDTO()
        {
            CommentDTO input = new()
            {
                UserId = Guid.NewGuid(),
                ArticleId = Guid.NewGuid(),
                Content = "testcontent"
            };

            var result = await _repository.CreateComment(input, default);

            Assert.Multiple(() =>
            {
                Assert.That(result.UserId, Is.EqualTo(input.UserId));
                Assert.That(result.ArticleId, Is.EqualTo(input.ArticleId));
                Assert.That(result.Content, Is.EqualTo(input.Content));
            });
        }

        [Test]
        public void CreateComment_NullModel_ReturnsException()
        {
            CommentDTO input = null!;

            async Task Check() 
            {
                await _repository.CreateComment(input, default); 
            }

            Assert.CatchAsync(Check);
        }

        [Test]
        public void CreateComment_EmptyUserId_ReturnsException()
        {
            CommentDTO input = new()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Empty,
                ArticleId = Guid.NewGuid(),
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow
            };

            async Task Check()
            {
                await _repository.CreateComment(input, default);
            }

            Assert.CatchAsync(Check);
        }

        [Test]
        public void CreateComment_EmptyArticleId_ReturnsException()
        {
            CommentDTO input = new()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ArticleId = Guid.Empty,
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow
            };

            async Task Check()
            {
                await _repository.CreateComment(input, default);
            }

            Assert.CatchAsync(Check);
        }

        [Test]
        public void CreateComment_EmptyContent_ReturnsException()
        {
            CommentDTO input = new()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ArticleId = Guid.NewGuid(),
                Content = string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            async Task Check()
            {
                await _repository.CreateComment(input, default);
            }

            Assert.CatchAsync(Check);
        }

        [Test]
        public async Task GetCommentsByArticleId_ExistedId_ReturnsCollection()
        {
            var articleId = Guid.NewGuid();

            Comment input1 = new()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ArticleId = articleId,
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow - TimeSpan.FromHours(1)
            };
            Comment input2 = new()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ArticleId = Guid.NewGuid(),
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(input1);
            _context.Comments.Add(input2);
            _context.SaveChanges();

            IEnumerable<CommentDTO> expected = new List<CommentDTO>() { _mapper.Map<CommentDTO>(input1) };

            var result = await _repository.GetCommentsByArticleId(articleId, 0, 5, default);

            Assert.Multiple(() =>
            {
                Assert.That(result.Count(), Is.EqualTo(expected.Count()));
                Assert.That(result.First(), Is.EqualTo(expected.First()));
            });
        }

        [Test]
        public async Task GetCommentsByArticleId_NotExistedId_ReturnsEmptyCollection()
        {
            Comment input1 = new()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ArticleId = Guid.NewGuid(),
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow - TimeSpan.FromHours(1)
            };
            Comment input2 = new()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ArticleId = Guid.NewGuid(),
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(input1);
            _context.Comments.Add(input2);
            _context.SaveChanges();

            var result = await _repository.GetCommentsByArticleId(Guid.NewGuid(), 0, 5, default);

            Assert.That(result.Count(), Is.Zero);
        }

        [Test]
        public void GetCommentsByArticleId_EmptyId_ReturnsException()
        {
            var id = Guid.Empty;

            async Task Check()
            {
                await _repository.GetCommentsByArticleId(id, 0, 5, default);
            }

            Assert.CatchAsync(Check);
        }

        [Test]
        public void GetCommentsByArticleId_IndexBelowZero_ReturnsException()
        {
            async Task Check()
            {
                await _repository.GetCommentsByArticleId(Guid.NewGuid(), -5, 5, default);
            }

            Assert.CatchAsync(Check);
        }

        [Test]
        public void GetCommentsByArticleId_CountBelowZero_ReturnsException()
        {
            async Task Check()
            {
                await _repository.GetCommentsByArticleId(Guid.NewGuid(), 0, -5, default);
            }

            Assert.CatchAsync(Check);
        }

        [Test]
        public async Task GetCommentsCountByArticleId_ExistedId_ReturnsCount()
        {
            var articleId = Guid.NewGuid();
            Comment input1 = new()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ArticleId = Guid.NewGuid(),
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow - TimeSpan.FromHours(1)
            };
            Comment input2 = new()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ArticleId = articleId,
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(input1);
            _context.Comments.Add(input2);
            _context.SaveChanges();

            var result = await _repository.GetCommentsCountByArticleId(articleId, default);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task GetCommentsCountByArticleId_NotExistedId_ReturnsException()
        {
            Comment input1 = new()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ArticleId = Guid.NewGuid(),
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow - TimeSpan.FromHours(1)
            };
            Comment input2 = new()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ArticleId = Guid.NewGuid(),
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(input1);
            _context.Comments.Add(input2);
            _context.SaveChanges();

            var result = await _repository.GetCommentsCountByArticleId(Guid.NewGuid(), default);

            Assert.That(result, Is.Zero);
        }

        [Test]
        public void GetCommentsCountByArticleId_EmptyId_ReturnsException()
        {
            var id = Guid.Empty;

            async Task Check()
            {
                await _repository.GetCommentsCountByArticleId(id, default);
            }

            Assert.CatchAsync(Check);
        }

        [Test]
        public async Task UpdateComment_NormalUpdate_ReturnsDTO()
        {
            Comment input = new()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ArticleId = Guid.NewGuid(),
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow
            };
            CommentDTO expected = new()
            {
                Id = input.Id,
                UserId = input.UserId,
                ArticleId = input.ArticleId,
                Content = "changedcontent",
                CreatedAt = input.CreatedAt
            };

            _context.Comments.Add(input);
            _context.SaveChanges();

            var result = await _repository.UpdateComment(expected, default);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void UpdateComment_NotExistedCommentId_ReturnsException()
        {
            Comment input = new()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ArticleId = Guid.NewGuid(),
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow
            };
            CommentDTO expected = new()
            {
                Id = Guid.NewGuid(),
                UserId = input.UserId,
                ArticleId = input.ArticleId,
                Content = "testcontent",
                CreatedAt = input.CreatedAt
            };

            _context.Comments.Add(input);
            _context.SaveChanges();

            async Task Check()
            {
                await _repository.UpdateComment(expected, default);
            }

            Assert.CatchAsync(Check);
        }

        [Test]
        public void UpdateComment_NullModel_ReturnsException()
        {
            CommentDTO expected = null!;

            async Task Check()
            {
                await _repository.UpdateComment(expected, default);
            }

            Assert.CatchAsync(Check);
        }

        [Test]
        public void UpdateComment_EmptyCommentId_ReturnsException()
        {
            CommentDTO input = new()
            {
                Id = Guid.Empty,
                UserId = Guid.NewGuid(),
                ArticleId = Guid.NewGuid(),
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow
            };

            async Task Check()
            {
                await _repository.UpdateComment(input, default);
            }

            Assert.CatchAsync(Check);
        }

        [Test]
        public void UpdateComment_EmptyUserId_ReturnsException()
        {
            Comment input = new()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ArticleId = Guid.NewGuid(),
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow
            };
            CommentDTO expected = new()
            {
                Id = input.Id,
                UserId = Guid.Empty,
                ArticleId = input.ArticleId,
                Content = "testcontent",
                CreatedAt = input.CreatedAt
            };

            _context.Comments.Add(input);
            _context.SaveChanges();

            async Task Check()
            {
                await _repository.UpdateComment(expected, default);
            }

            Assert.CatchAsync(Check);
        }

        [Test]
        public void UpdateComment_EmptyArticleId_ReturnsException()
        {
            Comment input = new()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ArticleId = Guid.NewGuid(),
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow
            };
            CommentDTO expected = new()
            {
                Id = input.Id,
                UserId = input.UserId,
                ArticleId = Guid.Empty,
                Content = "testcontent",
                CreatedAt = input.CreatedAt
            };

            _context.Comments.Add(input);
            _context.SaveChanges();

            async Task Check()
            {
                await _repository.UpdateComment(expected, default);
            }

            Assert.CatchAsync(Check);
        }

        [Test]
        public void UpdateComment_EmptyContent_ReturnsException()
        {
            Comment input = new()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ArticleId = Guid.NewGuid(),
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow
            };
            CommentDTO expected = new()
            {
                Id = input.Id,
                UserId = input.UserId,
                ArticleId = input.ArticleId,
                Content = string.Empty,
                CreatedAt = input.CreatedAt
            };

            _context.Comments.Add(input);
            _context.SaveChanges();

            async Task Check()
            {
                await _repository.UpdateComment(expected, default);
            }

            Assert.CatchAsync(Check);
        }

        [Test]
        public async Task DeleteComment_ExistedId_ReturnsTrue()
        {
            var id = Guid.NewGuid();

            Comment input = new()
            {
                Id = id,
                UserId = Guid.NewGuid(),
                ArticleId = Guid.NewGuid(),
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(input);
            _context.SaveChanges();

            var result = await _repository.DeleteComment(id, default);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task DeleteComment_NotExistedId_ReturnsFalse()
        {
            var id = Guid.NewGuid();

            var result = await _repository.DeleteComment(id, default);

            Assert.That(result, Is.False);
        }

        [Test]
        public void DeleteComment_EmptyId_ReturnsException()
        {
            var id = Guid.Empty;

            async Task Check()
            {
                await _repository.DeleteComment(id, default);
            }

            Assert.CatchAsync(Check);
        }
    }
}