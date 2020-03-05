using System.Net.Mime;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using olist.Models;

namespace olist.Controllers
{
    public class EmailController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        
        public EmailController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        
        public IActionResult Index()
        {

            
            return View();
        }

        public async Task<IActionResult> SendMail(string msg)
        {

            var user = await _userManager.GetUserAsync(User);
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("onskelista.xzy@gmail.com"));
            message.To.Add(new MailboxAddress("onskelista.xzy@gmail.com"));
            message.Subject = "Message from " + user.FirstName + " " + user.LastName;
            
            message.Body = new TextPart("html")
            {
                Text = "Message: " + msg
            } ;
            
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587);
                client.Authenticate("onskelista.xzy@gmail.com", "prosjektonske");
                client.Send(message);
                client.Disconnect(false);
            }

            return View("Index");
        }
        
    }
}