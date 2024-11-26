using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SQL.Ents;

public class Sale
{
    [Key]
    public int SaleId { get; set; }

    [Required]
    public int StoreId { get; set; }

    [ForeignKey(nameof(StoreId))]
    public Store Store { get; set; } = null!;

    [Required]
    public DateTimeOffset  SaleDate { get; set; }

    // Navigation property
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}