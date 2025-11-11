using AutoMapper;
using Products.Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Products.Application.DTOs.Product;
using Products.Application.Interfaces.Persistence;
using Products.Application.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Products.Application.Services
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
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product == null ? null : _mapper.Map<ProductResponseDto>(product);
        }

        public async Task<ProductResponseDto> CreateProductAsync(ProductCreateDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            var created = await _productRepository.AddAsync(product);
            return _mapper.Map<ProductResponseDto>(created);
        }

        public async Task<bool> UpdateProductAsync(int id, ProductCreateDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return false;

            _mapper.Map(dto, product);
            return await _productRepository.UpdateAsync(product);
        }

        public async Task<bool> PatchProductAsync(int id, JsonPatchDocument<ProductPatchDto> patchDoc)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return false;

            var patchDto = _mapper.Map<ProductPatchDto>(product);
            patchDoc.ApplyTo(patchDto);
            _mapper.Map(patchDto, product);

            return await _productRepository.UpdateAsync(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _productRepository.DeleteAsync(id);
        }
    }
}
