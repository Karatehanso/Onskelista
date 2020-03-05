using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using olist.Data;
using olist.Models;

namespace olist.Controllers
{
    public class FriendshipController : Controller
    {
        
        
        private readonly ApplicationDbContext _db;
        
        public FriendshipController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SearchFriend()
        {
            return View();
        }
        
        
        [Authorize]
        public  IActionResult Add(string id)
        {
            
            //Sende venneforespørsel

            var pendFrom = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var pendmyself = _db.ApplicationUsers.Find(pendFrom);

            var pendTo = _db.ApplicationUsers.Find(id);
            
            
            
            var pendos = new PendingFriend(pendmyself, pendTo, pendmyself.Nickname, pendmyself.LastName, pendmyself.BirthDate);
            
            if(pendmyself == pendTo)
                return RedirectToAction(nameof(SearchFriend));
            
            if(pendmyself != pendTo)
            {
                _db.PendingFriends.Add(pendos);
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}


