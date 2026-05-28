# Mini Bookstore Catalog - ASP.NET Core 8.0 MVC (Lab 03)

Dự án này là sản phẩm nâng cấp hoàn thiện từ bài Lab 02 thành bài Lab 03 theo chủ đề **Quản lý Danh mục Sách (Bookstore Catalog)** sử dụng framework **ASP.NET Core 8.0 MVC**. 

Ứng dụng đã được refactor toàn bộ tên lớp, dịch vụ, giao diện và biến từ đối tượng mẫu (`Product`) sang đối tượng nghiệp vụ thực tế là Sách (`Book`) để đáp ứng đúng yêu cầu đề bài.

---

## 🚀 Các tính năng chính được nâng cấp (Lab 03)

- **Layout dùng chung (`_Layout.cshtml`)**: Giao diện đồng nhất toàn trang, menu điều hướng linh hoạt sử dụng Tag Helpers.
- **Tái sử dụng giao diện với Partial Views**:
  - `_BookCard.cshtml`: Hiển thị thẻ thông tin sách trong danh sách chính.
  - `_BookSearchResultTable.cshtml`: Hiển thị bảng kết quả tìm kiếm sách trực quan.
- **Tag Helpers & Validation**: Sử dụng đầy đủ các tag helpers như `asp-controller`, `asp-action`, `asp-route-id`, `asp-for`, `asp-validation-for` kết hợp kiểm tra dữ liệu đầu vào bằng `DataAnnotations` trong `BookCreateViewModel`.
- **Form tìm kiếm nâng cao (GET)**: Tìm kiếm sách thời gian thực theo từ khóa (Mã SKU, Tên sách, Thể loại, Nhà xuất bản) và Đơn giá tối thiểu.
- **Bộ lọc kho hàng bằng JavaScript (Không cần tải lại trang)**: Lọc nhanh sách theo trạng thái (Còn hàng, Sắp hết, Hết hàng) trực tiếp tại trang danh sách.
- **Thêm sách mới (POST)**: Form tạo mới với đầy đủ validation và hiển thị thông báo thành công thông qua `TempData` và `RedirectToAction`.
- **Công cụ Quản trị Kho nhanh**: Cho phép nhập nhanh (+10 cuốn) hoặc bán nhanh (-1 cuốn) trực tiếp tại trang chi tiết sách.
- **Thống kê chi tiết kho hàng (`/Books/Stats`)**: Tổng quan số đầu sách, số lượng tồn kho, tổng giá trị kho, giá trị trung bình/cao nhất và biểu đồ phân tích theo từng thể loại.

---

## 🛠️ Công nghệ sử dụng

- **Backend**: C# 12, .NET 8.0, ASP.NET Core MVC.
- **Frontend**: HTML5, Vanilla CSS (Thiết kế hiện đại, responsive, không dùng Tailwind), JavaScript thuần.
- **Quản lý dữ liệu**: In-Memory Service (Singleton Lifecycle).
- **Quản lý mã nguồn**: Git & GitHub.

---

## 📂 Cấu trúc thư mục Refactored

```text
ASPNET-Lab03/
├── Controllers/
│   ├── BooksController.cs          # Điều hướng nghiệp vụ Sách
│   └── HomeController.cs           # Điều hướng trang chủ
├── Models/
│   ├── Book.cs                     # Model đại diện cho cuốn sách
│   └── ErrorViewModel.cs
├── Services/
│   └── BookService.cs              # Quản lý kho sách trong bộ nhớ (In-Memory)
├── ViewModels/
│   ├── BookCreateViewModel.cs      # Kiểm soát dữ liệu Form thêm mới
│   ├── BookDetailViewModel.cs      # Hiển thị chi tiết sách
│   ├── BookListItemViewModel.cs    # Hiển thị danh sách thu gọn
│   ├── BookSearchViewModel.cs      # Tìm kiếm sách
│   ├── BookStatsViewModel.cs        # Thống kê kho sách
│   └── CategoryStat.cs
├── Views/
│   ├── Books/                      # Thư mục View chứa các giao diện Sách
│   │   ├── Create.cshtml           # Form thêm mới
│   │   ├── Detail.cshtml           # Chi tiết sách & Action nhập/bán nhanh
│   │   ├── Index.cshtml            # Danh sách & Bộ lọc pills JavaScript
│   │   ├── Search.cshtml           # Form tìm kiếm sách (GET)
│   │   └── Stats.cshtml            # Trang thống kê
│   ├── Shared/
│   │   ├── _Layout.cshtml          # Layout chung
│   │   ├── _BookCard.cshtml        # Partial View thẻ sách
│   │   └── _BookSearchResultTable.cshtml  # Partial View bảng tìm kiếm
│   └── _ViewImports.cshtml         # Nạp Namespace & Tag Helpers
└── Program.cs                      # Cấu hình Services & Routing
```

---

## ⚙️ Hướng dẫn cài đặt & Chạy ứng dụng locally

### 1. Yêu cầu hệ thống
- Máy tính đã cài đặt **.NET 8.0 SDK** (kiểm tra bằng lệnh `dotnet --version`).

### 2. Các bước khởi chạy
Mở Terminal tại thư mục gốc của dự án và chạy các lệnh sau:

* **Build dự án**:
  ```bash
  dotnet build
  ```

* **Khởi chạy ứng dụng**:
  ```bash
  dotnet run
  ```

* **Truy cập ứng dụng**:
  Mở trình duyệt web bất kỳ và truy cập địa chỉ hiển thị trong terminal (mặc định là):
  - HTTP: `http://localhost:5256`
  - HTTPS: `https://localhost:7282`

---

## 📈 Quy tắc kiểm tra dữ liệu đầu vào (Validation)

- **Mã SKU**: Không được để trống, độ dài không vượt quá 20 ký tự.
- **Tên sách**: Không được để trống, độ dài không vượt quá 100 ký tự.
- **Thể loại / Nhà xuất bản**: Không được phép để trống.
- **Giá bán**: Phải lớn hơn hoặc bằng `1.000 VND`.
- **Số lượng / Mức tồn tối thiểu**: Không được phép âm.
