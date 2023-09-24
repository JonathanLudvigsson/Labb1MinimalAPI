using AutoMapper;
using Book_MinimalAPI.Models;
using Book_MinimalAPI.Models.DTOs;

namespace Book_MinimalAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Book, BookDTO>().ReverseMap();
        }
    }
}
