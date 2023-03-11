using HabrParser.Models.APIArticles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HabrParser.Models.APIArticles
{
    public class Metadata : BaseModel<Guid>
    {
        public Guid ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public Article Article { get; set; }
        public List<string>? StylesUrls { get; set; }
        public List<string>? ScriptUrls { get; set; }
        public string? ShareImageUrl { get; set; }
        public int? ShareImageWidth { get; set; }
        public int? ShareImageHeight { get; set; }
        public string? VKShareImageUrl { get; set; }
        public string? SchemaJsonLd { get; set; }
        public string? MetaDescription { get; set; }
        public string? MainImageUrl { get; set; }
        public bool Amp { get; set; }
        public List<string>? CustomTrackerLinks { get; set; }
    }
}
