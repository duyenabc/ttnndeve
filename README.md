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

## Deploy Render

Repo có `render.yaml` (PostgreSQL + API Docker + Static Vue).

### Cách nhanh trên Dashboard

1. [Render Dashboard](https://dashboard.render.com) → **New** → **Blueprint** → chọn repo `duyenabc/ttnndeve` → Apply `render.yaml`.
2. Đợi **ims-db**, **ims-api**, **ttnndeve** tạo xong.
3. Mở **ims-api** → copy URL (ví dụ `https://ims-api.onrender.com`).
4. Mở **ttnndeve** (Static) → Environment:
   - `VITE_API_BASE_URL` = `https://<ims-api-url>/api` (không dấu `/` cuối thừa)
5. Mở **ims-api** → Environment:
   - `Cors__Origins` = `https://<ttnndeve-url>` (URL frontend, không `/` cuối)
6. **Manual Deploy** lại cả API và Static (Vite chỉ đọc `VITE_*` lúc build).

### Checklist bạn cần chỉnh

| Mục | Ở đâu | Giá trị |
|-----|--------|---------|
| `VITE_API_BASE_URL` | Static site env | `https://ims-api-xxxx.onrender.com/api` |
| `Cors__Origins` | API env | `https://ttnndeve.onrender.com` (URL frontend thật) |
| `DATABASE_URL` | API (từ Blueprint) | Tự gắn từ Postgres — không sửa tay trừ khi lỗi SSL |
| Repo access | Render ↔ GitHub | Render có quyền đọc repo `ttnndeve` |

Free tier: API sleep sau idle; lần mở đầu có thể chậm ~30–60s.
