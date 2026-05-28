namespace AspNetWeek3.Mvc.ViewModels;

public class BookStatsViewModel
{
    public int TotalTitles { get; set; }
    public int TotalCopies { get; set; }
    public decimal TotalInventoryValue { get; set; }
    public int OutOfStockCount { get; set; }
    public int NeedReorderCount { get; set; }
    public int InStockCount { get; set; }
    public decimal MaxInventoryValue { get; set; }
    public decimal AverageInventoryValue { get; set; }
    public double AverageQuantity { get; set; }
    public List<CategoryStat> CategoryBreakdown { get; set; } = new();

    public string TotalInventoryValueText => $"{TotalInventoryValue:N0} VND";
    public string MaxInventoryValueText => $"{MaxInventoryValue:N0} VND";
    public string AverageInventoryValueText => $"{AverageInventoryValue:N0} VND";
}
