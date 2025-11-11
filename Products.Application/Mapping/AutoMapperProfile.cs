using AutoMapper;
using Products.Application.DTOs.Product;
using Products.Domain.Entities;

namespace Products.Application.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Entity → DTO
            CreateMap<Product, ProductResponseDto>();
            CreateMap<Product, ProductPatchDto>(); 

            // DTO → Entity
            CreateMap<ProductCreateDto, Product>();

            // PATCH DTO mapping (Ignore nulls)
            CreateMap<ProductPatchDto, Product>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
