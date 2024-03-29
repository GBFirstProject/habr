﻿using HabrParser.Models.APIArticles;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabrParser.Models.APIArticles
{
    [Table("LeadData")]
    public class LeadData : BaseModel<Guid>
    {
        public Guid ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public Article Article { get; set; }
        public string TextHtml { get; set; } = string.Empty;        
        public string? ImageUrl { get; set; } = string.Empty;
        public string? Image { get; set; } = string.Empty;
    }
}
