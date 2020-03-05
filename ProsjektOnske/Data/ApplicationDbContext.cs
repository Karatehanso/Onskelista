using System;
using System.Collections.Generic;
using System.Text;
using olist.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using olist.Models;

namespace olist.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
     
            modelBuilder.Entity<FriendRequest>()
                .HasKey(t => new { t.Id });

            modelBuilder.Entity<FriendRequest>()
                .HasOne(pt => pt.friendFrom)
                .WithMany(p => p.Friend)
                .HasForeignKey(pt => pt.friendFromId);
               

           // modelBuilder.Entity<FriendRequest>().Property<int>("FriendId");
            /* modelBuilder.Entity<FriendRequest>()
                 .HasOne(pt => pt.friendTo)
                 .WithMany(t => t.FriendTwo)
                 .HasForeignKey(pt => pt.friendToId)
                 */
        }
        
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        
        public DbSet<olist.Models.FriendRequest> FriendRequests { get; set; }
        
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<Product> Products { get; set; }
        
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        
        public DbSet<olist.Models.PendingFriend> PendingFriends { get; set; }

    }
    
}