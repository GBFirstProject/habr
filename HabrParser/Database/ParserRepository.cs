using HabrParser.Models.APIArticles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public void CreateHabrArticle(Article article)
        {
            if (article == null) return;
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
            _dbContext.Leads.Add(article.LeadData);           
            for(int i = 0; i < article.Tags.Count; i++)
            {
                Tag tag = AddTag(article.Tags[i]);
                article.Tags[i] = tag;
            }
            for (int i = 0; i < article.Hubs.Count; i++)
            {
                Hub hub = AddHub(article.Hubs[i]);
                article.Hubs[i] = hub;
            }
            _dbContext.Articles.Add(article);        
            UdpateLastId(article.hubrId);
            _dbContext.ChangeTracker.DetectChanges();
            _dbContext.SaveChangesAsync();
        }

        public Author AddAuthor(Author author)
        {
            if(author == null) return null;            
            string existAlias = author.NickName;            
            Author existAuthor = _dbContext.Authors.FirstOrDefault(a => a.NickName == existAlias);
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
        public Tag AddTag(Tag tag)
        {
            Tag? existTag = _dbContext.Tags.FirstOrDefault(t => t.TagName == tag.TagName);
            if(existTag == null)
            {
                
                _dbContext.Tags.Add(tag);
                return tag;
            }
            else
            {
                return existTag;
            }
        }
        public Hub AddHub(Hub hub)
        {
            Hub? existHub = _dbContext.Hubs.FirstOrDefault(h => h.Title == hub.Title);
            if(existHub == null)
            {
                _dbContext.Hubs.Add(hub);
                return hub;
            }
            else
            {
                return existHub;
            }
        }
        public Contact AddContact(Contact contact)
        {
            Contact? existContact = _dbContext.Contacts.FirstOrDefault(c => c.Equals(contact));
            if(existContact == null)
            {
                _dbContext.Contacts.Add(contact);
                return contact;
            }
            else
            { 
                return existContact; 
            }
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
