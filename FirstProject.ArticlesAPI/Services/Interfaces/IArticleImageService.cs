using FirstProject.ArticlesAPI.Models;

namespace FirstProject.ArticlesAPI.Services.Interfaces
{
    public interface IArticleImageService
    {
        public byte[] GenerateImage(Article articleData);
        public byte[] GetImageBytes(Article articleData);
    }
}
