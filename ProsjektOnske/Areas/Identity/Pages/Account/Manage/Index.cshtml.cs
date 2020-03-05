using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using olist.Data;
using olist.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace olist.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db; // New
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IHostingEnvironment _appEnvironment;

        public IndexModel(
            ApplicationDbContext db, // New
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            IHostingEnvironment appEnvironment)
        {
            _db = db; // New
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _appEnvironment = appEnvironment;
        }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            
            [Required]
            public string Nickname { get; set; }
            
            
            [Required]
            public DateTime BirthDate { get; set; }
            
            
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
            
                LastName = user.LastName,
                FirstName = user.FirstName,
                BirthDate = user.BirthDate,
                Nickname = user.Nickname,
                Email = email,
                PhoneNumber = phoneNumber
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }
            
                
            var friendo = _db.FriendRequests.Where(e => e.friendTo == user).ToList();


            user.UserName = Input.Email;

            user.Email = Input.Email;
            user.Nickname = Input.Nickname;
            user.BirthDate = Input.BirthDate;
            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            
            user.PhoneNumber = Input.PhoneNumber;
            
            foreach (var d in friendo)
            {
                d.FirstName = Input.FirstName;
                d.LastName = Input.LastName;
                d.Bursdag = Input.BirthDate;
            }
            
            _db.SaveChanges();

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }

        
        public async Task<IActionResult> OnPostUploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                StatusMessage = "File not selected";
                return RedirectToPage();
            }

            else
            {
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
                
                var v = User.FindFirstValue(ClaimTypes.NameIdentifier);


                if (user.ProfilePicture != "/images/default-profile.png")
                {
                    System.IO.File.Delete(pathRoot + user.ProfilePicture);
                    
                    
                }

                user.ProfilePicture = pathToImage;
                
                var friendo = _db.FriendRequests.Where(e => e.friendTo == user).ToList();

                foreach (var d in friendo)
                {
                    d.ProfilePicture = pathToImage;
                }
                
                _db.SaveChanges();

                // Output 
                //ViewData["FilePath"] =pathToImage;
                StatusMessage = "Your profile picture has been updated";
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostDeleteImage()
        {
            var user = await _userManager.GetUserAsync(User);
            
            if (user.ProfilePicture != "/images/default-profile.png")
            {                
                var pathRoot = _appEnvironment.WebRootPath;
                System.IO.File.Delete(pathRoot + user.ProfilePicture);
                
                user.ProfilePicture = "/images/default-profile.png";
                _db.SaveChanges();
                
                StatusMessage = "Your profile picture has been deleted";
            }
            else
            {
                StatusMessage = "You have no profile picture to delete";
            }

            return RedirectToPage();
        }
    }
}
