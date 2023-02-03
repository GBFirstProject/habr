// See https://aka.ms/new-console-template for more information
using AutoMapper;
using HabrParser.Database;
using HabrParser.Models;
using HabrParser.Models.ArticleOnly;
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
                        options.UseSqlServer(@"Initial Catalog=HabrParserDB; Data Source=localhost,1433;TrustServerCertificate=True;User ID=HabrUser;Password=123456");
                    });
                    services.AddTransient<IParserRepository, ParserRepository>();
                }).Build();

    int lastIdAdded;
    using (var context = new ArticlesDBContext(@"Initial Catalog=HabrParserDB; Data Source=localhost,1433;TrustServerCertificate=True;User ID=HabrUser;Password=123456"))
    {
        var lastParsedIdItem = context.ParserResult.FirstOrDefault();
        if(lastParsedIdItem != null)
            lastIdAdded = lastParsedIdItem.LastArtclelId;
        else
            lastIdAdded = 1;
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

        foreach (var script in doc.DocumentNode.Descendants("script").ToArray())
        {            
            string s = script.InnerText;

            try
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Article, ParsedArticle>();
                    cfg.CreateMap<HabrParser.Models.Author, HabrParser.Models.ArticleOnly.Author>();
                    cfg.CreateMap<HabrParser.Models.Flow, HabrParser.Models.ArticleOnly.Flow>();
                    cfg.CreateMap<HabrParser.Models.Hub, HabrParser.Models.ArticleOnly.Hub>();
                    cfg.CreateMap<HabrParser.Models.LeadData, HabrParser.Models.ArticleOnly.LeadData>();
                    cfg.CreateMap<HabrParser.Models.Tag, HabrParser.Models.ArticleOnly.Tag>();
                });
                // Настройка AutoMapper
                var mapper = new Mapper(config);
                if (s.Length > 26)
                {
                    if (s.Substring(0, 25) == "window.__INITIAL_STATE__=")
                    {
                        var jsonStr = s.Substring(25, s.Length - 147);
                        string strToReplace = $"\"articlesList\":{{\"{i}\"";
                        jsonStr = jsonStr.Replace(strToReplace, "\"articlesList\":{\"article\"");
                        Root data;
                        data = JsonConvert.DeserializeObject<Root>(jsonStr);
                        Console.ResetColor();

                        if (data.articlesList.articlesList.article != null)
                        {
                            Console.WriteLine($"ст. № {i} - {data.articlesList.articlesList.article.titleHtml}");
                            ParsedArticle article = mapper.Map<Article, ParsedArticle>(data.articlesList.articlesList.article);
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

