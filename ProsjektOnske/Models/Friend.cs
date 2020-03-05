using System.Collections.Generic;
using Microsoft.IdentityModel.Xml;

namespace olist.Models
{
    
    
    public class Friend
    {
        
        public int Id { get; set; }
        
        public string Navn { get; set; }
        
        public List<FriendRequest> Friends { get; set; }
        
    }
}