using HabrParser.Models.ArticleOnly;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabrParser.Database
{
    public class ParserRepository : IParserRepository
    {
        public readonly ArticlesDBContext _dbContext;
        public ParserRepository(ArticlesDBContext dbContext)
        { 
            _dbContext = dbContext;
        }

        public void CreateHabrArticle(ParsedArticle article)
        {
            if (article == null) return;
            Author authorForTable = AddAuthor(article.Author);
            article.Author = authorForTable;
            _dbContext.Leads.Add(article.LeadData);           
            foreach(var tag in article.Tags)
            {
                AddTag(tag);
            }
            foreach(var hub in article.Hubs)
            {
                AddHub(hub);
            }
            _dbContext.Articles.Add(article);        
            UdpateLastId(article.Id);
            _dbContext.SaveChangesAsync();
        }

        public Author AddAuthor(Author author)
        {
            if(author == null) return null;            
            string existAlias = author.Alias;            
            Author existAuthor = _dbContext.Authors.FirstOrDefault(a => a.Alias == existAlias);
            if (existAuthor == null) 
            {
                _dbContext.Authors.Add(author);
                //_dbContext.SaveChanges();        
                return author;
            }
            else
            {
                return existAuthor;
            }            
        }
        public void AddTag(Tag tag)
        {
            Tag? existTag = _dbContext.Tags.FirstOrDefault(t => t.TitleHtml == tag.TitleHtml);
            if(existTag == null)
            {
                
                _dbContext.Tags.Add(tag);
                //_dbContext.SaveChanges();
                
            }
            /*else
            {
                if (existTag.TitleHtml != tag.TitleHtml)
                {
                    _dbContext.Tags.Add(tag);
                    //_dbContext.SaveChanges();
                }
            }*/
        }
        public void AddHub(Hub hub)
        {
            Hub? existHub = _dbContext.Hubs.FirstOrDefault(h => h.Title == hub.Title);
            if(existHub == null)
            {
                _dbContext.Hubs.Add(hub);
                //_dbContext.SaveChangesAsync();
            }
            /*else
            {
                if (existHub.Title != hub.Title)
                {
                    _dbContext.Hubs.Add(hub);
                    //_dbContext.SaveChangesAsync();
                }
            }*/
        }

        public void UdpateLastId(int id)
        {
            var lastArcticleId = _dbContext.ParserResult.FirstOrDefault();
            if(lastArcticleId == null)
            {
                _dbContext.ParserResult.Add(new ParserResult { LastArtclelId = id });
            }
            else
            {
                lastArcticleId.LastArtclelId = id;
                _dbContext.ParserResult.Update(lastArcticleId);
            }
            //_dbContext.SaveChangesAsync().Wait();
        }

        public void Dispose()
        {
            this.Dispose();
        }

        public int LastArticleId 
        { 
            get 
            {
                var lastArticleId = _dbContext.ParserResult.FirstOrDefault();
                return lastArticleId.LastArtclelId;
            } 
        }
    }
}
