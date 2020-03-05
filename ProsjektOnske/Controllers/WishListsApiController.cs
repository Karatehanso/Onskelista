using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using olist.Data;
using olist.Models;
using Microsoft.AspNetCore.Mvc;

// Note on action names: The actions in an API controller can actually be called whatever you want.
// It's the attributes that dictate what HTTP verb(s) they handle.

namespace olist.Controllers
{
    // Route overrides the URL of the controller
    [Route("/api/WishLists")]
    // ApiController enables automatic model validation
    [ApiController]
    public class WishListsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        // Standard DB context dependency injection
        public WishListsApiController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // Handles GET (read) requests to /api/WishLists.
        [HttpGet]
        public IActionResult GetAll()
        {
            // Participants are returned with a 200 OK status code
            return Ok(_db.WishLists.ToList());
        }

       // Handles GET (read) requests to /api/WishLists/<id>, e.g. /api/WishLists/1
        [HttpGet("get/{id}")]
        public IActionResult GetOne(int id)
        {
            // Search for the product using the given id
            var wishList = _db.WishLists.Find(id);

            // Return 404 Not Found if the item isn't in the database
            if (wishList == null)
                return NotFound();

            // Return the item with 200 OK if it is
            return Ok(wishList);
        }
        
        [HttpGet("{applicationUserId}")]
        public IActionResult Get(string applicationUserId)
        {
            //search for wish lists for given user
            var applicationUser = _db.WishLists.Where(e => e.OwnerId == applicationUserId).ToList();

            //check if prod exists, return 404 if not
            // return 202 with prod
            return Ok(applicationUser);
            
        
        }

        // Handles POST (create) requests to /api/WishLists/<id>, e.g. /api/WishLists/1
        [HttpPost]
        public async Task<IActionResult> Post(WishList wishList)
        {
            // Return bad request if id is set. It should not be set on new wish lists.
            if (wishList.Id != 0)
                return BadRequest();

            // Add the new product
            var owner = await _userManager.GetUserAsync(User);
            wishList.OwnerId = owner.Id;
            _db.Add(wishList);
            _db.SaveChanges();

            // Return 201 Created with the location of the new item
            return Created("/api/WishLists/", wishList);
            
        }

        // Handles PUT (update) requests to /api/WishLists/<id>, e.g. /api/WishLists/1
        [HttpPut("{id}")]
        public IActionResult Put(WishList wishList)
        {
            // Return 404 Not Found if the item isn't in the database
            if (!_db.WishLists.Any(p => p.Id == wishList.Id))
                return NotFound();

            // Product exists, so let's update it
            _db.Update(wishList);
            _db.SaveChanges();

            // Return the modified item with 200 OK
            return Ok(wishList);
        }

        // Handles DELETE requests to /api/WishLists/<id>, e.g. /api/WishLists/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Look for the item we are deleting
            var wishList = _db.WishLists.Find(id);

            // Return 404 Not Found if the item isn't in the database
            if (wishList == null)
                return NotFound();

            // Product exists, so let's remove it
            _db.Remove(wishList);
            _db.SaveChanges();

            // Return the deleted item with 200 OK
            return Ok(wishList);
        }
    }
}