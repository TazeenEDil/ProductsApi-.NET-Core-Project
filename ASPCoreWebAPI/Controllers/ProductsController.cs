using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.DTOs;
using ProductsApi.Services;

namespace ProductsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _service;

        public ProductsController(ILogger<ProductsController> logger, IProductService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _service.GetAllProducts());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var product = await _service.GetProductById(id);
                if (product == null)
                    return NotFound("Product not found");

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving product");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            try
            {
                var created = await _service.CreateProduct(productDto);
                return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return BadRequest("Error adding product");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductDto productDto)
        {
            try
            {
                var result = await _service.UpdateProduct(id, productDto);
                if (!result)
                    return NotFound("Product not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteProduct(id);
                if (!result)
                    return NotFound("Product not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchProduct(int id, [FromBody] JsonPatchDocument<ProductPatchDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Invalid patch document.");

            try
            {
                // ✅ Check if product exists first
                var existingProduct = await _service.GetProductById(id);
                if (existingProduct == null)
                    return NotFound("Product not found.");

                // ✅ Convert existing product into Patch DTO
                var patchDto = new ProductPatchDto
                {
                    Name = existingProduct.Name,
                    Description = existingProduct.Description,
                    Price = existingProduct.Price
                };

                // ✅ Apply patch safely
                patchDoc.ApplyTo(patchDto, error =>
                {
                    ModelState.AddModelError(error.Operation.path, error.ErrorMessage);
                });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // ✅ Convert patched DTO back into normal update DTO
                var updateDto = new ProductDto
                {
                    Name = patchDto.Name,
                    Description = patchDto.Description,
                    Price = patchDto.Price ?? existingProduct.Price
                };

                var success = await _service.UpdateProduct(id, updateDto);

                if (!success)
                    return StatusCode(500, "Failed to apply patch update.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }




    }
}
