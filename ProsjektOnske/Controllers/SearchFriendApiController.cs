using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using olist.Data;
using olist.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Serialization;

// Note on action names: The actions in an API controller can actually be called whatever you want.
// It's the attributes that dictate what HTTP verb(s) they handle.

namespace olist.Controllers
{
    // Route overrides the URL of the controller
    [Route("/api/search")]
    // ApiController enables automatic model validation
    [ApiController]
    public class SearchFriendApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        
        [TempData]
        public string StatusMessage { get; set; }

        // Standard DB context dependency injection
        public SearchFriendApiController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Handles GET (read) requests to /api/Participants.
        [HttpGet]
        public IActionResult Get(string searchBy, string search)
        {

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var søk = _db.ApplicationUsers.Where(x => x.FirstName.Contains(search) || x.LastName.Contains(search)).ToList();

            
            if (searchBy == "FirstName")
                {
                    return NotFound();
                }
                else
                {
                    return Ok(søk);
                }
        }
    
        
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Search for the product using the given id
            var søk = _db.ApplicationUsers.Find(id);
            
            //  var picture = _db.ApplicationUsers.Find(id);
            
            // Return 404 Not Found if the item isn't in the database
            if (søk == null)
                return NotFound();

           
            // Return the item with 200 OK if it is
            
            return Ok(søk);
        }
        

        // Handles POST (create) requests to /api/Participants/<id>, e.g. /api/Participants/1
        [HttpPost("{id}")]
        public IActionResult Post(string id)
        {

            var pendFrom = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var pendmyself = _db.ApplicationUsers.Find(pendFrom);

            var pendTo = _db.ApplicationUsers.Find(id);

            var pendos = new PendingFriend(pendmyself, pendTo, pendmyself.FirstName,pendmyself.LastName,  pendmyself.BirthDate);

            // var friend = new FriendRequest(pendmyself, pendTo, pendTo.FirstName,pendTo.LastName,pendTo.BirthDate,pendTo.ProfilePicture,pendTo.Id);

            var søk = _db.PendingFriends.Any(x => x.pendingFromId.Contains(pendFrom) && x.pendingToId.Contains(id));

            var venn = _db.FriendRequests.Any(x => x.friendFromId.Contains(pendFrom) && x.friendToId.Contains(id));

            //Sjekker om en venneforespørsel allerde er sendt, eller om de allerede er venner
            if (søk || venn)
            {
                StatusMessage = "Your profile has been updated";
                return Ok();
                
            }

        else if(pendmyself != pendTo)
            {
                _db.PendingFriends.Add(pendos);
                _db.SaveChanges();
                return Ok(pendos);
            }
            
            return Ok();
        }
        
    }
}
