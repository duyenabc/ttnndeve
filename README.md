# IMS — Hệ thống Quản lý Thực tập

Stack: **Vue 3** (frontend) + **ASP.NET Core** (API) + **PostgreSQL** (database).

## Cấu trúc

```
├── src/                  # Vue 3 + Vite + Pinia + Tailwind
├── Backend/IMSBackend/   # ASP.NET Core Web API + EF Core (Npgsql)
└── .env.example          # VITE_API_BASE_URL
```

## Yêu cầu

- Node.js 22+
- .NET 10 SDK
- PostgreSQL 14+ (tạo database `ims_db`)

## Cấu hình PostgreSQL

Trong `Backend/IMSBackend/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=ims_db;Username=postgres;Password=postgres"
}
```

## Chạy local

### 1. API (.NET)

```bash
cd Backend/IMSBackend
dotnet restore
dotnet run
```

API mặc định: `http://localhost:5071`

### 2. Client (Vue)

```bash
npm install
cp .env.example .env   # Windows: copy .env.example .env
npm run dev
```

Client mặc định: `http://localhost:3000`

## Tài khoản seed (dev)

| Vai trò     | Mã định danh | Mật khẩu   |
|-------------|--------------|------------|
| Admin       | admin        | Admin@123  |
| Giảng viên  | GV001        | Gv@12345   |
| Sinh viên   | SV001        | Sv@12345   |

## Ghi chú kiến trúc

- Frontend gọi REST API qua Axios (`VITE_API_BASE_URL`).
- Thông báo dùng API `/api/notifications` (poll), không dùng Firebase.
- JWT Bearer cho các endpoint `[Authorize]`.
