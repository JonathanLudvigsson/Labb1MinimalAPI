using Book_MinimalAPI.Models.DTOs;

namespace Book_Web.Services
{
    public class BookService : BaseService, IBookService
    {
        private readonly IHttpClientFactory _clientFactory;

        public BookService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> CreateBook<T>(BookDTO bookDTO)
        {
            return await this.SendAsync<T>(new Models.APIRequest()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = bookDTO,
                Url = StaticDetails.BookApiBase + "/api/book/"
            });
        }

        public async Task<T> DeleteBook<T>(int id)
        {
            return await this.SendAsync<T>(new Models.APIRequest()
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = StaticDetails.BookApiBase + "/api/book/" + id,
                AccessToken = ""
            });
        }

        public async Task<T> GetAllBooks<T>()
        {
            return await this.SendAsync<T>(new Models.APIRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.BookApiBase + "/api/book/",
                AccessToken = ""
            });
        }

        public async Task<T> GetBook<T>(int id)
        {
            return await this.SendAsync<T>(new Models.APIRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.BookApiBase + "/api/book/" + id,
                AccessToken = ""
            });
        }

        public async Task<T> GetBooksFromAuthor<T>(string authorName)
        {
            return await this.SendAsync<T>(new Models.APIRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.BookApiBase + "/api/book/" + authorName,
                AccessToken = ""
            });
        }

        public async Task<T> UpdateBook<T>(BookDTO bookDTO)
        {
            return await this.SendAsync<T>(new Models.APIRequest()
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = bookDTO,
                Url = StaticDetails.BookApiBase + "/api/book/" + bookDTO.Id,
                AccessToken = ""
            });
        }
    }
}
