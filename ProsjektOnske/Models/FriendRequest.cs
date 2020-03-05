using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace olist.Models
{
    public class FriendRequest
    {
        
        public FriendRequest() {}

        public FriendRequest(ApplicationUser friendfrom, ApplicationUser friendto, string fornavn, string etternavn, DateTime bursdag, string picture)
        {
            friendFrom = friendfrom;
            friendTo = friendto;
            
            FirstName = fornavn;
            
            LastName = etternavn;

            Bursdag = bursdag;

            ProfilePicture = picture;
            

        }
      
        public int Id { get; set; }

        //public int OwnerId { get; set; }
        
        
        public string friendFromId { get; set; }
        public virtual ApplicationUser friendFrom { get; set; }
        
        public string friendToId { get; set; }
        public virtual ApplicationUser friendTo { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string ProfilePicture { get; set; }
        
        
        [DataType(DataType.Date)]
        [DisplayFormat( DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Bursdag { get; set; }
        
        
        
    }
}


