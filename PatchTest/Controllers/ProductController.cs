using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using PatchTest.Model;

namespace PatchTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {
        // some pseudo Mock Data
        private static List<Product> Products =new List<Product>()
        {
            new Product()
            {
                Id = Guid.NewGuid(),
                AvailableInStock = 10,
                Description = "Super Product 1",
                Name = "Blume",
                IsHighlighted = false
            },
            new Product()
            {
                Id = Guid.NewGuid(),
                AvailableInStock = 0,
                Description = "Super Product 1",
                Name = "Blume 2",
                IsHighlighted = true
            },
            new Product()
            {
                Id = Guid.NewGuid(),
                AvailableInStock = 5,
                Description = "Super Product 1",
                Name = "Blume 3",
                IsHighlighted = false
            }
        };

        [HttpGet]
        public List<Product> Get()
        {
            return Products;
        }

        [HttpGet("{id}")]
        public Product Get(Guid id)
        {
            return Products.FirstOrDefault(q => q.Id == id);
        }

        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            var existing = Products.FirstOrDefault(q => q.Id == id);
            if (existing == null) return false;
            Products.Remove(existing);
            return true;
        }

        [HttpPut()]
        public IActionResult Put([FromBody] Product product)
        {
            // replace existing product
            if(Delete(product.Id)){
                Products.Add(product);
            }
            return new ObjectResult(product);
        }

        [HttpPost()]
        public IActionResult Post([FromBody] Product product)
        {
            // set the new id
            product.Id = Guid.NewGuid();
            Products.Add(product);
            return new ObjectResult(product);
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(Guid id, [FromBody] JsonPatchDocument<Product> patchDoc)
        {

            if (patchDoc != null)
            {
                // load product
                var prod = Products.FirstOrDefault(q => q.Id == id);
                if (prod == null)
                    throw new Exception("Product not found");

                patchDoc.ApplyTo(prod, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return new ObjectResult(prod);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
