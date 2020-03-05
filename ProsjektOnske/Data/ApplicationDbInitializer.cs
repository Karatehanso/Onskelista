using System;
using System.Collections.Generic;
using olist.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace olist.Data
{
    public static class ApplicationDbInitializer
    {
        public static void Initialize(ApplicationDbContext db, UserManager<ApplicationUser> um, bool development)
        {
            
            
            
            
            if (!development)
            {
                
                db.Database.Migrate();
                
                return;
            }
                       
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            
           
            //Lager brukere

            var user = new ApplicationUser { UserName = "user@uia.no", FirstName = "Truls Ove", LastName = "Hansen", Email = "user@uia.no", Nickname = "Truls Ove", BirthDate = DateTime.Today, ProfilePicture = "/images/default-profile.png"};
           var user2 = new ApplicationUser { UserName = "user2@uia.no", FirstName = "Ola", LastName = "Nordmann", Email = "user2@uia.no", Nickname = "Doffen", BirthDate = DateTime.Today, ProfilePicture = "/images/default-profile.png" };
           var user3 = new ApplicationUser { UserName = "user3@uia.no", FirstName = "Jens", LastName = "Nilsen", Email = "user3@uia.no", Nickname = "Nabon", BirthDate = new DateTime(2017, 1, 18), ProfilePicture = "/images/default-profile.png" };
           
           um.CreateAsync(user, "Password1.").Wait();
           um.CreateAsync(user2, "Password1.").Wait();
           um.CreateAsync(user3, "Password1.").Wait();


            //Virtuelle venner
           // var venn = new Friend {Navn = "Tor"};
            //var venn2 = new Friend {Navn = "Olav"};

            db.SaveChanges();

            //Relasjon mellom bruker og venner
            
            db.FriendRequests.AddRange(new List<FriendRequest>{
             new FriendRequest(user2,  user, user.FirstName, user.LastName, user.BirthDate, user.ProfilePicture),
             new FriendRequest(user2,  user3, user3.FirstName, user3.LastName, user3.BirthDate, user3.ProfilePicture),
             
             new FriendRequest(user,  user2, user2.FirstName, user2.LastName, user2.BirthDate, user2.ProfilePicture),
             new FriendRequest(user3,  user2, user2.FirstName, user2.LastName, user2.BirthDate, user2.ProfilePicture),
             
           //  new FriendRequest( user,  user3, user3.FirstName, user3.LastName, user3.BirthDate, user3.ProfilePicture, user3.Id)
            });
            
            
            
          //  var relation = new FriendRequest{friendFrom = user2, friendTo = user, Nickname = user.Nickname};
            //var relation2 = new FriendRequest{friendFrom = user2, friendTo = user3,  Nickname = user2.Nickname};
            //var relation3 = new FriendRequest{friendFrom = user, friendTo = user3,  Nickname = user3.Nickname};
          //  var relation2 = new FriendRequest{Navn = venn, Owner = user2};
           // var relation3 = new FriendRequest{Navn = venn2, Owner = user};
            
           //Lager Ønskeliste til bruker

           var wishlist1 = new WishList{Owner = user2, Title = "List1",Category = "Bursdag", Privacy = "Friends", Deadline = DateTime.Today};
           var wishlist2 = new WishList{Owner = user2, Title = "List2",Category = "Jul", Privacy = "Friends", Deadline = DateTime.Today};
           var wishlist3 = new WishList{Owner = user, Title = "List3",Category = "Bursdag", Privacy = "Friends", Deadline = DateTime.Today};
           var wishlist4 = new WishList{Owner = user3, Title = "List4",Category = "Bryllup", Privacy = "Friends", Deadline = DateTime.Today};
           
           db.Add(wishlist1);
           db.Add(wishlist2);
           db.Add(wishlist3);
           db.Add(wishlist4);
           db.SaveChanges();

           //Lager Shoppingliste til bruker
        //   var shoppinglist = new ShoppingList{Owner = user};

      //     db.Add(shoppinglist);
             
           //Legger til produkter i ønskeliste
           var product1 = new Product{Name = "List1 prod1", Price = 11,Notes = "test 1", WishListId = 1, Url = "www.test1.no", Taken = true, Bought = false};
           var product2 = new Product{Name = "List1 prod2", Price = 12,Notes = "test 2", WishListId = 1, Url = "www.test2.no", Taken = false, Bought = false};
           var product3 = new Product{Name = "List2 prod1", Price = 21,Notes = "test 1", WishListId = 2, Url = "www.test3.no", Taken = false, Bought = false};
           var product4 = new Product{Name = "List2 prod2", Price = 22,Notes = "test 1", WishListId = 2, Url = "www.test4.no", Taken = false, Bought = false};
           var product5 = new Product{Name = "Vase", Price = 600, Notes = "Ming", WishListId = 4, Url = "www.gaver.no", Taken = true, Bought = false };
           
           db.Add(product1);
           db.Add(product2);
           db.Add(product3);
           db.Add(product4);
           db.Add(product5);
 
           //db.Add(user);
       //    db.Add(relation2);
         //  db.Add(relation3);
 
          // db.Add(product1);
           //db.Add(product2);

           db.SaveChanges();
           
         //  var shoppinglist1 = new ShoppingList{};
           
           // lager lister til user 2
          // var List1 = new WishList{Owner = user2,  };
        }
    }
}