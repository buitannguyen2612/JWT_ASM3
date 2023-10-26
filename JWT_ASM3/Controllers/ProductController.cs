using JWT_ASM3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace JWT_ASM3.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly JWTContext _context;

        public ProductController(JWTContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProduct(bool? inStock, int? skip, int? take)
        {
            var products = this._context.Products.AsQueryable();
            if (inStock != null)
            {
                products = this._context.Products.Where(i => i.AvailableQuantity > 0);
            }
            if (skip != null)
            {
                products = products.Skip((int)skip);
            }
            if (take != null)
            {
                products = products.Take((int)take);
            }
            return await products.ToListAsync();

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await this._context.Products.FindAsync(id);
            if(product == null)
            {
                return NotFound();
            }
            return product;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if(id  != product.ProductId)
            {
                return NotFound();
            }
            this._context.Entry(product).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            this._context.Products.Add(product);
            await this._context.SaveChangesAsync();
            return CreatedAtAction("GetProduct", new { id = product.ProductId },
           product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await this._context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            this._context.Products.Remove(product);
            await this._context.SaveChangesAsync();
            return NoContent();
        }
        private bool ProductExists(int id)
        {
            return this._context.Products.Any(e => e.ProductId == id);
        }
 

    }
}
