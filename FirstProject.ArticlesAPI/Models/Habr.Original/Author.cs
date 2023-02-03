using System.ComponentModel.DataAnnotations.Schema;

namespace FirstProject.ArticlesAPI.Models.Habr.Original
{
    [Table("Authors")]
    public class Author
    {
        public int Id { get; set; }
        public string? Alias { get; set; }
        public string? Fullname { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Speciality { get; set; }
        public int? Rating { get; set; }
        //public object? RelatedData { get; set; }
        public string? RelatedData { get; set; }
        //поля, доступные на хабре, но, вероятно, не нужные в нашем проекте
        //public List<Contact> contacts { get; set; }
        //public List<AuthorContact> authorContacts { get; set; }
        //public ScoreStats scoreStats { get; set; }
        //public PaymentDetails paymentDetails { get; set; }
        public string? Logo { get; set; }
        public string? Title { get; set; }
        public string? Link { get; set; }
    }
}
