# �� Cấu Trúc Cơ Sở Dữ Liệu - Website Bán Cafe Góc Phố

## 🗄️ Tổng Quan Hệ Thống

**Database Engine:** SQLite  
**Framework:** ASP.NET Core 9.0 với Entity Framework Core  
**Authentication:** ASP.NET Identity  

---

## 📋 Danh Sách Các Bảng

### 1. �� **AspNetUsers** (Identity Table)
*Bảng quản lý người dùng hệ thống*

| Cột | Kiểu Dữ Liệu | Ràng Buộc | Mô Tả |
|-----|--------------|-----------|-------|
| `Id` | `string` | Primary Key | ID duy nhất của user |
| `UserName` | `string` | Required, Unique | Tên đăng nhập |
| `Email` | `string` | Required, Unique | Email đăng ký |
| `EmailConfirmed` | `bool` | Default: false | Trạng thái xác thực email |
| `PhoneNumber` | `string` | Nullable | Số điện thoại |
| `PhoneNumberConfirmed` | `bool` | Default: false | Trạng thái xác thực SĐT |
| `PasswordHash` | `string` | Required | Mã hóa mật khẩu |
| `SecurityStamp` | `string` | Required | Security stamp |
| `ConcurrencyStamp` | `string` | Required | Concurrency control |
| `LockoutEnabled` | `bool` | Default: true | Cho phép khóa tài khoản |
| `LockoutEnd` | `DateTimeOffset?` | Nullable | Thời gian khóa tài khoản |
| `AccessFailedCount` | `int` | Default: 0 | Số lần đăng nhập sai |
| `TwoFactorEnabled` | `bool` | Default: false | Bật xác thực 2 yếu tố |

### 2. ��️ **Categories** (Danh Mục Sản Phẩm)
*Bảng quản lý các danh mục cà phê*

| Cột | Kiểu Dữ Liệu | Ràng Buộc | Mô Tả |
|-----|--------------|-----------|-------|
| `Id` | `int` | Primary Key, Identity | ID duy nhất |
| `Name` | `string(50)` | Required, MaxLength(50) | Tên danh mục |
| `Description` | `string(200)` | MaxLength(200) | Mô tả danh mục |

**Dữ liệu mẫu:**
- Cà phê đen
- Cà phê sữa  
- Cà phê đá xay
- Đồ uống khác

### 3. ☕ **Products** (Sản Phẩm)
*Bảng quản lý các sản phẩm cà phê*

| Cột | Kiểu Dữ Liệu | Ràng Buộc | Mô Tả |
|-----|--------------|-----------|-------|
| `Id` | `int` | Primary Key, Identity | ID duy nhất |
| `Name` | `string(100)` | Required, MaxLength(100) | Tên sản phẩm |
| `Description` | `string(500)` | MaxLength(500) | Mô tả sản phẩm |
| `Price` | `decimal(18,2)` | Required, Range(0.01, MaxValue) | Giá bán (VNĐ) |
| `ImageUrl` | `string(200)` | MaxLength(200) | URL hình ảnh |
| `IsAvailable` | `bool` | Default: true | Trạng thái có sẵn |
| `CategoryId` | `int` | Foreign Key → Categories.Id | ID danh mục |
| `CreatedAt` | `DateTime` | Default: DateTime.Now | Ngày tạo |

**Dữ liệu mẫu:**
- Cà phê đen đá (25,000 VNĐ)
- Cà phê sữa đá (30,000 VNĐ)
- Bạc xỉu (35,000 VNĐ)
- Cappuccino (45,000 VNĐ)
- Americano (40,000 VNĐ)
- Frappuccino (50,000 VNĐ)
- Espresso (35,000 VNĐ)
- Latte (42,000 VNĐ)
- Mocha (48,000 VNĐ)

### 4. �� **CartItems** (Giỏ Hàng)
*Bảng quản lý sản phẩm trong giỏ hàng*

| Cột | Kiểu Dữ Liệu | Ràng Buộc | Mô Tả |
|-----|--------------|-----------|-------|
| `Id` | `int` | Primary Key, Identity | ID duy nhất |
| `UserId` | `string` | Required, Foreign Key → AspNetUsers.Id | ID người dùng |
| `ProductId` | `int` | Foreign Key → Products.Id | ID sản phẩm |
| `Quantity` | `int` | Required, Range(1, MaxValue) | Số lượng |
| `CreatedAt` | `DateTime` | Default: DateTime.Now | Ngày thêm vào giỏ |

**Computed Properties:**
- `TotalPrice` = `Product.Price * Quantity`

### 5. 📦 **Orders** (Đơn Hàng)
*Bảng quản lý đơn hàng*

