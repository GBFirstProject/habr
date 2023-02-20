using AutoMapper;
using HabrParser.Database;
using HabrParser.Database.Repositories;
using HabrParser.Models.APIArticles;
using HtmlAgilityPack;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace HabrParser
{
    internal class Worker : BackgroundService
    {
        private string _mainTitle = string.Empty;
        private ArticleThreadLevelType _levelType;

        private readonly IParserRepository _parserRepository;
        private readonly IMapper _mapper;

        public Worker(IParserRepository parserRepository, IMapper mapper)
        {
            _parserRepository = parserRepository;
            _mapper = mapper;
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
                lastIdAdded = await _parserRepository.LastArticleId(_levelType, stoppingToken);
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
                var article_repo = await _parserRepository.ArticleAlreadyExists(articleId, cancellationToken);
                if (article_repo != null)
                {
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
                                    await _parserRepository.CreateHabrArticle(article, _levelType, cancellationToken);
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
    }
}
