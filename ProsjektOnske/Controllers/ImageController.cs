using System;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using olist.Data;
using olist.Models;


namespace olist.Controllers
{
    public class ImageController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _appEnvironment;
        public ImageController( IHostingEnvironment appEnvironment, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _appEnvironment = appEnvironment;
            _db = db;
            _userManager = userManager;
        }
        
        
        [HttpGet]
        public IActionResult Upload_Image()
        {
            return View();
        }
 
        [HttpPost]
        public async Task<IActionResult> Upload_Image(IFormFile file)
        {
            if (file == null || file.Length == 0) return Content("file not selected");

            // get Path
            var extension = Path.GetExtension(file.FileName);
            var pathRoot = _appEnvironment.WebRootPath;
            var pathToImage = "/images/" + Guid.NewGuid() + extension;
            var completePath = pathRoot + pathToImage;

            // Copy File to Target
            using (var stream = new FileStream(completePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            
            // Copy Path to Database & delete old image
            var user = await _userManager.GetUserAsync(User);
            
            if (user.ProfilePicture != "/images/default-profile.png")
            {
                System.IO.File.Delete(pathRoot + user.ProfilePicture);
            }
            user.ProfilePicture = pathToImage;

            _db.SaveChanges();
            
            // Output 
            //ViewData["FilePath"] =pathToImage;
            return View();
        }
    }
}