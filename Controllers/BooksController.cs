using AspNetWeek2.Mvc.Models;
using AspNetWeek2.Mvc.Services;
using AspNetWeek2.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AspNetWeek2.Mvc.Controllers;

public class BooksController : Controller
{
    private readonly BookService _bookService;

    public BooksController(BookService bookService)
    {
        _bookService = bookService;
    }

    public IActionResult Index()
    {
        var books = _bookService.GetAll().Select(ToListItemViewModel).ToList();
        return View(books);
    }

    public IActionResult Detail(int id)
    {
        var book = _bookService.GetById(id);
        if (book == null) return NotFound($"Không tìm thấy sách nào có id = {id}");
        return View(ToDetailViewModel(book));
    }

    public IActionResult Stats()
    {
        return View(_bookService.GetStats());
    }

    [HttpGet]
    public IActionResult Search(string? keyword, decimal? minPrice)
    {
        var books = _bookService.Search(keyword, minPrice)
            .Select(ToListItemViewModel)
            .ToList();

        var viewModel = new BookSearchViewModel
        {
            Keyword = keyword ?? "",
            MinPrice = minPrice,
            Books = books
        };

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var viewModel = new BookCreateViewModel
        {
            Quantity = 1,
            MinStock = 1
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(BookCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _bookService.Create(model);

        TempData["SuccessMessage"] = "Đã thêm sách thành công.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Restock(int id)
    {
        if (_bookService.Restock(id, 10))
        {
            TempData["SuccessMessage"] = "Đã nhập thêm 10 cuốn sách thành công.";
        }
        else
        {
            TempData["ErrorMessage"] = "Không thể thực hiện nhập kho.";
        }
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Sell(int id)
    {
        if (_bookService.Sell(id, 1))
        {
            TempData["SuccessMessage"] = "Đã bán 1 cuốn sách thành công.";
        }
        else
        {
            TempData["ErrorMessage"] = "Không thể thực hiện bán sách (Có thể sách đã hết hàng).";
        }
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        if (_bookService.Delete(id))
        {
            TempData["SuccessMessage"] = "Đã xóa sách khỏi danh mục thành công.";
            return RedirectToAction(nameof(Index));
        }
        TempData["ErrorMessage"] = "Không thể xóa sách này.";
        return RedirectToAction(nameof(Detail), new { id });
    }

    private static BookListItemViewModel ToListItemViewModel(Book b) => new BookListItemViewModel
    {
        Id = b.Id,
        Sku = b.Sku,
        Name = b.Name,
        Category = b.Category,
        Supplier = b.Supplier,
        UnitPrice = b.UnitPrice,
        Quantity = b.Quantity,
        MinStock = b.MinStock,
        Rating = b.Rating
    };

    private static BookDetailViewModel ToDetailViewModel(Book b) => new BookDetailViewModel
    {
        Id = b.Id,
        Sku = b.Sku,
        Name = b.Name,
        Category = b.Category,
        Supplier = b.Supplier,
        UnitPrice = b.UnitPrice,
        Quantity = b.Quantity,
        MinStock = b.MinStock,
        LastUpdatedAt = b.LastUpdatedAt,
        Rating = b.Rating
    };
}
