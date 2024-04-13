using System.ComponentModel.DataAnnotations;

namespace _535_Assignment.Models
{
    public class ShoppingList
    {
        [Required]
        public int ShoppingListId { get; set; }
        [Required]
        public string ListName { get; set; }
        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        public int UserId { get; set; }
        public virtual AppUser users { get; set; }
        public virtual ICollection<ItemsToListConnection> ItemList { get; set; }

    }
}
