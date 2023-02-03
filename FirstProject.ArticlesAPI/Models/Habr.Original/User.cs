﻿using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.ArticlesAPI.Models.Habr.Original
{
    [Table("Users")]
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}