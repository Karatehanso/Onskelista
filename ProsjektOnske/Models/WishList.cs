using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace olist.Models
{
    public class WishList
    {
        public int Id { get; set; }
        
        public ApplicationUser Owner { get; set; }
        public string OwnerId { get; set; }
        
        public string Title { get; set; }
        public DateTime Deadline { get; set; }
        public string Category { get; set; }
        public string Privacy { get; set; }
        
        public List<Product> Products { get; set; }
        
    }
}