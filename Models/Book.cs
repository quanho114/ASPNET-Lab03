namespace AspNetWeek2.Mvc.Models;

public class Book
{
    public int Id { get; set; }
    public string Sku { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string Supplier { get; set; } = "";
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public int MinStock { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public double Rating { get; set; }
}
