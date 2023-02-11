// See https://aka.ms/new-console-template for more information
using AutoMapper;
using HabrParser;
using HabrParser.Database;
using HabrParser.Models.APIArticles;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Xml;

class Program
{
    static IMapper mapper;
    static IHost host;
    static ArticleThreadLevelType levelType;
    static async Task Main(string[] args)
    {
        try
        {
            host = Host.CreateDefaultBuilder(args)
                        .ConfigureServices(services =>
                        {
                            services.AddDbContext<ArticlesDBContext>(options =>
                            {
                                options.UseSqlServer(@"Initial Catalog=ArticlesDbNew; Data Source=localhost,1433;TrustServerCertificate=True;User ID=sa;Password=123456");
                            });
                            services.AddTransient<IParserRepository, ParserRepository>();
                            //services.AddScoped<IParserRepository, ParserRepository>();
                        }).Build();

            
            /*using (var context = new ArticlesDBContext(@"Initial Catalog=ArticlesDb; Data Source=localhost,1433;TrustServerCertificate=True;User ID=sa;Password=123456"))
            {
                var lastParsedIdItem = context.ParserResult.FirstOrDefault();
                if (lastParsedIdItem != null)
                    lastIdAdded = lastParsedIdItem.LastArtcleId;
                else
                    lastIdAdded = 1;
            }*/
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Введите номер блока статей, который собираемся парсить |1 = 1; (номер * 1000 = номер первой статьи)|: ");
            var levelNumber = Console.ReadLine();
            levelType = ArticleThreadLevel.ThreadLevel(levelNumber);
            Console.ResetColor();

            int endArticleId = ArticleThreadLevel.IteratorLastNumber(levelType);
            int lastIdAdded = host.Services.GetRequiredService<IParserRepository>().LastArticleId(levelType);
            Console.Title = $"Парсинг статей с {lastIdAdded} по {endArticleId} - {levelType.ToString()}";

            if (lastIdAdded < 0) return;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<HabrParser.Models.Article, HabrParser.Models.ArticleOnly.ParsedArticle>();
                cfg.CreateMap<HabrParser.Models.Author, HabrParser.Models.ArticleOnly.Author>();
                cfg.CreateMap<HabrParser.Models.Flow, HabrParser.Models.ArticleOnly.Flow>();
                cfg.CreateMap<HabrParser.Models.Hub, HabrParser.Models.ArticleOnly.Hub>();
                cfg.CreateMap<HabrParser.Models.LeadData, HabrParser.Models.ArticleOnly.LeadData>();
                cfg.CreateMap<HabrParser.Models.Tag, HabrParser.Models.ArticleOnly.Tag>();

                cfg.CreateMap<string, Guid>().ConvertUsing(s => LocalConverters.Str2Guid(s));

                cfg.CreateMap<HabrParser.Models.Article, Article>()
                    .ForMember(m => m.hubrId,
                    opt => opt.MapFrom(a => a.id))
                    .ForMember(m => m.Title,
                    opt => opt.MapFrom(a => a.titleHtml))
                    .ForMember(m => m.Id,
                    opt => opt.Ignore())
                    .ForMember(m => m.Language,
                    opt => opt.MapFrom(a => LocalConverters.StrToLanguage(a.lang)));
                cfg.CreateMap<HabrParser.Models.Contact, Contact>()
                    .ForMember(x => x.Id, opt => opt.Ignore());
                cfg.CreateMap<HabrParser.Models.Author, Author>()
                    .ForMember(a => a.NickName, opt => opt.MapFrom(a => a.alias))
                    .ForMember(a => a.FirstName, opt => opt.MapFrom(a => LocalConverters.FirstNameFromStr(a.fullname)))
                    .ForMember(a => a.LastName, opt => opt.MapFrom(a => LocalConverters.LastNameFromStr(a.fullname)))
                    .ForMember(a => a.hubrId, opt => opt.MapFrom(a => a.id))
                    .ForMember(m => m.Id, opt => opt.Ignore())
                    .ForMember(m => m.AvatarUrl, opt => opt.MapFrom(a => a.avatarUrl.Replace("//", "http://")));
                cfg.CreateMap<HabrParser.Models.Hub, Hub>()
                    .ForMember(a => a.hubrId,
                    opt => opt.MapFrom(a => a.id))
                    .ForMember(m => m.Id,
                    opt => opt.Ignore());
                cfg.CreateMap<HabrParser.Models.LeadData, LeadData>()
                    .ForMember(m => m.Id,
                    opt => opt.Ignore());
                cfg.CreateMap<HabrParser.Models.Metadata, Metadata>()
                    .ForMember(m => m.Id,
                    opt => opt.Ignore());
                cfg.CreateMap<HabrParser.Models.Tag, Tag>()
                    .ForMember(t => t.TagName,
                    opt => opt.MapFrom(t => t.titleHtml))
                    .ForMember(m => m.Id,
                    opt => opt.Ignore());
                cfg.CreateMap<HabrParser.Models.Statistics, Statistics>()
                    .ForMember(x => x.Id, opt => opt.Ignore());
            });
            // Настройка AutoMapper
            mapper = new Mapper(config);

            for (int i = lastIdAdded; i < endArticleId; i++)
            //for (int i = 75651; i < 715400; i++)
            {
                //DBContext не может работать многопоточно
                //что, кстати, создаёт дополнительную проблему
                //при реализации ArticlesAPI
                //var task1 = GetArticleAndSaveToDbAsync(i);
                /*var task2 = GetArticleAndSaveToDbAsync(i + 1);
                var task3 = GetArticleAndSaveToDbAsync(i + 2);
                var task4 = GetArticleAndSaveToDbAsync(i + 3);
                var task5 = GetArticleAndSaveToDbAsync(i + 4);
                var task6 = GetArticleAndSaveToDbAsync(i + 5);
                var task7 = GetArticleAndSaveToDbAsync(i + 6);
                var task8 = GetArticleAndSaveToDbAsync(i + 7);*/
                //await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7, task8);
                //await Task.WhenAny(task1);
                GetArticleAndSaveToDb(i);
            }
        }
        catch (Exception e)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(e.Message.PadRight(60));
            Console.ResetColor();
        }
    }

    public static async Task GetArticleAndSaveToDbAsync(int articleId)
    {
        await Task.Run(() => GetArticleAndSaveToDb(articleId));
    }

    public static void GetArticleAndSaveToDb(int articleId)
    {
        var web = new HtmlWeb();
        var url = $"https://habr.com/ru/post/{articleId}/";
        HtmlDocument doc;
        try
        {
            if(host.Services.GetRequiredService<IParserRepository>().ArticleAlreadyExists(articleId))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"ст. № {articleId} уже присутствует в БД");
                Console.ResetColor();
                return;
            }
            doc = web.Load(url);        
            /*using (var fs = new FileStream("d:\\fileMainPrj.html", FileMode.Create))
            {
                doc.Save(fs);
            }*/
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
                            data = JsonConvert.DeserializeObject<HabrParser.Models.Root>(jsonStr);
                            Console.ResetColor();

                            if (data.articlesList.articlesList.article != null)
                            {
                                Console.WriteLine($"ст. № {articleId} - {data.articlesList.articlesList.article.titleHtml}");
                                //ParsedArticle article = mapper.Map<Article, ParsedArticle>(data.articlesList.articlesList.article);
                                Article article = mapper.Map<HabrParser.Models.Article,
                                    Article>(data.articlesList.articlesList.article);
                                host.Services.GetRequiredService<IParserRepository>().CreateHabrArticle(article, levelType);
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


