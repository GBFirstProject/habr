using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace FirstProject.AuthAPI.Articles.Data
{
    public class Author
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int hubrId { get; set; }
        [Required]
        public string NickName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; } = string.Empty;
        public float Rating { get; set; } = 0;        
        public string? Logo { get; set; }
        public string? Title { get; set; }
        public string? Link { get; set; }
        public List<Article> Articles { get; set; }
    }
}
