using AutoMapper;
using BookCatalogManagementSystem.Models.DTOs;
using BookCatalogManagementSystem.Models.Entities;
using BookCatalogManagementSystem.Repository.HelpModel;

namespace BookCatalogManagementSystemAPI.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BookDto, Book>().ReverseMap();
        CreateMap<PageListDto<BookDto>, PageList<Book>>().ReverseMap();
    }
}