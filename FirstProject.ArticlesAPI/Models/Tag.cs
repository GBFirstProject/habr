﻿using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.ArticlesAPI.Models
{
    [Table("Tags")]
    public class Tag : BaseModel<int>
    {
        public string TagName { get; set; } = string.Empty;
        public List<Article> Articles { get; set; } = new List<Article>();
    }
}
