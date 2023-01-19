using FirstProject.Web.Models;
using FirstProject.Web.Models.Dto;

namespace FirstProject.Web.Services.Interfaces
{
    public interface IBaseService : IDisposable
    {
        ResponseDto responseDto { get; set; }
        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
