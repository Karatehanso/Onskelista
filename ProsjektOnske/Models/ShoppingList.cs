using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace olist.Models
{
    public class ShoppingList
    {
        
        [ForeignKey("ApplicationUser")]
        
        public int Id { get; set; }
        
        public int OwnerId { get; set; }
        
        public ApplicationUser Owner { get; set; }
        
        public List<Product> Products { get; set; }
        
       // public virtual ApplicationUser User { get; set; }
    }
}