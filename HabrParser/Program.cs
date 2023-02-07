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

try
{
    var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services => {
                    services.AddDbContext<ArticlesDBContext>(options =>
                    {
                        options.UseSqlServer(@"Initial Catalog=ArticlesDb; Data Source=localhost,1433;TrustServerCertificate=True;User ID=sa;Password=123456");
                    });
                    services.AddTransient<IParserRepository, ParserRepository>();
                }).Build();

    int lastIdAdded;
    using (var context = new ArticlesDBContext(@"Initial Catalog=ArticlesDb; Data Source=localhost,1433;TrustServerCertificate=True;User ID=sa;Password=123456"))
    {
        var lastParsedIdItem = context.ParserResult.FirstOrDefault();
        if(lastParsedIdItem != null)
            lastIdAdded = lastParsedIdItem.LastArtclelId;
        else
            lastIdAdded = 1;

        
        /*context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Articles ON");
        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.ArticleTag ON");
        context.SaveChanges();*/
    }    
    if (lastIdAdded < 0) return;
    for (int i = lastIdAdded; i < 600000; i++)
    {

        var url = $"https://habr.com/ru/post/{i}/";

        var web = new HtmlWeb();
        var doc = web.Load(url);
        /*using (var fs = new FileStream("d:\\fileMainPrj.html", FileMode.Create))
        {
            doc.Save(fs);
        }*/

        
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
                opt => opt.Ignore());
            cfg.CreateMap<HabrParser.Models.Contact, Contact>()
                .ForMember(x => x.Id, opt => opt.Ignore());
            cfg.CreateMap<HabrParser.Models.Author, Author>()
                .ForMember(a => a.NickName,
                opt => opt.MapFrom(a => a.alias))
                .ForMember(a => a.FirstName,
                opt => opt.MapFrom(a => LocalConverters.FirstNameFromStr(a.fullname)))
                .ForMember(a => a.LastName,
                opt => opt.MapFrom(a => LocalConverters.LastNameFromStr(a.fullname)))
                .ForMember(a => a.hubrId,
                opt => opt.MapFrom(a => a.id))
                .ForMember(m => m.Id,
                opt => opt.Ignore());
            cfg.CreateMap<HabrParser.Models.Hub, Hub>()
                .ForMember(a => a.hubrId,
                opt => opt.MapFrom(a => a.id))
                .ForMember(m => m.Id,
                opt => opt.Ignore());
            cfg.CreateMap<HabrParser.Models.LeadData, LeadData>()
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
        var mapper = new Mapper(config);

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
                        string strToReplace = $"\"articlesList\":{{\"{i}\"";
                        jsonStr = jsonStr.Replace(strToReplace, "\"articlesList\":{\"article\"");
                        HabrParser.Models.Root data;
                        data = JsonConvert.DeserializeObject<HabrParser.Models.Root>(jsonStr);
                        Console.ResetColor();

                        if (data.articlesList.articlesList.article != null)
                        {
                            Console.WriteLine($"ст. № {i} - {data.articlesList.articlesList.article.titleHtml}");
                            //ParsedArticle article = mapper.Map<Article, ParsedArticle>(data.articlesList.articlesList.article);
                            Article article = mapper.Map<HabrParser.Models.Article,
                                Article>(data.articlesList.articlesList.article);
                            host.Services.GetRequiredService<IParserRepository>().CreateHabrArticle(article);
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
}

catch (Exception e)
{
    Console.BackgroundColor = ConsoleColor.DarkRed;
    Console.WriteLine(e.Message.PadRight(60));
    Console.ResetColor();    
}

