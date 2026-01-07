# Example API - Multi-Language Implementation

這是一個展示如何用不同程式語言實作相同 API 規格的專案，涵蓋 C# ASP.NET Core、Node.js Express 和 Python FastAPI 三種實作。

## 📁 Repository 結構

```
example-api/
├── src/
│   ├── csharp/                     # C# ASP.NET Core 實作
│   ├── nodejs/                     # Node.js + Express + TypeScript 實作
│   └── python/                     # Python + FastAPI 實作
├── docker/
│   ├── pg-docker-compose.yml       # PostgreSQL 資料庫 compose 檔
│   ├── api-docker-compose.yml      # API 服務 compose 檔
│   ├── dockerfile                  # API Docker 映像檔
│   └── dockerfile.alpine           # Alpine 版本 Docker 映像檔
├── db/                             # PostgreSQL 資料目錄（本機開發用）
├── .env                            # 環境變數設定
├── .env.example                    # 環境變數範例
├── api.http                        # REST Client 測試檔案
├── run-csharp.sh                   # 啟動 C# API 腳本
├── run-nodejs.sh                   # 啟動 Node.js API 腳本
└── stop-csharp.sh                  # 停止 C# API 腳本
```

---

## 🏗️ 專案結構說明

### C# ASP.NET Core 實作

```
src/csharp/
├── jubo-example-api.sln           # Solution 檔案
└── api/
    ├── api.csproj                 # 專案檔
    ├── Program.cs                 # 應用程式進入點
    ├── Controllers/               # API 控制器
    ├── Services/                  # 業務邏輯層
    ├── Repositories/              # 資料存取層
    │   └── Caches/                # Redis 快取裝飾器
    ├── Models/                    # 資料模型 (Entities)
    ├── Dtos/                      # 資料傳輸物件
    │   ├── Requests/              # 請求 DTO
    │   └── Responses/             # 回應 DTO
    ├── Mappers/                   # Entity ↔ DTO 映射
    ├── Data/                      # EF Core DbContext
    ├── Migrations/                # EF Core 資料庫遷移檔
    ├── Infrastructure/            # 基礎設施 (UnitOfWork Pattern & DbSession abstraction)
    ├── DateTimeOffsetProviders/   # 時間提供者
    ├── Enums/                     # 列舉定義
    └── Validators/                # 資料驗證器
```

### Node.js Express 實作

```
src/nodejs/
├── package.json                   # npm 套件設定
├── tsconfig.json                  # TypeScript 設定
├── eslint.config.js               # ESLint 設定
└── src/
    ├── server.ts                  # 應用程式進入點
    ├── routes/                    # API 路由
    ├── services/                  # 業務邏輯層
    ├── repositories/              # 資料存取層
    │   └── caches/                # Redis 快取裝飾器
    ├── entities/                  # 資料模型 (Entities)
    ├── dtos/                      # 資料傳輸物件
    │   ├── requests/              # 請求 DTO
    │   └── responses/             # 回應 DTO
    ├── database/                  # 資料庫連線設定
    ├── cache/                     # Redis 快取設定
    ├── validators/                # 資料驗證器
    ├── middlewares/               # 中介軟體
    └── utils/                     # 工具函式
```

### Python FastAPI 實作

```
src/python/
├── requirements.txt               # pip 套件清單
├── pyproject.toml                 # Python 專案設定
├── main.py                        # 應用程式進入點
└── app/
    ├── configs/                   # 設定檔
    │   ├── database_config.py     # 資料庫設定
    │   └── cache_config.py        # Redis 快取設定
    ├── infrastructure/            # 基礎設施層
    │   ├── database.py            # 資料庫連線
    │   └── redis_client.py        # Redis 客戶端
    ├── routers/                   # API 路由
    ├── services/                  # 業務邏輯層
    ├── repositories/              # 資料存取層
    │   └── caches/                # Redis 快取裝飾器
    ├── entities/                  # 資料模型 (Entities)
    ├── schemas/                   # Pydantic Schema
    │   ├── requests/              # 請求 Schema
    │   ├── responses/             # 回應 Schema
    │   └── dtos/                  # DTO Schema
    └── validators/                # 資料驗證器
```

---

## 🎯 API 功能

### Patient Management (病患管理)
- `GET /api/patients` - 查詢病患列表（支援日期範圍、分頁）
- `GET /api/patients/{id}` - 查詢單一病患
- `POST /api/patients` - 新增病患

### Order Management (訂單管理)
- `GET /api/orders/{id}` - 查詢單一訂單
- `POST /api/orders` - 新增訂單
- `PUT /api/orders/{id}` - 更新訂單訊息

---

## 🐳 Docker Compose 說明

### `pg-docker-compose.yml` - PostgreSQL 資料庫
啟動 PostgreSQL 15 資料庫服務，用於所有三種語言實作的資料儲存。

```bash
# 啟動資料庫
docker-compose --env-file .env -f ./docker/pg-docker-compose.yml up -d

# 停止資料庫
docker-compose -f ./docker/pg-docker-compose.yml down
```

### `api-docker-compose.yml` - API 服務
用於容器化部署 API 服務。

---

## 📜 Shell Scripts 說明

### `run-csharp.sh`
啟動 C# ASP.NET Core API 服務。
- 自動啟動 PostgreSQL 資料庫（如果尚未啟動）
- 啟動 Redis 服務
- 執行 dotnet run
- API 預設運行於 `http://localhost:5000`
- Swagger UI: `http://localhost:5000/swagger`

### `run-nodejs.sh`
啟動 Node.js Express API 服務。
- 自動啟動 PostgreSQL 資料庫（如果尚未啟動）
- 啟動 Redis 服務
- 執行 npm run dev
- API 預設運行於 `http://localhost:5001`
- API Docs: `http://localhost:5001/api-docs`