| Cột | Kiểu Dữ Liệu | Ràng Buộc | Mô Tả |
|-----|--------------|-----------|-------|
| `Id` | `int` | Primary Key, Identity | ID duy nhất |
| `UserId` | `string` | Required, Foreign Key → AspNetUsers.Id | ID người dùng |
| `CustomerName` | `string(100)` | Required, MaxLength(100) | Tên khách hàng |
| `PhoneNumber` | `string(20)` | Required, MaxLength(20), Phone | Số điện thoại |
| `DeliveryAddress` | `string(200)` | Required, MaxLength(200) | Địa chỉ giao hàng |
| `Notes` | `string(500)` | MaxLength(500) | Ghi chú đơn hàng |
| `TotalAmount` | `decimal(18,2)` | Required | Tổng tiền đơn hàng |
| `Status` | `int` | Default: 0 (Pending) | Trạng thái đơn hàng |
| `OrderDate` | `DateTime` | Default: DateTime.Now | Ngày đặt hàng |
| `DeliveryDate` | `DateTime?` | Nullable | Ngày giao hàng |

**OrderStatus Enum:**
- `0` - Pending (Chờ xác nhận)
- `1` - Confirmed (Đã xác nhận)
- `2` - Preparing (Đang chuẩn bị)
- `3` - Delivering (Đang giao hàng)
- `4` - Delivered (Đã giao hàng)
- `5` - Cancelled (Đã hủy)

### 6. 📋 **OrderItems** (Chi Tiết Đơn Hàng)
*Bảng quản lý chi tiết sản phẩm trong đơn hàng*

| Cột | Kiểu Dữ Liệu | Ràng Buộc | Mô Tả |
|-----|--------------|-----------|-------|
| `Id` | `int` | Primary Key, Identity | ID duy nhất |
| `OrderId` | `int` | Foreign Key → Orders.Id | ID đơn hàng |
| `ProductId` | `int` | Foreign Key → Products.Id | ID sản phẩm |
| `Quantity` | `int` | Required, Range(1, MaxValue) | Số lượng |
| `UnitPrice` | `decimal(18,2)` | Required | Giá đơn vị tại thời điểm đặt hàng |

**Computed Properties:**
- `TotalPrice` = `UnitPrice * Quantity`

---

## 🔗 Mối Quan Hệ Giữa Các Bảng

### **One-to-Many Relationships:**
- `Categories` → `Products` (1 danh mục có nhiều sản phẩm)
- `Products` → `CartItems` (1 sản phẩm có trong nhiều giỏ hàng)
- `Products` → `OrderItems` (1 sản phẩm có trong nhiều đơn hàng)
- `AspNetUsers` → `CartItems` (1 user có nhiều item trong giỏ)
- `AspNetUsers` → `Orders` (1 user có nhiều đơn hàng)
- `Orders` → `OrderItems` (1 đơn hàng có nhiều chi tiết)

### **Foreign Key Constraints:**
- `Products.CategoryId` → `Categories.Id` (Restrict on delete)
- `CartItems.ProductId` → `Products.Id` (Cascade on delete)
- `OrderItems.OrderId` → `Orders.Id` (Cascade on delete)
- `OrderItems.ProductId` → `Products.Id` (Restrict on delete)

---

## 📊 Thống Kê Database

| Bảng | Số Cột | Số Index | Số Foreign Key |
|------|--------|----------|----------------|
| AspNetUsers | 12 | 3 | 0 |
| Categories | 3 | 1 | 0 |
| Products | 8 | 2 | 1 |
| CartItems | 5 | 2 | 2 |
| Orders | 9 | 2 | 1 |
| OrderItems | 5 | 2 | 2 |

---

## 🛠️ Các Tính Năng Database

### **Data Validation:**
- Email format validation
- Phone number format validation
- String length constraints
- Range validation cho số lượng và giá
- Required field validation

### **Data Integrity:**
- Foreign key constraints
- Cascade delete cho CartItems và OrderItems
- Restrict delete cho Products (bảo vệ dữ liệu quan trọng)

### **Performance:**
- Primary key indexes
- Foreign key indexes
- Unique constraints cho Email và UserName

---

## 📝 Ghi Chú Quan Trọng

1. **Bảo mật:** Mật khẩu được hash bằng ASP.NET Identity
2. **Audit:** Có trường CreatedAt để tracking thời gian tạo
3. **Soft Delete:** Có thể thêm trường IsDeleted cho soft delete
4. **Scaling:** Database được thiết kế để dễ dàng scale và thêm tính năng
5. **Backup:** Nên backup định kỳ file SQLite (.db)

---

*Cập nhật lần cuối: 09/19/2025*  
*Phiên bản: 1.0*  
*Tác giả: System Administrator*