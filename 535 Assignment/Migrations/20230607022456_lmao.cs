using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _535_Assignment.Migrations
{
    public partial class lmao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    AppUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.AppUserId);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(1064)", maxLength: 1064, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "Decimal(19,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingList",
                columns: table => new
                {
                    ShoppingListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingList", x => x.ShoppingListId);
                    table.ForeignKey(
                        name: "FK_ShoppingList_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "AppUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemsInList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    ListId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsInList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemsInList_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemsInList_ShoppingList_ListId",
                        column: x => x.ListId,
                        principalTable: "ShoppingList",
                        principalColumn: "ShoppingListId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "AppUserId", "Password", "Role", "UserName" },
                values: new object[] { 1, "$2a$11$0xwyrJ15VGZ0jkROz0sxOeafbldvVyha/eitYtUj41onxCqC4nsq2", "Admin", "You" });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemId", "ItemName", "Unit", "UnitPrice" },
                values: new object[,]
                {
                    { 1, "Fresh Tomatoes", "500g", 5.9m },
                    { 2, "Watermelon", "Whole", 6.6m },
                    { 3, "Cucumber", "1 whole", 1.9m },
                    { 4, "Red Potato Washed", "1kg", 4m },
                    { 5, "Red Tipped Bananas", "1kg", 4.9m },
                    { 6, "Red Onion", "1kg", 3.5m },
                    { 7, "Carrots", "1kg", 2m },
                    { 8, "Iceburg Lettuce", "1", 2.5m },
                    { 9, "Helga's Wholemeal", "1", 3.7m },
                    { 10, "Free Range Chicken", "1kg", 7.5m },
                    { 11, "Manning Valley 6-pk", "6 eggs", 3.6m },
                    { 12, "A2 Light Milk", "1 litre", 2.9m },
                    { 13, "Chobani Strawberry Yoghurt", "1", 1.5m },
                    { 14, "Lurpark Salted Blend", "250g", 5m },
                    { 15, "Bega Farmers Tasty", "250g", 4m },
                    { 16, "Babybel Mini", "100g", 4.2m },
                    { 17, "Cobram EVOO", "375ml", 8m },
                    { 18, "Heinz Tomato Soup", "535g", 2.5m },
                    { 19, "John West Tuna can", "95g", 1.5m },
                    { 20, "Cadbury Dairy Milk", "200g", 5m },
                    { 21, "Coca Cola", "2 litre", 2.85m },
                    { 22, "Smith's Original Share Pack Crisps", "170g", 3.29m },
                    { 23, "Birds Eye Fish Fingers", "375g", 4.5m },
                    { 24, "Berri Orange Juice", "2 litre", 6m },
                    { 25, "Vegemite", "380g", 6m },
                    { 26, "Cheddar Shapes", "175g", 2m },
                    { 27, "Colgate Total ToothPaste", "110g", 3.5m },
                    { 28, "Milo Chocolate Malt", "200g", 4m },
                    { 29, "Weet Bix Saniatarium Organic", "750g", 5.33m },
                    { 30, "Lindt Excellence 70% Cocoa Block", "100g", 4.25m },
                    { 31, "Original Tim Tams Chocolate", "200g", 3.65m },
                    { 32, "Philadelphia Original Cream Cheese", "250g", 4.3m },
                    { 33, "Moccona Classic Instant Medium Roast", "100g", 6m },
                    { 34, "Capilano Sqeezable Honey", "500g", 7.35m },
                    { 35, "Nutella Jar", "400g", 4m },
                    { 36, "Arnott's Scotch Finger", "250g", 2.85m },
                    { 37, "South Cape Greek Feta", "200g", 5m },
                    { 38, "Salsa Pasta Tomato Basil Sauce", "420g", 4.5m },
                    { 39, "Primo English Ham", "100g", 3m },
                    { 40, "Primo Short Cut Rindless Bacon", "175g", 5m },
                    { 41, "Golden Circle Pinapple Pieces in natural juice", "440g", 3.25m }
                });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemId", "ItemName", "Unit", "UnitPrice" },
                values: new object[] { 42, "San Renmo Linguine Pasta No 1", "500g", 1.95m });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "ItemId", "ItemName", "Unit", "UnitPrice" },
                values: new object[] { 43, "Granny Smith Apples", "1kg", 5.5m });

            migrationBuilder.InsertData(
                table: "ShoppingList",
                columns: new[] { "ShoppingListId", "Created", "ListName", "UserId" },
                values: new object[] { 1, new DateTime(2023, 6, 7, 12, 24, 56, 114, DateTimeKind.Local).AddTicks(434), "Your List", 1 });

            migrationBuilder.InsertData(
                table: "ItemsInList",
                columns: new[] { "Id", "ItemId", "ListId", "Quantity" },
                values: new object[] { 1, 38, 1, 34 });

            migrationBuilder.CreateIndex(
                name: "IX_ItemsInList_ItemId",
                table: "ItemsInList",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsInList_ListId",
                table: "ItemsInList",
                column: "ListId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingList_UserId",
                table: "ShoppingList",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemsInList");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "ShoppingList");

            migrationBuilder.DropTable(
                name: "AppUsers");
        }
    }
}
