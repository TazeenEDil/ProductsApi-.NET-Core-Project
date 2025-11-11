
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.Models;

namespace ProductsApi.Controllers
{
    [Route("api/[controller]")] //Endpoint to access controller
    [ApiController]
    public class CrudController : ControllerBase
    {
        public List<Product> products = new List<Product>() {
            new Product {Id=1, Name="Lipstick", Description="Red luscious lipstick", Price= 800},
            new Product {Id=2, Name="Blush", Description="Pink cherry blush on for cheeks", Price= 1200},
            new Product {Id=3, Name="Liner", Description="Black wing liner", Price= 700},

        };

        //Get products
        [HttpGet]
        public ActionResult<List<Product>> GetProducts() // <List<Product>> to return products + HTTP status codes
        {
            return products;
        }

        // Get products by id
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound();  // 404 if product not found

            return product;         // 200 OK + product data
        }

        // Post products
        [HttpPost]
        public ActionResult<Product> PostProduct(Product product)
        {
            product.Id = products.Max(p => p.Id) + 1; // Product id is assigned after the previous max value of id
            products.Add(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product); // CreatedAction(actionName, routeValues,value)
        }

        // Put products
        [HttpPut("{id}")]
        public IActionResult PutProduct(int id, Product updatedProduct)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;

            return NoContent();
        }

        //Patch products

        [HttpPatch("{id}")]
        public IActionResult PatchProduct(int id, [FromBody] Dictionary<string, object> updates)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();

            foreach (var key in updates.Keys)
            {
                switch (key.ToLower())
                {
                    case "name":
                        product.Name = updates[key].ToString();
                        break;
                    case "description":
                        product.Description = updates[key].ToString();
                        break;
                    case "price":
                        product.Price = Convert.ToDouble(updates[key]);
                        break;
                }
            }

            return Ok(product);

        }


        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();

            products.Remove(product);
            return NoContent();
        }
    }

    }
