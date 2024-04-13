namespace _535_Assignment.Models
{
    public class ItemsToListConnection
    {
        public int Id { get; set; }
        public int ItemId { get; set; }

        public int ListId { get; set; }

        public int Quantity { get; set; }

        public virtual ShoppingList list { get; set; }
        public virtual Item item { get; set; }

    }
}
