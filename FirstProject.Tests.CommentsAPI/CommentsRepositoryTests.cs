using AutoMapper;
using EntityFrameworkCore.Testing.Moq;
using FirstProject.CommentsAPI;
using FirstProject.CommentsAPI.Data;
using FirstProject.CommentsAPI.Data.Models;
using FirstProject.CommentsAPI.Data.Models.DTO;
using FirstProject.CommentsAPI.Interfaces;
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
                Username = Guid.NewGuid(),
                ArticleId = Guid.NewGuid(),
                Content = "testcontent"
            };

            var result = await _repository.CreateComment(input, default);

            Assert.Multiple(() =>
            {
                Assert.That(result.Username, Is.EqualTo(input.Username));
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
                Username = Guid.Empty,
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
                Username = Guid.NewGuid(),
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
                Username = Guid.NewGuid(),
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

        [Test]
        public async Task LikeComment_Like_ReturnsDTO()
        {
            var commentId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            Comment input = new()
            {
                Id = commentId,
                UserId = Guid.NewGuid(),
                ArticleId = Guid.NewGuid(),
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(input);
            _context.SaveChanges();

            CommentDTO expected = new()
            {
                Id = input.Id,
                Username = input.UserId,
                ArticleId = input.ArticleId,
                Content = input.Content,
                CreatedAt = input.CreatedAt,
                Likes = new() { userId }
            };

            var result = await _repository.LikeComment(commentId, userId, default);

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(expected.Id));
                Assert.That(result.Username, Is.EqualTo(expected.Username));
                Assert.That(result.ArticleId, Is.EqualTo(expected.ArticleId));
                Assert.That(result.Content, Is.EqualTo(expected.Content));
                Assert.That(result.CreatedAt, Is.EqualTo(expected.CreatedAt));
                Assert.That(result.Likes, Is.EqualTo(expected.Likes));
            });
        }


        [Test]
        public void LikeComment_NotExistedCommentId_ReturnsException()
        {
            var commentId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            async Task Check()
            {
                await _repository.LikeComment(commentId, userId, default);
            }

            Assert.CatchAsync(Check);
        }


        [Test]
        public void LikeComment_EmptyCommentId_ReturnsException()
        {
            var commentId = Guid.Empty;
            var userId = Guid.NewGuid();

            async Task Check()
            {
                await _repository.LikeComment(commentId, userId, default);
            }

            Assert.CatchAsync(Check);
        }


        [Test]
        public void LikeComment_EmptyUserId_ReturnsException()
        {
            var commentId = Guid.NewGuid();
            var userId = Guid.Empty;

            async Task Check()
            {
                await _repository.LikeComment(commentId, userId, default);
            }

            Assert.CatchAsync(Check);
        }


        [Test]
        public async Task LikeComment_LikeWithAlreadyDislike_ReturnsDTO()
        {
            var commentId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            Comment input = new()
            {
                Id = commentId,
                UserId = Guid.NewGuid(),
                ArticleId = Guid.NewGuid(),
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow,
                Likes = new(),
                Dislikes = new() { userId }
            };

            _context.Comments.Add(input);
            _context.SaveChanges();

            CommentDTO expected = new()
            {
                Id = input.Id,
                Username = input.UserId,
                ArticleId = input.ArticleId,
                Content = input.Content,
                CreatedAt = input.CreatedAt,
                Likes = new() { userId },
                Dislikes = new()
            };

            var result = await _repository.LikeComment(commentId, userId, default);

            Assert.Multiple(() =>
            {
                Assert.That(result.Likes, Is.EqualTo(expected.Likes));
                Assert.That(result.Dislikes, Is.EqualTo(expected.Dislikes));
            });
        }


        [Test]
        public async Task DislikeComment_Like_ReturnsDTO()
        {
            var commentId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            Comment input = new()
            {
                Id = commentId,
                UserId = Guid.NewGuid(),
                ArticleId = Guid.NewGuid(),
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(input);
            _context.SaveChanges();

            CommentDTO expected = new()
            {
                Id = input.Id,
                Username = input.UserId,
                ArticleId = input.ArticleId,
                Content = input.Content,
                CreatedAt = input.CreatedAt,
                Dislikes = new() { userId }
            };

            var result = await _repository.DislikeComment(commentId, userId, default);

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(expected.Id));
                Assert.That(result.Username, Is.EqualTo(expected.Username));
                Assert.That(result.ArticleId, Is.EqualTo(expected.ArticleId));
                Assert.That(result.Content, Is.EqualTo(expected.Content));
                Assert.That(result.CreatedAt, Is.EqualTo(expected.CreatedAt));
                Assert.That(result.Dislikes, Is.EqualTo(expected.Dislikes));
            });
        }


        [Test]
        public void DislikeComment_NotExistedCommentId_ReturnsException()
        {
            var commentId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            async Task Check()
            {
                await _repository.DislikeComment(commentId, userId, default);
            }

            Assert.CatchAsync(Check);
        }


        [Test]
        public void DislikeComment_EmptyCommentId_ReturnsException()
        {
            var commentId = Guid.Empty;
            var userId = Guid.NewGuid();

            async Task Check()
            {
                await _repository.DislikeComment(commentId, userId, default);
            }

            Assert.CatchAsync(Check);
        }


        [Test]
        public void DislikeComment_EmptyUserId_ReturnsException()
        {
            var commentId = Guid.NewGuid();
            var userId = Guid.Empty;

            async Task Check()
            {
                await _repository.DislikeComment(commentId, userId, default);
            }

            Assert.CatchAsync(Check);
        }


        [Test]
        public async Task DislikeComment_DislikeWithAlreadyLike_ReturnsDTO()
        {
            var commentId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            Comment input = new()
            {
                Id = commentId,
                UserId = Guid.NewGuid(),
                ArticleId = Guid.NewGuid(),
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow,
                Likes = new() { userId },
                Dislikes = new()
            };

            _context.Comments.Add(input);
            _context.SaveChanges();

            CommentDTO expected = new()
            {
                Id = input.Id,
                Username = input.UserId,
                ArticleId = input.ArticleId,
                Content = input.Content,
                CreatedAt = input.CreatedAt,
                Likes = new(),
                Dislikes = new() { userId }
            };

            var result = await _repository.DislikeComment(commentId, userId, default);

            Assert.Multiple(() =>
            {
                Assert.That(result.Likes, Is.EqualTo(expected.Likes));
                Assert.That(result.Dislikes, Is.EqualTo(expected.Dislikes));
            });
        }


        [Test]
        public async Task ChangeContentComment_Change_ReturnsDTO()
        {
            var commentId = Guid.NewGuid();
            var changedcontent = "changedcontent";

            Comment input = new()
            {
                Id = commentId,
                UserId = Guid.NewGuid(),
                ArticleId = Guid.NewGuid(),
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(input);
            _context.SaveChanges();

            CommentDTO expected = new()
            {
                Id = input.Id,
                Username = input.UserId,
                ArticleId = input.ArticleId,
                Content = changedcontent,
                CreatedAt = input.CreatedAt
            };

            var result = await _repository.ChangeContentComment(commentId, changedcontent, default);

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo(expected.Id));
                Assert.That(result.Username, Is.EqualTo(expected.Username));
                Assert.That(result.ArticleId, Is.EqualTo(expected.ArticleId));
                Assert.That(result.Content, Is.EqualTo(expected.Content));
                Assert.That(result.CreatedAt, Is.EqualTo(expected.CreatedAt));
            });
        }


        [Test]
        public void ChangeContentComment_NotExistedCommentId_ReturnsException()
        {
            var commentId = Guid.NewGuid();
            var content = "content";

            async Task Check()
            {
                await _repository.ChangeContentComment(commentId, content, default);
            }

            Assert.CatchAsync(Check);
        }


        [Test]
        public void ChangeContentComment_EmptyCommentId_ReturnsException()
        {
            var commentId = Guid.Empty;
            var content = "content";

            async Task Check()
            {
                await _repository.ChangeContentComment(commentId, content, default);
            }

            Assert.CatchAsync(Check);
        }


        [Test]
        public void ChangeContentComment_EmptyContent_ReturnsException()
        {
            var commentId = Guid.NewGuid();
            var content = string.Empty;

            async Task Check()
            {
                await _repository.ChangeContentComment(commentId, content, default);
            }

            Assert.CatchAsync(Check);
        }


        [Test]
        public async Task GetUserIdByCommentId_ExistedId_ReturnsGuid()
        {
            var commentId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            Comment input = new()
            {
                Id = commentId,
                UserId = userId,
                ArticleId = Guid.NewGuid(),
                Content = "testcontent",
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(input);
            _context.SaveChanges();

            var result = await _repository.GetUsernameByCommentId(commentId, default);

            Assert.That(result, Is.EqualTo(userId));
        }


        [Test]
        public void GetUserIdByCommentId_NotExistedId_ReturnsException()
        {
            var commentId = Guid.NewGuid();

            async Task Check()
            {
                await _repository.GetUsernameByCommentId(commentId, default);
            }

            Assert.CatchAsync(Check);
        }
    }
}