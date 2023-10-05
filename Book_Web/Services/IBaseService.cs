using Book_Web.Models;

namespace Book_Web.Services
{
    public interface IBaseService : IDisposable
    {
        APIResponseDTO responseModel { get; set; }
        Task<T> SendAsync<T>(APIRequest apiRequest);
    }
}
