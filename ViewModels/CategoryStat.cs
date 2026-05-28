namespace AspNetWeek2.Mvc.ViewModels;

public class CategoryStat
{
    public string CategoryName { get; set; } = "";
    public int Count { get; set; }
    public decimal TotalValue { get; set; }
    public string TotalValueText => $"{TotalValue:N0} VND";
}
