using AutoMapper;
using ProductsApi.DTOs;
using ProductsApi.Models;

namespace ProductsApi.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductResponseDto>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductPatchDto, Product>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
