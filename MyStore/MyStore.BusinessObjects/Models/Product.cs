using System.ComponentModel.DataAnnotations;

namespace MyStore.BusinessObjects.Models
{
    public partial class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int? CategoryId { get; set; }
        public int? UnitsInStock { get; set; }
        public decimal? UnitPrice { get; set; }
        public virtual Category? Category { get; set; }
    }
}