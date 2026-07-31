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

Repo có `render.yaml` (PostgreSQL + API Docker + Static Vue). Cần đủ **3** service: `ims-db`, `ims-backend`, `ttnndeve`.

**Lỗi đăng nhập hiện tại:** `https://ims-api.onrender.com` đang là **Express** (`Home Page` / `Cannot POST /api/auth/login`), không phải API .NET. Frontend gọi URL đó nên login luôn fail. Dùng service Docker **`ims-backend`** (trong Blueprint mới); xóa hoặc bỏ qua `ims-api` Express.

### Cách nhanh: Blueprint (khuyên dùng)

1. [Render Dashboard](https://dashboard.render.com) → **New** → **Blueprint** → repo `duyenabc/ttnndeve` → Apply.
2. Đợi 3 service xanh: **ims-db**, **ims-backend**, **ttnndeve**.
3. Mở **ims-backend** → copy URL thật (thường `https://ims-backend.onrender.com` hoặc `…-xxxx.onrender.com`).  
   Mở URL đó: phải thấy JSON `{ "service": "IMS API", "status": "ok" }` (không phải `"Home Page"`, không `X-Powered-By: Express`).
4. **ttnndeve** → Environment → `VITE_API_BASE_URL` = `https://<ims-backend-url>/api` → **Manual Deploy**.
5. **ims-backend** → Environment → `Cors__Origins` = `https://<ttnndeve-url>` (không `/` cuối) → restart/redeploy.
6. API và DB phải cùng region (**Oregon**). `DATABASE_URL` = Internal URL của `ims-db` (Blueprint tự gắn).

### Tránh lỗi `yarn start` / `Command "start" not found`

Frontend **không** phải Web Service Node mặc định. Nếu tạo tay:

| Service | Loại đúng | Build | Start / Publish |
|---------|-----------|-------|-----------------|
| Frontend `ttnndeve` | **Static Site** | `npm ci && npm run build` | Publish dir: `dist` (không cần Start) |
| API `ims-backend` | **Web Service → Docker** | (Dockerfile) | không dùng `yarn start` / Express |

Nếu lỡ tạo frontend kiểu Web Service (Node): Build = `npm ci && npm run build`, Start = `npm start` (đã có script phục vụ `dist`).

### Docker (nếu tạo API Web Service tay)

| Mục | Giá trị |
|-----|---------|
| Dockerfile Path | `Backend/IMSBackend/Dockerfile` |
| Docker Context | `.` (gốc repo) |
| Root Directory | để trống |
| Region | Oregon (cùng `ims-db`) |

### Checklist

| Mục | Ở đâu | Giá trị |
|-----|--------|---------|
| `VITE_API_BASE_URL` | Static env | `https://ims-backend….onrender.com/api` |
| `Cors__Origins` | API env | URL frontend thật |
| `DATABASE_URL` | API env | Internal Database URL từ `ims-db` |
| Không Suspended | Overview | Cả 3 service Active |

Free tier: API sleep sau idle; lần mở đầu ~30–60s. Nếu Logs báo DB fail: kiểm tra Internal URL + cùng region.
