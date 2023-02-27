using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace FirstProject.AuthAPI.Articles.Data
{    
    public class Hub 
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int hubrId { get; set; }
        public string Alias { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<Article> Articles { get; set; } = new List<Article>();

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(Hub)) return false;
            var objToCompare = (Hub)obj;
            return Alias.Equals(objToCompare.Alias) &&
                  Type.Equals(objToCompare.Type) &&
                  Title.Equals(objToCompare.Title) &&
                  hubrId.Equals(objToCompare.hubrId);
        }
        public override string ToString()
        {
            return Title;
        }
    }
}
