﻿using HabrParser.Models.APIArticles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabrParser.Database
{
    public interface IParserRepository : IDisposable
    {
        void CreateHabrArticle(Article article);
    }
}
