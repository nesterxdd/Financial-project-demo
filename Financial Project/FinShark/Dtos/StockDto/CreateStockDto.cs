using System.ComponentModel.DataAnnotations;

public class CreateStockDto
{
    [Required]
	[MaxLength(10, ErrorMessage = "Symbol must be less than 10 characters")]
    public string Symbol { get; set; } = string.Empty;
    [Required]
	[MaxLength(10, ErrorMessage = "Company name must be less than 10 characters")]
	public string CompanyName { get; set; } = string.Empty;
    [Required]
    [Range(1, 1000000000)]
    public decimal Purchase { get; set; }
    [Required]
    [Range(0.001, 100)]
    public decimal LastDiv { get; set; }
    [Required]
	[MaxLength(10, ErrorMessage = "Industry must be less than 10 characters")]
	public string Industry { get; set; } = string.Empty;
    [Required]
    [Range(1,500000000)]
    public long MarketCap { get; set; }
}