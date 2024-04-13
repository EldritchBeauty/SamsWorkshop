using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _535_Assignment.Models
{
    public class AppUser
    {
        public int AppUserId { get; set; }
        [Required]
        [StringLength (50)]
        public string UserName { get; set; }
        [Required]
        [Column("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role { get; set; }
        public ICollection<ShoppingList> ShoppingLists { get; set; }
    }
}
