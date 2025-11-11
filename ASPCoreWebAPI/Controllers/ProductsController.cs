using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.DTOs;
using ProductsApi.Services;

namespace ProductsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _service.GetAllProductsAsync();
            return Ok(products);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _service.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            var created = await _service.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = created.Id }, created);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            var success = await _service.UpdateProductAsync(id, product);

            if (!success)
                return NotFound();

            return NoContent();
        }

        // PATCH: api/products/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchProduct(int id, Dictionary<string, object> updates)
        {
            var success = await _service.PatchProductAsync(id, updates);

            if (!success)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _service.DeleteProductAsync(id);

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
