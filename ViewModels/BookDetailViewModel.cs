namespace AspNetWeek2.Mvc.ViewModels;

public class BookDetailViewModel
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

    public string PriceText => $"{UnitPrice:N0} VND";
    public decimal InventoryValue => UnitPrice * Quantity;
    public string InventoryValueText => $"{InventoryValue:N0} VND";
    public string LastUpdatedAtText => LastUpdatedAt.ToString("dd/MM/yyyy HH:mm");

    public string StockStatus
    {
        get
        {
            if (Quantity <= 0) return "Hết hàng";
            if (Quantity <= MinStock) return "Cần nhập thêm";
            return "Còn hàng";
        }
    }

    public string ReorderSuggestion
    {
        get
        {
            if (Quantity <= 0) return "Cần nhập ngay vì sách đã hết hàng.";
            if (Quantity <= MinStock) return $"Nên nhập thêm. Hiện tại chỉ còn {Quantity} cuốn, mức tối thiểu là {MinStock}.";
            return "Số lượng sách trong kho đang ổn định.";
        }
    }

    public string StockStatusClass
    {
        get
        {
            if (Quantity <= 0) return "danger";
            if (Quantity <= MinStock) return "warning";
            return "success";
        }
    }

    public string StockBadgeClass => $"badge-{StockStatusClass}";
}
