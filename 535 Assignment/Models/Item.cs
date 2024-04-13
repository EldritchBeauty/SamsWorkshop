using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _535_Assignment.Models
{
    public class Item
    {
        [Required]
        public int ItemId { get; set; }

        [StringLength(1064)]
        [Required]
        public string ItemName { get; set; }

        [StringLength(100)]
        [Required]
        public string Unit { get; set; }

        [Column(TypeName = "Decimal(19, 4)")]
        [Required]
        public double UnitPrice { get; set; }

         public ICollection<ItemsToListConnection> ItemList { get; set; }
    }
}
