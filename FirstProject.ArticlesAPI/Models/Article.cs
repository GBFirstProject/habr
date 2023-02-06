﻿using FirstProject.ArticlesAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.ArticlesAPI.Models
{
    public class Article : BaseModel<Guid>
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public ArticleLanguage Language { get; set; } = ArticleLanguage.Russian;
        [Required]
        public string TextHtml { get; set; } = string.Empty;
        [Required]
        public DateTimeOffset? TimePublished { get; set; } = DateTime.UtcNow;
        public bool CommentsEnabled { get; set; } = true;
        public string ImageLink { get; set; } = string.Empty;
        public int? LeadDataId { get; set; }
        [ForeignKey("LeadDataId")]
        public LeadData LeadData { get; set; } = new LeadData();
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public Author Author { get; set; } = new Author();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<Hub> Hubs { get; set; } = new List<Hub>();
        
    }    
}
