using HabrParser.Models.APIArticles;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace HabrParser.Database.Repositories
{
    public class ParserRepository : IParserRepository
    {
        public readonly ArticlesDBContext _dbContext;
        private ArticleThreadLevelType _levelType = ArticleThreadLevelType.None;
        public ParserRepository(ArticlesDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Article> CreateHabrArticle(Article article, ArticleThreadLevelType threadLevel = ArticleThreadLevelType.None, CancellationToken cancellationToken = default)
        {
            if (article == null) return null!;

            var entry = await ArticleAlreadyExists(article.hubrId, cancellationToken);
            if (entry != null) return entry;

            article.Author ??= AddAuthor(new Author { NickName = "UNKNOWN" });

            await _dbContext.Articles.AddAsync(article, cancellationToken);
            UdpateLastId(article.hubrId);
            _dbContext.ChangeTracker.DetectChanges();
            await _dbContext.SaveChangesAsync(cancellationToken);

            return article;
        }

        public async Task<Article> ArticleAlreadyExists(int hubrId, CancellationToken cancellationToken)
        {
            try
            {
                var article = await _dbContext.Articles.FirstOrDefaultAsync(article => article.hubrId == hubrId, default);
                if (article == null) return null!;
                else return article;
            }
            catch
            {
                return null!;
            }
        }

        public Task<int> LastArticleId(ArticleThreadLevelType levelType, CancellationToken cancellationToken)
        {
            int lastId = 0;
            try
            {
                using (var fs = new FileStream($"{AppContext.BaseDirectory}\\{levelType.ToString()}.parser", FileMode.Open))
                {
                    byte[] buffer = new byte[255];
                    fs.Read(buffer, 0, buffer.Length);
                    string str = Encoding.Default.GetString(buffer);
                    lastId = int.Parse(str);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                switch (levelType)
                {
                    case ArticleThreadLevelType.Level1:
                        { lastId = 1; break; }
                    case ArticleThreadLevelType.Level50:
                        { lastId = 50000; break; }
                    case ArticleThreadLevelType.Level100:
                        { lastId = 100000; break; }
                    case ArticleThreadLevelType.Level150:
                        { lastId = 150000; break; }
                    case ArticleThreadLevelType.Level200:
                        { lastId = 200000; break; }
                    case ArticleThreadLevelType.Level250:
                        { lastId = 250000; break; }
                    case ArticleThreadLevelType.Level300:
                        { lastId = 300000; break; }
                    case ArticleThreadLevelType.Level350:
                        { lastId = 350000; break; }
                    case ArticleThreadLevelType.Level400:
                        { lastId = 400000; break; }
                    case ArticleThreadLevelType.Level450:
                        { lastId = 450000; break; }
                    case ArticleThreadLevelType.Level500:
                        { lastId = 500000; break; }
                    case ArticleThreadLevelType.Level550:
                        { lastId = 550000; break; }
                    case ArticleThreadLevelType.Level600:
                        { lastId = 600000; break; }
                    case ArticleThreadLevelType.Level650:
                        { lastId = 650000; break; }
                    case ArticleThreadLevelType.Level700:
                        { lastId = 700000; break; }
                    default:
                        { lastId = 1; break; }
                }
                return Task.FromResult(lastId);
            }

            /*ParserResult lastArcticleId = GetParsedResultMatchToLevelType(_levelType);
            if (lastArcticleId == null) return ArticleThreadLevel.IteratorFirstNumber(levelType);
            
            switch (levelType)
            {
                case ArticleThreadLevelType.Level1:
                    { lastId = lastArcticleId.LastArticleId1; if (lastId < 1 || lastId > 50000) lastId = 1; break; }
                case ArticleThreadLevelType.Level50:
                    { lastId = lastArcticleId.LastArticleId50; if (lastId < 50000 || lastId > 100000) lastId = 50000; break; }
                case ArticleThreadLevelType.Level100:
                    { lastId = lastArcticleId.LastArticleId100; if (lastId < 100000 || lastId > 150000) lastId = 100000; break; }
                case ArticleThreadLevelType.Level150:
                    { lastId = lastArcticleId.LastArticleId150; if (lastId < 150000 || lastId > 200000) lastId = 150000; break; }
                case ArticleThreadLevelType.Level200:
                    { lastId = lastArcticleId.LastArticleId200; if (lastId < 200000 || lastId > 250000) lastId = 200000; break; }
                case ArticleThreadLevelType.Level250:
                    { lastId = lastArcticleId.LastArticleId250; if (lastId < 250000 || lastId > 300000) lastId = 250000; break; }
                case ArticleThreadLevelType.Level300:
                    { lastId = lastArcticleId.LastArticleId300; if (lastId < 300000 || lastId > 350000) lastId = 300000; break; }
                case ArticleThreadLevelType.Level350:
                    { lastId = lastArcticleId.LastArticleId350; if (lastId < 350000 || lastId > 400000) lastId = 350000; break; }
                case ArticleThreadLevelType.Level400:
                    { lastId = lastArcticleId.LastArticleId400; if (lastId < 400000 || lastId > 450000) lastId = 400000; break; }
                case ArticleThreadLevelType.Level450:
                    { lastId = lastArcticleId.LastArticleId450; if (lastId < 450000 || lastId > 500000) lastId = 450000; break; }
                case ArticleThreadLevelType.Level500:
                    { lastId = lastArcticleId.LastArticleId500; if (lastId < 500000 || lastId > 550000) lastId = 500000; break; }
                case ArticleThreadLevelType.Level550:
                    { lastId = lastArcticleId.LastArticleId550; if (lastId < 550000 || lastId > 600000) lastId = 550000; break; }
                case ArticleThreadLevelType.Level600:
                    { lastId = lastArcticleId.LastArticleId600; if (lastId < 600000 || lastId > 650000) lastId = 600000; break; }
                case ArticleThreadLevelType.Level650:
                    { lastId = lastArcticleId.LastArticleId650; if (lastId < 650000 || lastId > 700000) lastId = 650000; break; }
                case ArticleThreadLevelType.Level700:
                    { lastId = lastArcticleId.LastArticleId700; if (lastId < 700000 || lastId > 715000) lastId = 700000; break; }
                default:
                    { lastId = lastArcticleId.LastArticleId; if (lastId < 1) lastId = 1; break; }
            }*/
            return Task.FromResult(lastId);
        }

        public void Dispose()
        {
            Dispose();
        }

        private Author AddAuthor(Author author)
        {
            if (author == null) return null!;
            string existAlias = author.NickName;
            Author existAuthor = _dbContext.Authors.FirstOrDefault(a => a.NickName == existAlias);
            if (existAuthor == null)
            {      
                return author;
            }
            else
            {
                return existAuthor;
            }
        }

        private void UdpateLastId(int id)
        {
            /*ParserResult lastArcticleId = GetParsedResultMatchToLevelType(_levelType);            
            ParserResult res;
            if (lastArcticleId != null)
            {
                res = lastArcticleId;
            }
            else
            {
                res = new ParserResult();
            }
            switch (_levelType)
            {
                case ArticleThreadLevelType.None:
                    res.LastArticleId = id;
                    break;
                case ArticleThreadLevelType.Level1:
                    res.LastArticleId1 = id;
                    break;
                case ArticleThreadLevelType.Level50:
                    res.LastArticleId50 = id;
                    break;
                case ArticleThreadLevelType.Level100:
                    res.LastArticleId100 = id;
                    break;
                case ArticleThreadLevelType.Level150:
                    res.LastArticleId150 = id;
                    break;
                case ArticleThreadLevelType.Level200:
                    res.LastArticleId200 = id;
                    break;
                case ArticleThreadLevelType.Level250:
                    res.LastArticleId250 = id;
                    break;
                case ArticleThreadLevelType.Level300:
                    res.LastArticleId300 = id;
                    break;
                case ArticleThreadLevelType.Level350:
                    res.LastArticleId350 = id;
                    break;
                case ArticleThreadLevelType.Level400:
                    res.LastArticleId400 = id;
                    break;
                case ArticleThreadLevelType.Level450:
                    res.LastArticleId450 = id;
                    break;
                case ArticleThreadLevelType.Level500:
                    res.LastArticleId500 = id;
                    break;
                case ArticleThreadLevelType.Level550:
                    res.LastArticleId550 = id;
                    break;
                case ArticleThreadLevelType.Level600:
                    res.LastArticleId600 = id;
                    break;
                case ArticleThreadLevelType.Level650:
                    res.LastArticleId650 = id;
                    break;
                case ArticleThreadLevelType.Level700:
                    res.LastArticleId700 = id;
                    break;
                default:
                    res.LastArticleId = id;
                    break;
            }
            if(lastArcticleId == null)
            {
                //res.ParserResultId = 1;
                _dbContext.ParserResult.Add(res);
            }
            else
            {
                //res.ParserResultId = lastArcticleId.ParserResultId;
                _dbContext.ParserResult.Update(res);
            }*/
            //_dbContext.ParserResult.Add(res);
            //_dbContext.SaveChangesAsync().Wait();
            try
            {
                using (var fs = new FileStream($"{AppContext.BaseDirectory}\\{_levelType.ToString()}.parser", FileMode.Create))
                {
                    byte[] buffer = Encoding.Default.GetBytes(id.ToString());
                    fs.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }
        }

        private ParserResult GetParsedResultMatchToLevelType(ArticleThreadLevelType levelType)
        {
            ParserResult lastArcticleId;
            switch (_levelType)
            {
                case ArticleThreadLevelType.None:
                    lastArcticleId = _dbContext.ParserResult.FirstOrDefault(la => la.LastArticleId != 0);
                    break;
                case ArticleThreadLevelType.Level1:
                    lastArcticleId = _dbContext.ParserResult.FirstOrDefault(la => la.LastArticleId1 != 0);
                    break;
                case ArticleThreadLevelType.Level50:
                    lastArcticleId = _dbContext.ParserResult.FirstOrDefault(la => la.LastArticleId50 != 0);
                    break;
                case ArticleThreadLevelType.Level100:
                    lastArcticleId = _dbContext.ParserResult.FirstOrDefault(la => la.LastArticleId100 != 0);
                    break;
                case ArticleThreadLevelType.Level150:
                    lastArcticleId = _dbContext.ParserResult.FirstOrDefault(la => la.LastArticleId150 != 0);
                    break;
                case ArticleThreadLevelType.Level200:
                    lastArcticleId = _dbContext.ParserResult.FirstOrDefault(la => la.LastArticleId200 != 0);
                    break;
                case ArticleThreadLevelType.Level250:
                    lastArcticleId = _dbContext.ParserResult.FirstOrDefault(la => la.LastArticleId250 != 0);
                    break;
                case ArticleThreadLevelType.Level300:
                    lastArcticleId = _dbContext.ParserResult.FirstOrDefault(la => la.LastArticleId300 != 0);
                    break;
                case ArticleThreadLevelType.Level350:
                    lastArcticleId = _dbContext.ParserResult.FirstOrDefault(la => la.LastArticleId350 != 0);
                    break;
                case ArticleThreadLevelType.Level400:
                    lastArcticleId = _dbContext.ParserResult.FirstOrDefault(la => la.LastArticleId400 != 0);
                    break;
                case ArticleThreadLevelType.Level450:
                    lastArcticleId = _dbContext.ParserResult.FirstOrDefault(la => la.LastArticleId450 != 0);
                    break;
                case ArticleThreadLevelType.Level500:
                    lastArcticleId = _dbContext.ParserResult.FirstOrDefault(la => la.LastArticleId500 != 0);
                    break;
                case ArticleThreadLevelType.Level550:
                    lastArcticleId = _dbContext.ParserResult.FirstOrDefault(la => la.LastArticleId550 != 0);
                    break;
                case ArticleThreadLevelType.Level600:
                    lastArcticleId = _dbContext.ParserResult.FirstOrDefault(la => la.LastArticleId600 != 0);
                    break;
                case ArticleThreadLevelType.Level650:
                    lastArcticleId = _dbContext.ParserResult.FirstOrDefault(la => la.LastArticleId650 != 0);
                    break;
                case ArticleThreadLevelType.Level700:
                    lastArcticleId = _dbContext.ParserResult.FirstOrDefault(la => la.LastArticleId700 != 0);
                    break;
                default:
                    lastArcticleId = _dbContext.ParserResult.FirstOrDefault(la => la.LastArticleId != 0);
                    break;
            }
            return lastArcticleId;
        }
    }
}
