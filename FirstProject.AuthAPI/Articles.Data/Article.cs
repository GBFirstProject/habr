using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.AuthAPI.Articles.Data
{
    public class Article
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int hubrId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; }
        [Required]
        public string TextHtml { get; set; } = string.Empty;
        [Required]
        public DateTimeOffset? TimePublished { get; set; } = DateTime.UtcNow;
        public bool CommentsEnabled { get; set; } = true;
        public Guid LeadDataId { get; set; }        
        public Guid MetaDataId { get; set; }        
        public Guid AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public Author Author { get; set; } = new Author();
        public Guid StatisticsId { get; set; }        
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<Hub> Hubs { get; set; } = new List<Hub>();
        public bool IsPublished { get; set; } = false;
        /// <summary>
        /// последняя версия - в базе храним только ник автора, а самого автора в другом сервисе
        /// </summary>
        public string AuthorNickName { get; set; }
    }
}
