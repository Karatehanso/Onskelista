using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using olist.Data;
using olist.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

// Note on action names: The actions in an API controller can actually be called whatever you want.
// It's the attributes that dictate what HTTP verb(s) they handle.

namespace olist.Controllers
{
    // Route overrides the URL of the controller
    [Route("/api/Pendos")]
    // ApiController enables automatic model validation
    [ApiController]
    public class NotifikasjonAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        // Standard DB context dependency injection
        public NotifikasjonAPIController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Handles GET (read) requests to /api/Participants.
        [HttpGet]
        public IActionResult GetAll()
        {

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var pendingFriend = _db.PendingFriends.Where(e => e.pendingToId == userId).ToList();
            
            
            
            // Participants are returned with a 200 OK status code          
            return Ok(pendingFriend);
        }

        // Handles GET (read) requests to /api/Participants/<id>, e.g. /api/Participants/1
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            // Search for the product using the given id
            var pendingFriend = _db.PendingFriends.Find(id);
            
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Return 404 Not Found if the item isn't in the database
            if (pendingFriend == null)
                return NotFound();

            if(pendingFriend.pendingFromId == userId)
                return Ok(pendingFriend);
            // Return the item with 200 OK if it is
            
            return Ok(pendingFriend);
           
        }
        
        
        [HttpPost("{id}")]
        public IActionResult Post(int id)
        {

            var user = _db.PendingFriends.Find(id);

            var user2 = _db.ApplicationUsers.Find(user.pendingFromId);
            
            if (user2 == null)
                return NoContent();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           
            var myself = _db.ApplicationUsers.Find(userId);
           
            var relationship = new FriendRequest(myself, user2, user2.FirstName, user2.LastName, user2.BirthDate, user2.ProfilePicture);
            var relationship2 = new FriendRequest(user2, myself, myself.FirstName, myself.LastName, myself.BirthDate, myself.ProfilePicture);

            //Legger tl de nye vennskapene til databasen
            _db.FriendRequests.Add(relationship);
            _db.FriendRequests.Add(relationship2);
            _db.SaveChanges();
            
            //Fjern venneforespørselet fra databasen
            _db.Remove(user);
            _db.SaveChanges();
            
            // Return 201 Created with the location of the new item
            return Ok(relationship);
        
        }

        // Handles DELETE requests to /api/Participants/<id>, e.g. /api/Participants/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Look for the item we are deleting
            var pendingFriend = _db.PendingFriends.Find(id);

            // Return 404 Not Found if the item isn't in the database
            if (pendingFriend == null)
                return NotFound();

            // Product exists, so let's remove it
            
            _db.Remove(pendingFriend);
            _db.SaveChanges();
            
            // Return the deleted item with 200 OK
            return Ok(pendingFriend);
        }
    }
}