using FirstProject.ArticlesAPI.Models.DTO;

namespace FirstProject.ArticlesAPI.Services.Interfaces
{
    public interface IHubService
    {
        public Task<IEnumerable<HubDto>> GetTopHubsLastMonth(int count, CancellationToken cancellation);
    }
}
