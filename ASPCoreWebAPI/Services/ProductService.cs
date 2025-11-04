using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using ProductsApi.DTOs;
using ProductsApi.Models;
using ProductsApi.Repositories.Interfaces;
using ProductsApi.Services.Interfaces;

namespace ProductsApi.Services
{
   
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
            }
            catch { throw; }
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                return product == null ? null : _mapper.Map<ProductResponseDto>(product);
            }
            catch { throw; }
        }

        public async Task<ProductResponseDto> CreateProductAsync(ProductCreateDto dto)
        {
            try
            {
                var product = _mapper.Map<Product>(dto);
                var created = await _productRepository.AddAsync(product);
                return _mapper.Map<ProductResponseDto>(created);
            }
            catch { throw; }
        }

        public async Task<bool> UpdateProductAsync(int id, ProductCreateDto dto)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null) return false;

                _mapper.Map(dto, product); // maps updated fields
                return await _productRepository.UpdateAsync(product);
            }
            catch { throw; }
        }

        public async Task<bool> PatchProductAsync(int id, JsonPatchDocument<ProductPatchDto> patchDoc)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null) return false;

                // create patch DTO from entity
                var patchDto = _mapper.Map<ProductPatchDto>(product);

                // apply patch (errors handled in controller via ModelState)
                patchDoc.ApplyTo(patchDto);

                // map patched DTO back to entity
                _mapper.Map(patchDto, product);
                return await _productRepository.UpdateAsync(product);
            }
            catch { throw; }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try { return await _productRepository.DeleteAsync(id); }
            catch { throw; }
        }
    }
}
