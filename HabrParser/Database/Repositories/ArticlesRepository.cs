using HabrParser.Models.APIArticles;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace HabrParser.Database.Repositories
{
    public class ArticlesRepository : IArticlesRepository
    {
        public readonly ArticlesDBContext _dbContext;
        private ArticleThreadLevelType _levelType = ArticleThreadLevelType.None;
        public ArticlesRepository(ArticlesDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Article> CreateHabrArticle(Article article, ArticleThreadLevelType threadLevel = ArticleThreadLevelType.None, CancellationToken cancellationToken = default)
        {
            if (article == null) return null!;

            var entry = await ArticleAlreadyExists(article.hubrId, cancellationToken);
            if (entry != null) return entry;

            if (article.Author != null)
            {
                Author authorForTable = AddAuthor(article.Author);
                if (authorForTable.Contacts != null)
                {
                    for (int i = 0; i < authorForTable.Contacts.Count; i++)
                    {
                        Contact contact = AddContact(authorForTable.Contacts[i]);
                        authorForTable.Contacts[i] = contact;
                    }
                }
                article.Author = authorForTable;
            }
            else
            {
                article.Author = AddAuthor(new Author { NickName = "UNKNOWN" });
            }
            
            for (int i = 0; i < article.Tags.Count; i++)
            {
                Tag tag = AddTag(article.Tags[i]);
                article.Tags[i] = tag;
            }
            for (int i = 0; i < article.Hubs.Count; i++)
            {
                Hub hub = AddHub(article.Hubs[i]);
                article.Hubs[i] = hub;
            }

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
        private Tag AddTag(Tag tag)
        {
            Tag? existTag = _dbContext.Tags.FirstOrDefault(t => t.TagName == tag.TagName);
            if (existTag == null)
            {

                _dbContext.Tags.Add(tag);
                return tag;
            }
            else
            {
                return existTag;
            }
        }
        private Hub AddHub(Hub hub)
        {
            Hub? existHub = _dbContext.Hubs.FirstOrDefault(h => h.Title == hub.Title);
            if (existHub == null)
            {
                _dbContext.Hubs.Add(hub);
                return hub;
            }
            else
            {
                return existHub;
            }
        }
        private Contact AddContact(Contact contact)
        {
            Contact? existContact = _dbContext.Contacts.FirstOrDefault(c => c.Equals(contact));
            if (existContact == null)
            {
                _dbContext.Contacts.Add(contact);
                return contact;
            }
            else
            {
                return existContact;
            }
        }

        private void UdpateLastId(int id)
        {            
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

        public async Task<Author> CreateAuthor(Author author, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _dbContext.Authors.FirstOrDefaultAsync(s => s.NickName == author.NickName, cancellationToken);
                if (result != null)
                {
                    return result;
                }
                await _dbContext.AddAsync(author, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return author;
            }
            catch
            {
                throw;
            }
        }
    }
}
