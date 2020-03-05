using System;
using System.ComponentModel.DataAnnotations;

namespace olist.Models
{
    public class PendingFriend
    {
        
        public PendingFriend() {}
        
        public PendingFriend(ApplicationUser pendingfrom, ApplicationUser pendingto,String firstname,String lastName, DateTime bursdag)
        {
            pendingFrom = pendingfrom;
            pendingTo = pendingto;
            
            FirstName = firstname;
            
            LastName = lastName;

            Bursdag = bursdag;
            
        }
        
        public int Id { get; set; }
        
        public string pendingFromId { get; set; }
        
        public virtual ApplicationUser pendingFrom { get; set; }
        
        public string pendingToId { get; set; }
        public virtual ApplicationUser pendingTo { get; set; }
        
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Bursdag { get; set; }
    }
}