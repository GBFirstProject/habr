using FirstProject.ArticlesAPI.Models.DTO;

namespace FirstProject.ArticlesAPI.Services.Interfaces
{
    public interface ITagService
    {
        public Task<IEnumerable<TagDto>> GetTopTagsForLastMonthAsync(int count, CancellationToken cancellation);
    }
}
