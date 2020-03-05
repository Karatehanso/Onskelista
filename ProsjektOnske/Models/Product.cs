
namespace olist.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public decimal Price { get; set; }
        public string Notes { get; set; }
        public string ProdImage { get; set; }
        public string Url { get; set; }
        
        public int WishListId { get; set; }
        public WishList WishList { get; set; }
        
        // if taken is true, the product should be in someones shoppinglist
        public bool Taken { get; set; }
        
       // public int ShoppingListId { get; set; }
        
       // public ShoppingList ShoppingList { get; set; }

        public ShoppingList ShoppingList { get; set; }
        
        //if bought is true, the product is bought by another user
        public bool Bought { get; set; }
    }
}