### `stop-csharp.sh`
停止 C# ASP.NET Core API 服務。

---

## 🚀 快速啟動

### C# ASP.NET Core 版本

```bash
# 啟動 API
./run-csharp.sh

# 瀏覽 Swagger 文件
open http://localhost:5000/swagger
```

詳細說明請參考：[src/csharp/README.md](src/csharp/README.md)

---

### Node.js Express 版本

```bash
# 安裝依賴
cd src/nodejs && npm install

# 啟動 API
./run-nodejs.sh

# 瀏覽 API 文件
open http://localhost:5001/api-docs
```

詳細說明請參考：[src/nodejs/README.md](src/nodejs/README.md)

---

### Python FastAPI 版本

```bash
# 建立虛擬環境
cd src/python && python -m venv venv

# 啟動虛擬環境
source venv/bin/activate  # macOS/Linux
# or
venv\Scripts\activate     # Windows

# 安裝依賴
pip install -r requirements.txt

# 啟動 API
uvicorn main:app --reload --host 0.0.0.0 --port 5002

# 瀏覽 API 文件
open http://localhost:5002/swagger
```

詳細說明請參考：[src/python/README.md](src/python/README.md)

---

## 🗄️ 資料庫管理

### Database Migration

**重要說明：** 目前資料庫遷移（Database Migration）統一由 **C# ASP.NET Core 的 EF Core** 管理。

Node.js 和 Python 實作目前**不支援**獨立的 database migration，所有 schema 變更必須透過 C# 專案的 `dotnet ef` CLI 工具處理。

#### 執行 Migration (C# 專案)

```bash
cd src/csharp/api

# 新增 Migration
dotnet ef migrations add MigrationName

# 更新資料庫
dotnet ef database update

# 回滾 Migration
dotnet ef database update PreviousMigrationName
```

### 資料庫 Schema

- `patient` - 病患資料表
  - `id` (bigint, PK)
  - `name` (varchar)
  - `created_at` (timestamptz)
  - `updated_at` (timestamptz)

- `order` - 訂單資料表
  - `id` (bigint, PK)
  - `patient_id` (bigint, FK → patient.id)
  - `message` (text)
  - `created_at` (timestamptz)
  - `updated_at` (timestamptz)

---

## 🔧 技術棧比較

| 功能      | C# ASP.NET Core                                       | Node.js Express              | Python FastAPI             |
| --------- | ----------------------------------------------------- | ---------------------------- | -------------------------- |
| Web 框架  | ASP.NET Core 10                                       | Express 5 + TypeScript       | FastAPI 0.115              |
| ORM       | Entity Framework Core + Dapper                        | TypeORM                      | SQLAlchemy 2.0 (Async)     |
| 資料驗證  | IValidatableObject (ASP.NET Core 內建)                | class-validator              | Pydantic                   |
| 快取      | Redis (IDistributedCache)                             | Redis (ioredis)              | Redis (redis.asyncio)      |
| API 文件  | Swagger (Swashbuckle)                                 | Swagger (swagger-ui-express) | OpenAPI (FastAPI built-in) |
| 日誌      | Serilog + ILogger (Microsoft.Extensions.Logging)      | Winston                      | Python logging             |
| 彈性機制  | Polly (Retry, Circuit Breaker)                        | -                            | -                          |
| Migration | EF Core Migrations                                    | ❌ 不支援                     | ❌ 不支援                   |

---

## 🧪 測試 API

專案根目錄包含 `api.http` 檔案，可使用 REST Client (VS Code Extension) 或類似工具進行 API 測試。

```http
### Get Patients
GET http://localhost:5000/api/patients?startTime=2020-01-01T00:00:00Z&endTime=2026-12-31T23:59:59Z&pageNumber=1&pageSize=10

### Create Patient
POST http://localhost:5000/api/patients
Content-Type: application/json

{
  "name": "John Doe"
}
```

---

## 📦 環境變數設定

複製 `.env.example` 為 `.env` 並設定以下變數：

```bash
# PostgreSQL
POSTGRES_USER=your_user
POSTGRES_PASSWORD=your_password

# Redis
REDIS_HOST=localhost
REDIS_PORT=6379
REDIS_DB=0
REDIS_PASSWORD=your_redis_password

# Node.js
NODE_EXPRESS_PORT=5001

# Cache
CACHE_SLIDING_EXPIRATION_MINUTES=10
CACHE_ABSOLUTE_EXPIRATION_MINUTES=60

# Logging
LOG_LEVEL=debug
LOG_REQUEST_BODY=true
LOG_RESPONSE_BODY=true
```

---

## 🏛️ 架構特色

### 共同架構模式
- **Repository Pattern** - 資料存取抽象化
- **Service Layer** - 業務邏輯封裝
- **DTO Pattern** - 資料傳輸物件分離
- **Dependency Injection** - 依賴注入
- **Decorator Pattern** - Redis 快取裝飾器
- **Unit of Work Pattern** - 交易管理 (C# 實作)

### Redis 快取策略
- **Cache-Aside Pattern** - 查詢時先檢查快取
- **Write-Through** - 更新時同步清除快取
- **Decorator Pattern** - 透過裝飾器添加快取功能，不影響原有 Repository

---

## 📝 開發備註

- 這是一個學習/比較用途的專案
- 三個版本實作相同的 API 規格和架構模式
- 可用於對照不同語言/框架的實作方式
- 資料庫 Migration 統一由 C# EF Core 管理
- 所有實作共用同一個 PostgreSQL 資料庫和 Redis 服務

---

## 🤝 貢獻

歡迎提出 Issue 或 Pull Request！

## 📄 授權

MIT License
