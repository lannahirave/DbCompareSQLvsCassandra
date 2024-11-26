using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQL.Ents;

public class SaleItem
{
    [Key]
    public int SaleItemId { get; set; }

    [Required]
    public int SaleId { get; set; }

    [ForeignKey(nameof(SaleId))]
    public Sale Sale { get; set; } = null!;

    [Required]
    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;

    [Required]
    public int Quantity { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }

    [NotMapped]
    public decimal TotalPrice => UnitPrice * Quantity;
}