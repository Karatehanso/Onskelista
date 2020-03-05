using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using olist.Data;
using olist.Models;

namespace olist.Controllers

{
    [ApiController]
    [Route("api/Shopping")]
    public class ShoppingListApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ShoppingListApiController(ApplicationDbContext db)
        {
            _db = db;
        }



        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_db.ShoppingLists.ToList());
        }




        [HttpGet("{owner}")]
        public IActionResult Get(string ApplicationUser)
        {
            //search for products for given wishlist
            //  var wishList = _db.Products.Where(e => e.WishListId == WishListId).ToList();
            var product = _db.Products.Find(ApplicationUser);
            if (product == null)
                return NotFound();
            //check if prod exists, return 404 if not
            // return 202 with prod
            return Ok(_db.Products.ToList());
        }


        /*  [HttpGet("{Owner}")]
          public IActionResult Get (string ApplicationUser)
          {
              //search for products for given wishlist
              var ShoppingList = _db.Products.Where(e => e.ApplicationUser == Owner).ToList();
  
              //check if prod exists, return 404 if not
              // return 202 with prod
              return Ok(ShoppingList);
          
      }*/
    }
}