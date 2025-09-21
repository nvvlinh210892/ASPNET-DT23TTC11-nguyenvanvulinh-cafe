# ☕ Góc Phố - Website Bán Cafe Online

[![.NET](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/download)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-9.0-green.svg)](https://docs.microsoft.com/aspnet/core)
[![SQLite](https://img.shields.io/badge/Database-SQLite-lightblue.svg)](https://sqlite.org)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## �� Tổng quan

**Góc Phố** là một website bán cafe online được xây dựng bằng ASP.NET Core 9.0, cung cấp trải nghiệm mua sắm cafe trực tuyến với giao diện thân thiện và dễ sử dụng.

## ✨ Tính năng chính

### 👥 Quản lý người dùng
- Đăng ký tài khoản mới
- Đăng nhập/Đăng xuất an toàn
- Quản lý thông tin cá nhân
- Phân quyền người dùng (User/Admin)

### 🛒 Hệ thống mua sắm
- Duyệt menu cafe đa dạng
- Thêm sản phẩm vào giỏ hàng
- Quản lý giỏ hàng (thêm/xóa/sửa số lượng)
- Thanh toán và đặt hàng
- Theo dõi lịch sử đơn hàng

### 🎛️ Quản trị hệ thống (Admin)
- Quản lý sản phẩm (CRUD operations)
- Quản lý danh mục sản phẩm
- Quản lý đơn hàng và cập nhật trạng thái
- Quản lý người dùng
- Dashboard thống kê

### �� Giao diện người dùng
- Responsive design (tương thích mobile)
- Giao diện hiện đại với Bootstrap
- Trải nghiệm người dùng mượt mà
- Tối ưu hóa cho các thiết bị khác nhau

## ��️ Công nghệ sử dụng

### Backend
- **ASP.NET Core 9.0** - Web framework
- **Entity Framework Core 9.0** - ORM (Object-Relational Mapping)
- **ASP.NET Identity** - Authentication & Authorization
- **SQLite** - Database

### Frontend
- **Razor Pages** - Server-side rendering
- **Bootstrap 5** - CSS framework
- **jQuery** - JavaScript library
- **jQuery Validation** - Client-side validation

### Development Tools
- **Visual Studio 2022** / **VS Code**
- **Entity Framework Tools** - Database migrations
- **SQLite Browser** - Database management

## 🚀 Cài đặt và chạy dự án

### Yêu cầu hệ thống
- .NET 9.0 SDK hoặc mới hơn
- Visual Studio 2022 hoặc VS Code
- Git

### 📥 Cài đặt phần mềm cần thiết

1. **Tải và cài đặt Visual Studio 2022:**
   - Truy cập: https://visualstudio.microsoft.com/downloads/
   - Chọn **Community** (miễn phí) hoặc **Professional/Enterprise**
   - Trong quá trình cài đặt, chọn workload **"ASP.NET and web development"**

2. **Cài đặt .NET 9.0 SDK:**
   - Truy cập: https://dotnet.microsoft.com/download/dotnet/9.0
   - Tải và cài đặt **.NET 9.0 SDK**

### 🖥️ Chạy dự án trên Visual Studio

1. **Mở dự án:**
   - Mở **Visual Studio 2022**
   - **File** → **Open** → **Project/Solution**
   - Điều hướng đến thư mục dự án
   - Chọn file **`GocPho.csproj`** → **Open**

2. **Restore NuGet Packages:**
   - Click chuột phải vào **Solution** trong **Solution Explorer**
   - Chọn **Restore NuGet Packages**
   - Hoặc: **Tools** → **NuGet Package Manager** → **Package Manager Console**
   - Chạy lệnh: `dotnet restore`

3. **Build dự án:**
   - Click chuột phải vào project **GocPho**
   - Chọn **Build** hoặc **Rebuild**
   - Hoặc sử dụng phím tắt: **Ctrl + Shift + B**

4. **Cập nhật Database:**
   - Mở **Package Manager Console** (Tools → NuGet Package Manager → Package Manager Console)
   - Chạy lệnh: `Update-Database`

5. **Chạy ứng dụng:**
   - **Cách 1:** Click nút **▶️ IIS Express** (màu xanh) trên thanh toolbar
   - **Cách 2:** Click chuột phải vào project → **Debug** → **Start New Instance**
   - **Cách 3:** Sử dụng phím tắt **F5** (Debug) hoặc **Ctrl + F5** (Run without debugging)

6. **Truy cập website:**
   - Visual Studio sẽ tự động mở trình duyệt
   - **HTTP:** `http://localhost:5198/`
   - **HTTPS:** `https://localhost:7102/`

### 💻 Chạy dự án bằng Command Line

1. **Clone repository**
   ```bash
   git clone <repository-url>
   cd ASPNET-DT23TTC11-nguyenvanvulinh-cafe
   ```

2. **Restore packages**
   ```bash
   dotnet restore
   ```

3. **Cập nhật database**
   ```bash
   dotnet ef database update
   ```

4. **Chạy ứng dụng**
   ```bash
   dotnet run
   ```

5. **Truy cập website**
   - Mở trình duyệt và truy cập: `http://localhost:5198/`

## �� Tài khoản mặc định

### Admin Account
- **Email:** admin@gocpho.com
- **Username:** admin
- **Password:** GocPho@2025!
- **Role:** Administrator

### Quyền hạn Admin
- Quản lý sản phẩm và danh mục
- Xem và cập nhật đơn hàng
- Quản lý người dùng
- Truy cập dashboard thống kê

## �� Cấu trúc dự án
```
ASPNET-DT23TTC11-nguyenvanvulinh-cafe/
├── Controllers/           # MVC Controllers
│   ├── AccountController.cs
│   ├── AdminController.cs
│   ├── CartController.cs
│   ├── HomeController.cs
│   ├── OrderController.cs
│   └── ProductController.cs
├── Data/                 # Database context và seed data
│   ├── ApplicationDbContext.cs
│   └── SeedData.cs
├── Models/               # Data models
│   ├── CartItem.cs
│   ├── Order.cs
│   ├── UserViewModel.cs
│   └── ViewModels.cs
├── Services/             # Business logic services
│   └── CartService.cs
├── Views/                # Razor views
│   ├── Account/
│   ├── Admin/
│   ├── Cart/
│   ├── Home/
│   ├── Order/
│   ├── Product/
│   └── Shared/
├── wwwroot/              # Static files
│   ├── css/
│   ├── js/
│   ├── images/
│   └── lib/
├── Migrations/           # EF Core migrations
├── Program.cs            # Application entry point
└── GocPho.csproj        # Project file
```

## 🗄️ Database Schema

### Các bảng chính
- **AspNetUsers** - Thông tin người dùng
- **AspNetRoles** - Vai trò người dùng
- **Products** - Sản phẩm cafe
- **Categories** - Danh mục sản phẩm
- **Orders** - Đơn hàng
- **OrderItems** - Chi tiết đơn hàng
- **CartItems** - Giỏ hàng (Session-based)

## 🔧 Cấu hình

### Connection String
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=GocPho.db"
  }
}
```

### Identity Configuration
- Password yêu cầu: Tối thiểu 6 ký tự
- Bao gồm: Chữ số, chữ thường
- Không yêu cầu: Chữ hoa, ký tự đặc biệt
- Xác thực email: Tắt (Development mode)

## 📊 API Endpoints

### Public Endpoints
- `GET /` - Trang chủ
- `GET /Product/Menu` - Menu sản phẩm
- `GET /Product/Details/{id}` - Chi tiết sản phẩm
- `GET /Account/Login` - Đăng nhập
- `GET /Account/Register` - Đăng ký

### Protected Endpoints (User)
- `GET /Cart` - Giỏ hàng
- `POST /Cart/Add` - Thêm vào giỏ hàng
- `GET /Order/Checkout` - Thanh toán
- `GET /Order/MyOrders` - Đơn hàng của tôi

### Admin Endpoints
- `GET /Admin` - Dashboard admin
- `GET /Admin/Products` - Quản lý sản phẩm
- `GET /Admin/Orders` - Quản lý đơn hàng
- `GET /Admin/Users` - Quản lý người dùng

## 🚀 Triển khai (Deployment)

### Development
```bash
dotnet run --environment Development
```

### Production
```bash
dotnet publish -c Release
dotnet run --environment Production
```

## 🧪 Testing

### Chạy tests (nếu có)
```bash
dotnet test
```

### Kiểm tra database
```bash
dotnet ef database update
```

##  Changelog

- ✅ Khởi tạo dự án ASP.NET Core 9.0
- ✅ Tích hợp ASP.NET Identity
- ✅ Thiết kế database với Entity Framework Core
- ✅ Xây dựng hệ thống giỏ hàng
- ✅ Phát triển giao diện admin
- ✅ Responsive design với Bootstrap
- ✅ Seed data và tài khoản admin

##  Đóng góp (Contributing)

1. Fork dự án
2. Tạo feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Mở Pull Request

##  License

Dự án này được phân phối dưới MIT License. Xem file `LICENSE` để biết thêm chi tiết.

## 👨‍ Tác giả

**Nguyễn Văn Vũ Linh**
- Email: linhnvv210892@sv-onuni.edu.vn
- GitHub: https://github.com/nvvlinh210892/ASPNET-DT23TTC11-nguyenvanvulinh-cafe
- Điện thoại : 0975720128
## 📞 Hỗ trợ

Nếu bạn gặp vấn đề hoặc có câu hỏi, vui lòng:
- Tạo issue trên GitHub
- Liên hệ qua email: [support-email@example.com]

## 🙏 Lời cảm ơn

- ASP.NET Core team cho framework tuyệt vời
- Bootstrap team cho UI components
- Entity Framework team cho ORM mạnh mẽ
- Cộng đồng .NET Việt Nam

---

**Góc Phố** - Nơi hương vị cafe được tôn vinh! ☕✨
```

Tôi đã tạo file README.md chi tiết cho dự án cafe Góc Phở của bạn. File này bao gồm:

## 📋 Nội dung chính:

1. **Tổng quan dự án** - Mô tả ngắn gọn về website bán cafe
2. **Tính năng chính** - Liệt kê đầy đủ các chức năng của hệ thống
3. **Công nghệ sử dụng** - Backend, Frontend và Development tools
4. **Hướng dẫn cài đặt** - Các bước chi tiết để chạy dự án
5. **Tài khoản mặc định** - Thông tin admin account
6. **Cấu trúc dự án** - Mô tả các thư mục và file quan trọng
7. **Database Schema** - Thông tin về các bảng dữ liệu
8. **API Endpoints** - Danh sách các route chính
9. **Hướng dẫn triển khai** - Cách deploy ứng dụng
10. **Changelog** - Lịch sử phát triển dự án

## ✨ Đặc điểm nổi bật:

- **Tiếng Việt** với thuật ngữ kỹ thuật tiếng Anh (theo yêu cầu)
- **Badges** hiện đại để hiển thị công nghệ
- **Cấu trúc rõ ràng** với emoji và formatting đẹp mắt
- **Thông tin chi tiết** về tài khoản admin và cấu hình
- **Hướng dẫn đầy đủ** từ cài đặt đến triển khai
- **Professional** và dễ hiểu cho cả developer và stakeholder

File README.md này sẽ giúp người khác hiểu rõ về dự án và có thể dễ dàng setup để chạy thử nghiệm!
