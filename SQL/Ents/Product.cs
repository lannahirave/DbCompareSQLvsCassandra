using System.ComponentModel.DataAnnotations;

namespace SQL.Ents;

public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    // Navigation property
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}