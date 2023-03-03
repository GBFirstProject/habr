using AutoMapper;
using HabrParser.Database;
using HabrParser.Interfaces;
using HabrParser.Models.APIArticles;
using HabrParser.Models.APIAuth;
using HabrParser.Models.APIComments;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace HabrParser
{
    internal class Worker : BackgroundService
    {
        private string _mainTitle = string.Empty;
        private ArticleThreadLevelType _levelType;

        private readonly IArticlesRepository _articlesRepository;
        private readonly ICommentsRepository _commentsRepository;
        private readonly ICommentsCountRepository _countRepository;
        private readonly IMapper _mapper;

        private readonly UserManager<ApplicationUser> _userManager;

        public Worker(
            IArticlesRepository articlesRepository,
            ICommentsRepository commentsRepository,
            ICommentsCountRepository countRepository,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _articlesRepository = articlesRepository;
            _commentsRepository = commentsRepository;
            _countRepository = countRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Введите номер блока статей (* 1000) (50, 100, ..., 700), либо ID статьи, с которой нужно начать: ");
            var levelNumber = Console.ReadLine();
            _levelType = ArticleThreadLevel.ThreadLevel(levelNumber);
            Console.ResetColor();

            int endArticleId = 0;
            int lastIdAdded = 0;

            if (_levelType != ArticleThreadLevelType.None)
            {
                lastIdAdded = await _articlesRepository.LastArticleId(_levelType, stoppingToken);
                endArticleId = ArticleThreadLevel.IteratorLastNumber(_levelType);
            }
            else
            {
                lastIdAdded = Convert.ToInt32(levelNumber);
                endArticleId = ArticleThreadLevel.IteratorLastNumber(_levelType);
            }

            _mainTitle = $"Парсинг статей с {lastIdAdded} по {endArticleId} - {_levelType}";
            Console.Title = _mainTitle;

            if (lastIdAdded < 0) return;

            for (int i = lastIdAdded; i < endArticleId; i++)
            {
                await GetArticleAndSaveToDb(i, stoppingToken);
            }
        }

        private async Task GetArticleAndSaveToDb(int articleId, CancellationToken cancellationToken)
        {
            var web = new HtmlWeb();
            var url = $"https://habr.com/ru/post/{articleId}/";
            HtmlDocument doc;
            try
            {
                var article_repo = await _articlesRepository.ArticleAlreadyExists(articleId, cancellationToken);
                if (article_repo != null)
                {
                    if (!await _commentsRepository.CommentsAlreadyExists(article_repo.Id, cancellationToken))
                    {
                        await GetCommentsAndSaveToDb(article_repo, cancellationToken);
                    }
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"ст. № {articleId} уже присутствует в БД");
                    Console.ResetColor();
                    return;
                }

                doc = web.Load(url);
                foreach (var script in doc.DocumentNode.Descendants("script").ToArray())
                {
                    string s = script.InnerText;

                    try
                    {
                        if (s.Length > 26)
                        {
                            if (s.Substring(0, 25) == "window.__INITIAL_STATE__=")
                            {
                                var jsonStr = s.Substring(25, s.Length - 147);
                                string strToReplace = $"\"articlesList\":{{\"{articleId}\"";
                                jsonStr = jsonStr.Replace(strToReplace, "\"articlesList\":{\"article\"");
                                HabrParser.Models.Root data;
                                data = JsonConvert.DeserializeObject<HabrParser.Models.Root>(jsonStr)!;
                                Console.ResetColor();

                                if (data.articlesList.articlesList.article != null)
                                {
                                    Console.Title = _mainTitle + $" - ст. № {articleId} - {data.articlesList.articlesList.article.titleHtml}";
                                    Console.WriteLine($"ст. № {articleId} - {data.articlesList.articlesList.article.titleHtml}");
                                    Article article = _mapper.Map<Models.Article, Article>(data.articlesList.articlesList.article);

                                    var guid = await GetUserAndSaveToDb(article.Author, cancellationToken);
                                    article.Author.Id = guid;
                                    
                                    var result = await _articlesRepository.CreateHabrArticle(article, _levelType, cancellationToken);
                                    await GetCommentsAndSaveToDb(result, cancellationToken);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task GetCommentsAndSaveToDb(Article article, CancellationToken cancellationToken)
        {
            try
            {
                var web = new HtmlWeb();
                var url = $"https://habr.com/ru/post/{article.hubrId}/comments";
                HtmlDocument doc = web.Load(url);

                var comments = doc.DocumentNode.Descendants(20).FirstOrDefault(n => n.HasClass("tm-comments__tree"));
                if (comments == null)
                {
                    return;
                }
                foreach (var comment in comments.ChildNodes)
                {
                    await ParseComment(comment, Guid.Empty, article.Id, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task ParseComment(HtmlNode comment, Guid replyId, Guid articleId, CancellationToken cancellationToken)
        {
            try
            {
                var temp1 = comment.Element("article").Element("div").Elements("div");
                var temp2 = temp1.First().Element("header")?.FirstChild.FirstChild.Element("span");
                var nickname = temp2?.FirstChild.InnerText.TrimStart().TrimEnd();
                var date = temp2?.LastChild.InnerText.TrimStart().TrimEnd().Replace(" в ", " ")!;

                if (!DateTime.TryParse(date, out var c) && date != null)
                {
                    date = date.Split('\n')[0];
                }

                var content = temp1.First().Element("div").InnerHtml;
                var rating = temp1.Last().FirstChild.FirstChild?.FirstChild.InnerText.Split(':').Last().TrimStart().TrimEnd();

                int likes = 0;
                if (!string.IsNullOrEmpty(rating))
                {
                    string[] parts = rating.Split(" и ");
                    if (parts.Length > 0)
                    {
                        string likesString = parts[0].Remove(0, 1);
                        int parsedLikes;
                        if (int.TryParse(likesString, out parsedLikes))
                        {
                            likes = parsedLikes;
                        }
                    }
                }

                int dislikes = 0;
                if (!string.IsNullOrEmpty(rating))
                {
                    string[] parts = rating.Split(" и ");
                    if (parts.Length > 1)
                    {
                        string dislikesString = parts[1].Remove(0, 1);
                        int parsedDislikes;
                        if (int.TryParse(dislikesString, out parsedDislikes))
                        {
                            dislikes = parsedDislikes;
                        }
                    }
                }

                Author author = new()
                {
                    NickName = string.IsNullOrEmpty(nickname) ? "UNKNOWN" : nickname,
                };

                var guid = await ParseUser(author.NickName, cancellationToken);
                author.Id = guid;
                var author_result = await _articlesRepository.CreateAuthor(author, cancellationToken);

                Comment entry = new()
                {
                    ArticleId = articleId,
                    Username = author_result.NickName,
                    Content = content,
                    CreatedAt = string.IsNullOrEmpty(date) ? new DateTime(2000, 1, 1) : DateTime.Parse(date),
                    ReplyTo = replyId
                };
                for (int i = 0; i < likes; i++)
                {
                    entry.Likes.Add(Guid.NewGuid().ToString());
                }
                for (int i = 0; i < dislikes; i++)
                {
                    entry.Dislikes.Add(Guid.NewGuid().ToString());
                }

                entry.CreatedAt = DateTime.Parse(date);
                var result = await _commentsRepository.CreateComment(entry, cancellationToken);
                await _countRepository.IncreaseCount(articleId, cancellationToken);

                var replyComments = comment.Element("div");
                if (replyComments == null)
                {
                    return;
                }
                foreach (var replyComment in replyComments.ChildNodes)
                {
                    await ParseComment(replyComment, entry.Id, articleId, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task<Guid> GetUserAndSaveToDb(Author author, CancellationToken cancellationToken)
        {
            return await CreateUser(author.NickName, author.FirstName, author.LastName, cancellationToken);
        }

        private async Task<Guid> ParseUser(string username, CancellationToken cancellationToken)
        {
            var web = new HtmlWeb();
            var url = $"https://habr.com/ru/users/{username}";
            HtmlDocument doc = web.Load(url);

            var data = doc.DocumentNode.Descendants(20).FirstOrDefault(n => n.HasClass("tm-user-card__name"));

            string firstName = string.Empty, lastName = string.Empty;

            if (data != null)
            {
                var splitted = data.InnerText.Split(' ');
                if (splitted.Length < 2)
                {
                    firstName = splitted[0];
                }
                else
                {
                    firstName = splitted[0];
                    lastName = splitted[1];
                }
            }

            return await CreateUser(username, firstName, lastName, cancellationToken);
        }

        private async Task<Guid> CreateUser(string username, string firstname, string lastname, CancellationToken cancellationToken)
        {
            try
            {
                var email = username.ToLower() + "@gmail.com";

                var entry = await _userManager.FindByNameAsync(email);
                if (entry != null)
                {
                    return Guid.Parse(entry.Id);
                }

                var user = new ApplicationUser()
                {
                    UserName = email,
                    FirstName = firstname,
                    LastName = lastname,
                    EmailConfirmed = true,
                    PhoneNumber = "88005553535",
                    Email = email
                };

                var result_create = await _userManager.CreateAsync(user, "P@ssw0rd");
                var result_role = await _userManager.AddToRoleAsync(user, Config.User);

                return Guid.Parse(user.Id);
            }
            catch
            {
                return Guid.Empty;
            }
        }
    }
}
