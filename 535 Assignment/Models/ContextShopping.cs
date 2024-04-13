using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace _535_Assignment.Models
{
    public class ContextShopping : DbContext
    {
        public DbSet<ShoppingList> ShoppingList { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemsToListConnection> ItemsInList { get; set; }

        public ContextShopping(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ShoppingList>()
                .HasOne(c => c.users)
                .WithMany(c => c.ShoppingLists)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ItemsToListConnection>()
                 .HasOne(c => c.item)
                 .WithMany(ce => ce.ItemList)
                 .HasForeignKey(c => c.ItemId)
                 .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ItemsToListConnection>()
                .HasOne(c => c.list)
                .WithMany(ce => ce.ItemList)
                .HasForeignKey(c => c.ListId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AppUser>().HasData(
                    new AppUser
                    {
                        AppUserId = 1,
                        UserName = "You",
                        Password = BCrypt.Net.BCrypt.EnhancedHashPassword("Password"),
                        Role = "Admin"
                    }); 

            builder.Entity<ShoppingList>().HasData(
                    new ShoppingList
                    {
                        ShoppingListId = 1,
                        ListName = "Your List",
                        Created = DateTime.Now,
                        UserId = 1,
                    });

            builder.Entity<Item>().HasData(
                    new Item
                    {
                        ItemId = 1,
                        ItemName = "Fresh Tomatoes",
                        Unit = "500g",
                        UnitPrice = 5.90,
                    },

                    new Item
                    {
                        ItemId = 2,
                        ItemName = "Watermelon",
                        Unit = "Whole",
                        UnitPrice = 6.60,
                    },

                    new Item
                    {
                        ItemId = 3,
                        ItemName = "Cucumber",
                        Unit = "1 whole",
                        UnitPrice = 1.90,
                    },

                    new Item
                    {
                        ItemId = 4,
                        ItemName = "Red Potato Washed",
                        Unit = "1kg",
                        UnitPrice = 4.00,
                    },

                    new Item
                    {
                        ItemId = 5,
                        ItemName = "Red Tipped Bananas",
                        Unit = "1kg",
                        UnitPrice = 4.90,
                    },

                    new Item
                    {
                        ItemId = 6,
                        ItemName = "Red Onion",
                        Unit = "1kg",
                        UnitPrice = 3.50,
                    },

                    new Item
                    {
                        ItemId = 7,
                        ItemName = "Carrots",
                        Unit = "1kg",
                        UnitPrice = 2.00,
                    },

                    new Item
                    {
                        ItemId = 8,
                        ItemName = "Iceburg Lettuce",
                        Unit = "1",
                        UnitPrice = 2.50,
                    },

                    new Item
                    {
                        ItemId = 9,
                        ItemName = "Helga's Wholemeal",
                        Unit = "1",
                        UnitPrice = 3.70,
                    },

                    new Item
                    {
                        ItemId = 10,
                        ItemName = "Free Range Chicken",
                        Unit = "1kg",
                        UnitPrice = 7.50,
                    },

                    new Item
                    {
                        ItemId = 11,
                        ItemName = "Manning Valley 6-pk",
                        Unit = "6 eggs",
                        UnitPrice = 3.60,
                    },

                    new Item
                    {
                        ItemId = 12,
                        ItemName = "A2 Light Milk",
                        Unit = "1 litre",
                        UnitPrice = 2.90,
                    },

                    new Item
                    {
                        ItemId = 13,
                        ItemName = "Chobani Strawberry Yoghurt",
                        Unit = "1",
                        UnitPrice = 1.50,
                    },

                    new Item
                    {
                        ItemId = 14,
                        ItemName = "Lurpark Salted Blend",
                        Unit = "250g",
                        UnitPrice = 5.00,
                    },

                    new Item
                    {
                        ItemId = 15,
                        ItemName = "Bega Farmers Tasty",
                        Unit = "250g",
                        UnitPrice = 4.00,
                    },

                    new Item
                    {
                        ItemId = 16,
                        ItemName = "Babybel Mini",
                        Unit = "100g",
                        UnitPrice = 4.20,
                    },

                    new Item
                    {
                        ItemId = 17,
                        ItemName = "Cobram EVOO",
                        Unit = "375ml",
                        UnitPrice = 8.00,
                    },

                    new Item
                    {
                        ItemId = 18,
                        ItemName = "Heinz Tomato Soup",
                        Unit = "535g",
                        UnitPrice = 2.50,
                    },

                    new Item
                    {
                        ItemId = 19,
                        ItemName = "John West Tuna can",
                        Unit = "95g",
                        UnitPrice = 1.50,
                    },

                    new Item
                    {
                        ItemId = 20,
                        ItemName = "Cadbury Dairy Milk",
                        Unit = "200g",
                        UnitPrice = 5.00,
                    },

                    new Item
                    {
                        ItemId = 21,
                        ItemName = "Coca Cola",
                        Unit = "2 litre",
                        UnitPrice = 2.85,
                    },

                    new Item
                    {
                        ItemId = 22,
                        ItemName = "Smith's Original Share Pack Crisps",
                        Unit = "170g",
                        UnitPrice = 3.29,
                    },

                    new Item
                    {
                        ItemId = 23,
                        ItemName = "Birds Eye Fish Fingers",
                        Unit = "375g",
                        UnitPrice = 4.50,
                    },

                    new Item
                    {
                        ItemId = 24,
                        ItemName = "Berri Orange Juice",
                        Unit = "2 litre",
                        UnitPrice = 6.00,
                    },

                    new Item
                    {
                        ItemId = 25,
                        ItemName = "Vegemite",
                        Unit = "380g",
                        UnitPrice = 6.00,
                    },

                    new Item
                    {
                        ItemId = 26,
                        ItemName = "Cheddar Shapes",
                        Unit = "175g",
                        UnitPrice = 2.00,
                    },

                    new Item
                    {
                        ItemId = 27,
                        ItemName = "Colgate Total ToothPaste",
                        Unit = "110g",
                        UnitPrice = 3.50,
                    },

                    new Item
                    {
                        ItemId = 28,
                        ItemName = "Milo Chocolate Malt",
                        Unit = "200g",
                        UnitPrice = 4.00,
                    },

                    new Item
                    {
                        ItemId = 29,
                        ItemName = "Weet Bix Saniatarium Organic",
                        Unit = "750g",
                        UnitPrice = 5.33,
                    },

                    new Item
                    {
                        ItemId = 30,
                        ItemName = "Lindt Excellence 70% Cocoa Block",
                        Unit = "100g",
                        UnitPrice = 4.25,
                    },

                    new Item
                    {
                        ItemId = 31,
                        ItemName = "Original Tim Tams Chocolate",
                        Unit = "200g",
                        UnitPrice = 3.65,
                    },

                    new Item
                    {
                        ItemId = 32,
                        ItemName = "Philadelphia Original Cream Cheese",
                        Unit = "250g",
                        UnitPrice = 4.30,
                    },

                    new Item
                    {
                        ItemId = 33,
                        ItemName = "Moccona Classic Instant Medium Roast",
                        Unit = "100g",
                        UnitPrice = 6.00,
                    },

                    new Item
                    {
                        ItemId = 34,
                        ItemName = "Capilano Sqeezable Honey",
                        Unit = "500g",
                        UnitPrice = 7.35,
                    },

                    new Item
                    {
                        ItemId = 35,
                        ItemName = "Nutella Jar",
                        Unit = "400g",
                        UnitPrice = 4.00,
                    },

                    new Item
                    {
                        ItemId = 36,
                        ItemName = "Arnott's Scotch Finger",
                        Unit = "250g",
                        UnitPrice = 2.85,
                    },

                    new Item
                    {
                        ItemId = 37,
                        ItemName = "South Cape Greek Feta",
                        Unit = "200g",
                        UnitPrice = 5.00,
                    },

                    new Item
                    {
                        ItemId = 38,
                        ItemName = "Salsa Pasta Tomato Basil Sauce",
                        Unit = "420g",
                        UnitPrice = 4.50,
                    },

                    new Item
                    {
                        ItemId = 39,
                        ItemName = "Primo English Ham",
                        Unit = "100g",
                        UnitPrice = 3.00,
                    },

                    new Item
                    {
                        ItemId = 40,
                        ItemName = "Primo Short Cut Rindless Bacon",
                        Unit = "175g",
                        UnitPrice = 5.00,
                    },

                    new Item
                    {
                        ItemId = 41,
                        ItemName = "Golden Circle Pinapple Pieces in natural juice",
                        Unit = "440g",
                        UnitPrice = 3.25,
                    },

                    new Item
                    {
                        ItemId = 42,
                        ItemName = "San Renmo Linguine Pasta No 1",
                        Unit = "500g",
                        UnitPrice = 1.95,

                    },
                    
                    new Item
                    {
                        ItemId = 43,
                        ItemName = "Granny Smith Apples",
                        Unit = "1kg",
                        UnitPrice = 5.50,
                    });


            builder.Entity<ItemsToListConnection>().HasData(
                    new ItemsToListConnection
                    {
                        Id = 1,
                        ItemId = 38,
                        ListId = 1,
                        Quantity = 34
                    });

        }
    }
}
