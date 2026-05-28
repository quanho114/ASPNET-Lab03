using AspNetWeek3.Mvc.Models;
using AspNetWeek3.Mvc.ViewModels;

namespace AspNetWeek3.Mvc.Services;

public class BookService
{
    private readonly List<Book> _books = new()
    {
        new Book { Id = 1, Sku = "BA-1001", Name = "Dế Mèn Phiêu Lưu Ký", Category = "Thiếu nhi", Supplier = "NXB Kim Đồng", UnitPrice = 45000, Quantity = 60, MinStock = 10, LastUpdatedAt = new DateTime(2025, 1, 10), Rating = 4.8 },
        new Book { Id = 2, Sku = "BA-1002", Name = "Đắc Nhân Tâm", Category = "Kỹ năng sống", Supplier = "NXB Tổng Hợp", UnitPrice = 88000, Quantity = 5, MinStock = 15, LastUpdatedAt = new DateTime(2025, 2, 12), Rating = 4.5 },
        new Book { Id = 3, Sku = "BA-1003", Name = "Clean Code in C#", Category = "IT", Supplier = "NXB Trẻ", UnitPrice = 250000, Quantity = 0, MinStock = 5, LastUpdatedAt = new DateTime(2024, 12, 1), Rating = 5.0 },
        new Book { Id = 4, Sku = "BA-1004", Name = "Lược Sử Loài Người", Category = "Khoa học - Lịch sử", Supplier = "Nhã Nam", UnitPrice = 180000, Quantity = 12, MinStock = 10, LastUpdatedAt = new DateTime(2025, 3, 5), Rating = 4.7 },
        new Book { Id = 5, Sku = "BA-1005", Name = "Nhà Giả Kim", Category = "Văn học", Supplier = "Nhã Nam", UnitPrice = 120000, Quantity = 3, MinStock = 10, LastUpdatedAt = new DateTime(2025, 4, 1), Rating = 4.9 },
        new Book { Id = 6, Sku = "BA-1006", Name = "Đất Rừng Phương Nam", Category = "Văn học", Supplier = "NXB Kim Đồng", UnitPrice = 65000, Quantity = 25, MinStock = 8, LastUpdatedAt = new DateTime(2025, 1, 15), Rating = 4.6 },
        new Book { Id = 7, Sku = "BA-1007", Name = "Tuổi Trẻ Đáng Giá Bao Nhiêu", Category = "Kỹ năng sống", Supplier = "Nhã Nam", UnitPrice = 70000, Quantity = 40, MinStock = 15, LastUpdatedAt = new DateTime(2025, 3, 20), Rating = 4.4 },
        new Book { Id = 8, Sku = "BA-1008", Name = "Design Patterns", Category = "IT", Supplier = "NXB Trẻ", UnitPrice = 290000, Quantity = 15, MinStock = 5, LastUpdatedAt = new DateTime(2025, 2, 10), Rating = 4.9 },
        new Book { Id = 9, Sku = "BA-1009", Name = "Vũ Trụ (Cosmos)", Category = "Khoa học - Lịch sử", Supplier = "Nhã Nam", UnitPrice = 220000, Quantity = 8, MinStock = 10, LastUpdatedAt = new DateTime(2025, 4, 5), Rating = 4.8 },
        new Book { Id = 10, Sku = "BA-1010", Name = "Không Gia Đình", Category = "Thiếu nhi", Supplier = "NXB Kim Đồng", UnitPrice = 110000, Quantity = 0, MinStock = 10, LastUpdatedAt = new DateTime(2025, 1, 20), Rating = 4.7 }
    };

    public List<Book> GetAll() => _books;

    public Book? GetById(int id) => _books.FirstOrDefault(b => b.Id == id);

    public List<Book> Search(string? keyword, decimal? minPrice)
    {
        var query = _books.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(b =>
                b.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                b.Category.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                b.Sku.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                b.Supplier.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }

        if (minPrice.HasValue)
        {
            query = query.Where(b => b.UnitPrice >= minPrice.Value);
        }

        return query.ToList();
    }

    public Book Create(BookCreateViewModel model)
    {
        var newId = _books.Count == 0
            ? 1
            : _books.Max(b => b.Id) + 1;

        var book = new Book
        {
            Id = newId,
            Sku = model.Sku,
            Name = model.Name,
            Category = model.Category,
            Supplier = model.Supplier,
            UnitPrice = model.UnitPrice,
            Quantity = model.Quantity,
            MinStock = model.MinStock,
            LastUpdatedAt = DateTime.Now,
            Rating = 5.0
        };

        _books.Add(book);
        return book;
    }

    public BookStatsViewModel GetStats()
    {
        var totalTitles = _books.Count;
        var totalCopies = _books.Sum(b => b.Quantity);
        var totalValue = _books.Sum(b => b.UnitPrice * b.Quantity);

        var categoryStats = _books.GroupBy(b => b.Category)
                                  .Select(g => new CategoryStat
                                  {
                                      CategoryName = g.Key,
                                      Count = g.Count(),
                                      TotalValue = g.Sum(b => b.UnitPrice * b.Quantity)
                                  }).ToList();

        return new BookStatsViewModel
        {
            TotalTitles = totalTitles,
            TotalCopies = totalCopies,
            TotalInventoryValue = totalValue,
            OutOfStockCount = _books.Count(b => b.Quantity <= 0),
            NeedReorderCount = _books.Count(b => b.Quantity > 0 && b.Quantity <= b.MinStock),
            InStockCount = _books.Count(b => b.Quantity > b.MinStock),
            MaxInventoryValue = _books.Any() ? _books.Max(b => b.UnitPrice * b.Quantity) : 0,
            AverageInventoryValue = totalTitles > 0 ? totalValue / totalTitles : 0,
            AverageQuantity = totalTitles > 0 ? (double)totalCopies / totalTitles : 0,
            CategoryBreakdown = categoryStats
        };
    }

    public bool Restock(int id, int amount)
    {
        var book = GetById(id);
        if (book == null) return false;
        book.Quantity += amount;
        book.LastUpdatedAt = DateTime.Now;
        return true;
    }

    public bool Sell(int id, int amount)
    {
        var book = GetById(id);
        if (book == null || book.Quantity < amount) return false;
        book.Quantity -= amount;
        book.LastUpdatedAt = DateTime.Now;
        return true;
    }

    public bool Delete(int id)
    {
        var book = GetById(id);
        if (book == null) return false;
        _books.Remove(book);
        return true;
    }
}
