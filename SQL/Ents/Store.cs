using System.ComponentModel.DataAnnotations;

namespace SQL.Ents;

public class Store
{
    [Key]
    public int StoreId { get; set; }

    [Required] public string Name { get; set; } = null!;

    // Navigation property
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}