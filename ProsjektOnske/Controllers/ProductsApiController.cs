using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using olist.Data;
using olist.Models;

namespace olist.Controllers
{
    [ApiController]
    [Route("api/Products")]
    public class ProductsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProductsApiController(ApplicationDbContext db)
        {
            _db = db;
        }
     
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_db.Products.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int WishListId)
        {
            //search for products for given wishlist
            var wishList = _db.Products.Where(e => e.WishListId== WishListId).ToList();

            //check if prod exists, return 404 if not
            // return 202 with prod
            return Ok(wishList);
        }

        [HttpPost]
        public IActionResult Post(Product product)
        {
            if (product.Id != 0)
                return BadRequest();
         //   var List = _db.Products.Where(e => e.WishListId == product.WishListId).ToList();
         //  Product.WishListId = List.Id;
            _db.Add(product);
            _db.SaveChanges();
            return Ok(product);
        }
        
        
        [HttpPut("{id}")]
        public IActionResult Put(Product product)
        {
            if (!_db.Products.Any(p => p.Id == product.Id))
                return NotFound();

            _db.Update(product);
            _db.SaveChanges();
            return Ok(product);
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)

        {
            var product = _db.Products.Find(id);
            if (product == null)
                return NotFound();

            _db.Remove(product);
            _db.SaveChanges();
            //returns no content satus: 204
            return NoContent();
        }
        
        
    }
}