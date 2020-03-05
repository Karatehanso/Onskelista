using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using olist.Data;

namespace olist.Controllers
{
    public class NotifikasjonController : Controller
    {
        
        private readonly ApplicationDbContext _db;
        
        public NotifikasjonController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(string id)
        {

            /*
            var currentuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            //var exist = _db.PendingFriends.Find(currentuser);

            if (_db.PendingFriends.Any(o => o.pendingToId == currentuser))
            {
                
                
                return View();
              
            }
            */
            
            return View();
                
        }
    }
}