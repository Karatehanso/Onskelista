using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using olist.Data;
using olist.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Bcpg;

// Note on action names: The actions in an API controller can actually be called whatever you want.
// It's the attributes that dictate what HTTP verb(s) they handle.

namespace olist.Controllers
{
    // Route overrides the URL of the controller
    [Route("/api/controller")]
    // ApiController enables automatic model validation
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        
        // Standard DB context dependency injection
        public FriendController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Handles GET (read) requests to /api/Participants.
        [HttpGet]
        public IActionResult GetAll()
        {

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var friendRequest = _db.FriendRequests.Where(e => e.friendFromId == userId ).ToList();

            var g = _db.ApplicationUsers.ToList();
 
            foreach (var d in friendRequest)
            {
               g = _db.ApplicationUsers.Where(e => e.Id == d.friendToId).ToList();

            }
            //
            
            var userpictu = _db.ApplicationUsers.Find(userId);
                
            //
            
            var usero = _db.ApplicationUsers.Find(userId);

            var userpic = usero.ProfilePicture;
            
            var x  = new 
            {
                
               friendRequest,
               g
            };
            
            // Participants are returned with a 200 OK status code          
            return Ok(x);
         
        }

        // Handles GET (read) requests to /api/Participants/<id>, e.g. /api/Participants/1
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            // Search for the product using the given id
            var friendRequest = _db.FriendRequests.Find(id);
            
            var useru = _db.ApplicationUsers.Find(id);

            var userpic = useru.ProfilePicture;
            
          //  var picture = _db.ApplicationUsers.Find(id);
            
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var x  = new 
            {
                userpic,

                friendRequest 

            };
            
            // Return 404 Not Found if the item isn't in the database
            if (friendRequest == null)
                return NotFound();

            if(friendRequest.friendFromId == userId)
                return Ok(x);
            // Return the item with 200 OK if it is
            
            return Ok(x);
            
        }

        // Handles POST (create) requests to /api/Participants/<id>, e.g. /api/Participants/1
        [HttpPost]
        public IActionResult Post(FriendRequest friendRequest)
        {
            // Return bad request if id is set. It should not be set on new participants.
            if (friendRequest.Id != 0)
                return BadRequest();

            // Add the new product
            _db.Add(friendRequest);
            _db.SaveChanges();

            // Return 201 Created with the location of the new item
            return CreatedAtAction(nameof(Get), new { id = friendRequest.Id }, friendRequest);
        }

        // Handles DELETE requests to /api/Participants/<id>, e.g. /api/Participants/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Finner begge venneforhold (Når et venneforhold skal avsluttes må to vennskap i databasen fjernes)
            var friendRequest = _db.FriendRequests.Find(id);
            
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var g = friendRequest.friendFromId;
            var h = friendRequest.friendToId;

            var friendRequest2 = _db.FriendRequests.Where(e => e.friendFromId == h && e.friendToId == userId).ToList();
            //var friendRequest2 = _db.FriendRequests.Find(id + 1);

            foreach (var d in friendRequest2)
            {
                _db.Remove(d);
                
            }
            _db.SaveChanges();
            // Return 404 Not Found if the item isn't in the database
            if (friendRequest == null)
                return NotFound();

            // Fjerner venneforhold til valgt venn, og vennens venneforhold til bruker
            
            _db.Remove(friendRequest);
    
            _db.SaveChanges();
            
            // Return the deleted item with 200 OK
            return Ok(friendRequest);
        }
    }
}
