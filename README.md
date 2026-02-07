# Example API - Multi-Language Implementation

- 這是一個展示如何用不同程式語言實作相同 API 規格的專案，涵蓋 C# ASP.NET Core、Node.js Express 和 Python FastAPI 三種實作。
- 希望可以提供一種基本簡易的程式架構範例，讓開發者能夠參考不同語言的實作方式與架構設計，而不是都使用「義大利麵式」一條龍的寫法寫出可閱讀性低、不語意化、不可維護、不可測試的 Web API 專案。
- 三種程式語言的實作目前都採用 3-Layer 的程式架構，暫時不採用 DDD 或 CQRS 的架構，目的是希望 Keep it simple, keep it stupid. (KISS)。
- 建議開發人員寫出 intention-level 的程式碼，而不是 code-level 的「代碼」。
  - C# 是高度現代化的高階程式語言，**讓「意圖」(What) 清晰才是重點，而不是「怎麼實作」(How)**。
  - 只有語意化的程式碼才是高品質、高質量、高可閱讀性、高可維護性、高可擴充性、高可測試性的程式碼，即便要應付高併發與大流量的程式碼，仍應該要「語意化」。
  - 如果你的程式碼與程式架構不能讓只有 1 年工作經驗的人看得懂，代表你的程式碼品質就是 code-level 的代碼。
  - 如果你的程式碼停留在 code-level 代碼階段，基本上你也不用去考慮 DDD、CQRS 或者 Clean Architecture 了，**因為你完全不了解什麼叫做「程式設計」、「軟體開發工程」、「協作開發」的重要性和最基礎的 OOP (物件導向)**。
  - 要寫出能動的「代碼」此專案就沒有任何參考價值。
- 本專案不包含任何前端相關技術，單純的示範後端 API 的實作方式，非常適合應用於前後端分離並且採用容器化 Microservices 架構的專案。

## 📁 Git Repository 結構

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
├── jjyy-example-api.sln           # Solution 檔案
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
- `GET /api/patients?` - 查詢病患列表（支援日期範圍與分頁）
- `GET /api/patients/{id}` - 查詢單一的病患
- `GET /api/patients/{id}/order-histories` - 查詢病患的訂單歷史紀錄
- `POST /api/patients` - 新增病患
- `PUT /api/patients/{id}` - 更新病患基本資料

### Patient Order Management (病患訂單管理)
- `GET /api/orders?` - 查詢病患訂單列表（支援分頁與篩選）
- `GET /api/orders/{id}` - 查詢單一病患訂單
- `GET /api/orders/{id}/histories` - 查詢病患的訂單歷史紀錄
- `POST /api/orders` - 新增病患訂單
- `POST /api/orders/{id}/dispense` - 對病患訂單執行配藥動作
- `POST /api/orders/{id}/execute` - 對病患訂單執行治療動作
- `POST /api/orders/{id}/cancel` - 對病患訂單執行取消動作

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
- API Docs: `http://localhost:5001/swagger`

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
open http://localhost:5001/swagger
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

- 本範例專案使用 PostgreSQL 作為 RDBMS 資料庫，以及 Redis 當作快取層。
- 建議可以使用 docker-compose 來快速啟動 PostgreSQL 和 Redis 服務，在 localhost 上進行開發與測試。
  - [PostgreSQL docker-compose.yml 範例](https://gitlab.com/ponylin1985/MyDockerContainers/-/blob/master/postgresql/docker-compose-15.3.yml?ref_type=heads)
  - [Redis docker-compose.yml 範例](https://gitlab.com/ponylin1985/MyDockerContainers/-/blob/master/redis/docker-compose-7.2.2-alpine3.18.yml?ref_type=heads)

### Database Migration

**重要說明：** 目前資料庫遷移（Database Migration）統一由 **C# ASP.NET Core 的 EF Core** 管理。

Node.js 和 Python 實作目前**不支援**獨立的 database migration，所有 schema 變更必須透過 C# 專案的 `dotnet ef` CLI 工具處理。<br/>
因此，請確保在進行任何資料庫 schema 變更時，皆在 C# 專案中執行 migration。

- 必要安裝軟體：
  - .NET SDK 10。
  - dotnet ef CLI 工具：`dotnet tool install --global dotnet-ef`

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


### 資料庫 Table Schema

```mermaid
erDiagram
    patient ||--o{ patient_order : "places"
    patient_order ||--o{ prescription : "contains"
    medication ||--o{ prescription : "is prescribed in"

    patient {
        bigint id PK
        varchar_50 name
        int age
        smallint gender
        varchar_100 email
        varchar_10 phone_number
        varchar_25 country
        varchar_25 city
        varchar_25 area
        varchar_25 road
        varchar_25 street
        varchar_100 address_others
        date date_of_birth
        timestamptz first_visit_date
        smallint status
        varchar_500 remarks
        timestamptz created_at
        varchar_50 created_by
        timestamptz updated_at
        varchar_50 updated_by
    }

    patient_order {
        bigint id PK
        bigint patient_id FK
        varchar_500 instructions
        timestamptz next_visit_date
        timestamptz start_date
        timestamptz end_date
        smallint type
        smallint status
        timestamptz created_at
        varchar_50 created_by
        timestamptz updated_at
        varchar_50 updated_by
    }

    medication {
        bigint id PK
        varchar_50 name
        varchar_50 manufacturer
        timestamptz created_at
        varchar_50 created_by
        timestamptz updated_at
        varchar_50 updated_by
    }

    prescription {
        bigint id PK
        bigint order_id FK
        bigint medication_id FK
        varchar_50 dose
        varchar_50 frequency
        int duration_in_days
        smallint route
        timestamptz created_at
        varchar_50 created_by
        timestamptz updated_at
        varchar_50 updated_by
    }
```

- `patient` - 病患基本資料表
  - `id` (bigint, PK)
  - `name` (varchar(50), NOT NULL)
  - `age` (int, NOT NULL)
  - `gender` (smallint, NOT NULL)
  - `email` (varchar(100), NULL)
  - `phone_number` (varchar(10), NOT NULL)
  - `country` (varchar(25), NULL)
  - `city` (varchar(25), NULL)
  - `area` (varchar(25), NULL)
  - `road` (varchar(25), NULL)
  - `street` (varchar(25), NULL)
  - `address_others` (varchar(100), NULL)
  - `date_of_birth` (date, NOT NULL)
  - `first_visit_date` (timestamptz, NOT NULL)
  - `status` (smallint, NOT NULL)
  - `remarks` (varchar(500), NULL)
  - `created_at` (timestamptz, NOT NULL, DEFAULT NOW())
  - `created_by` (varchar(50), NOT NULL)
  - `updated_at` (timestamptz, NOT NULL, DEFAULT NOW())
  - `updated_by` (varchar(50), NOT NULL)

- `patient_order` - 病患訂單資料表
  - `id` (bigint, PK)
  - `patient_id` (bigint, `FK → patient.id`, NOT NULL)
  - `instructions` (varchar(500), NULL)
  - `next_visit_date` (timestamptz, NULL)
  - `start_date` (timestamptz, NULL)
  - `end_date` (timestamptz, NULL)
  - `type` (smallint, NOT NULL)
  - `status` (smallint, NOT NULL)
  - `created_at` (timestamptz, NOT NULL, DEFAULT NOW())
  - `created_by` (varchar(50), NOT NULL)
  - `updated_at` (timestamptz, NOT NULL, DEFAULT NOW())
  - `updated_by` (varchar(50), NOT NULL)

- `medication` - 藥品資料表
  - `id` (bigint, PK)
  - `name` (varchar(50), NOT NULL)
  - `manufacturer` (varchar(50), NOT NULL)
  - `created_at` (timestamptz, NOT NULL, DEFAULT NOW())
  - `created_by` (varchar(50), NOT NULL)
  - `updated_at` (timestamptz, NOT NULL, DEFAULT NOW())
  - `updated_by` (varchar(50), NOT NULL)

- `prescription` - 處方資料表
  - `id` (bigint, PK)
  - `order_id` (bigint, `FK → patient_order.id`, NOT NULL)
  - `medication_id` (bigint, `FK → medication.id`, NOT NULL)
  - `dose` (varchar(50), NULL)
  - `frequency` (varchar(50), NULL)
  - `duration_in_days` (int, NOT NULL)
  - `route` (smallint, NOT NULL)
  - `created_at` (timestamptz, NOT NULL, DEFAULT NOW())
  - `created_by` (varchar(50), NOT NULL)
  - `updated_at` (timestamptz, NOT NULL, DEFAULT NOW())
  - `updated_by` (varchar(50), NOT NULL)

---

## 🔧 技術棧比較

| 功能      | C# ASP.NET Core                                                                              | Node.js Express              | Python FastAPI             |
| --------- | -------------------------------------------------------------------------------------------- | ---------------------------- | -------------------------- |
| Web 框架  | ASP.NET Core 10                                                                              | Express 5 + TypeScript       | FastAPI 0.115              |
| ORM       | Entity Framework Core + Dapper                                                               | TypeORM                      | SQLAlchemy 2.0 (Async)     |
| 資料驗證  | IValidatableObject (ASP.NET Core 內建)                                                       | class-validator              | Pydantic                   |
| 快取      | StackExchange.Redis (IDistributedCache)<br/>CacheOutput (Microsoft.AspNetCore.OutputCaching) | Redis (ioredis)              | Redis (redis.asyncio)      |
| API 文件  | Swagger (Swashbuckle)                                                                        | Swagger (swagger-ui-express) | OpenAPI (FastAPI built-in) |
| 日誌      | Serilog + ILogger (Microsoft.Extensions.Logging)                                             | Winston                      | Python logging             |
| 彈性機制  | Polly (Retry, Circuit Breaker)                                                               | 自行實作 (Retry)             | -                          |
| Migration | EF Core Migrations                                                                           | ❌ 不支援                     | ❌ 不支援                   |

### Node.js 彈性機制實作範例

#### Retry Pattern (重試機制)

```typescript
// utils/retry.ts
export async function withRetry<T>(
  fn: () => Promise<T>,
  maxRetries: number = 3,
  delayMs: number = 1000
): Promise<T> {
  let lastError: Error;

  for (let attempt = 1; attempt <= maxRetries; attempt++) {
    try {
      return await fn();
    } catch (error) {
      lastError = error as Error;
      if (attempt < maxRetries) {
        await new Promise(resolve => setTimeout(resolve, delayMs * attempt));
      }
    }
  }

  throw lastError!;
}

// 使用範例
const result = await withRetry(
  () => externalApiCall(),
  3,  // 最多重試 3 次
  1000 // 延遲 1 秒
);
```

#### Circuit Breaker Pattern (斷路器機制)

```typescript
// utils/circuitBreaker.ts
export class CircuitBreaker {
  private failureCount = 0;
  private lastFailureTime?: number;
  private state: 'CLOSED' | 'OPEN' | 'HALF_OPEN' = 'CLOSED';

  constructor(
    private failureThreshold: number = 5,
    private resetTimeoutMs: number = 60000
  ) {}

  async execute<T>(fn: () => Promise<T>): Promise<T> {
    if (this.state === 'OPEN') {
      if (Date.now() - this.lastFailureTime! > this.resetTimeoutMs) {
        this.state = 'HALF_OPEN';
      } else {
        throw new Error('Circuit breaker is OPEN');
      }
    }

    try {
      const result = await fn();
      this.onSuccess();
      return result;
    } catch (error) {
      this.onFailure();
      throw error;
    }
  }

  private onSuccess() {
    this.failureCount = 0;
    this.state = 'CLOSED';
  }

  private onFailure() {
    this.failureCount++;
    this.lastFailureTime = Date.now();
    if (this.failureCount >= this.failureThreshold) {
      this.state = 'OPEN';
    }
  }
}

// 使用範例
const breaker = new CircuitBreaker(5, 60000);
const result = await breaker.execute(() => externalApiCall());
```

---

## 🧪 測試 API

- 專案根目錄包含 `api.http` 檔案，可使用 REST Client (VS Code Extension) 或類似工具進行 API 測試。
- 弱者使用各個語言實作的出來的 `GET /swagger` 頁面進行測試。

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

### 分層架構流程

- 主要為 3-Layer 架構 (Presentation Layer/Business Logic Layer/Data Access Layer)，透過 ASP.NET Core 內建的 Output Caching 和 Redis 快取裝飾器 (Decorator Pattern) 以提升效能。
- 目前 Entity 主要為貧血模型 (Anemic Model)，業務邏輯集中在 Service 層，適合中小型業務邏輯不複雜的專案，未來可能再提供以 DDD 架構的範例。
- 整體流程如下：

```
Router/ApiEndpoint/Controller
    ↓
Http Output Cache Middleware
    ↓
Service (Business Logic)
    ↓
Cached Repository (Decorator)
    ↓
Repository (Data Access)
    ↓
Database
```

- 下圖為整體架構流程圖：

```mermaid
flowchart TD
    Client["API Client"]

    %% Middlewares
    M1["GlobalExceptionHandlerMiddleware"]
    M2["ResponseCompressionMiddleware"]
    M3["TraceIdMiddleware"]
    M4["OutputCacheMiddleware"]
    M5["SlowRequestLoggingMiddleware"]
    M6["RequestResponseLoggingMiddleware"]

    Endpoint["Minimal API Endpoint<br/>(ApiEndpoints)"]

    %% Business Logic Layer
    subgraph Business_Logic_Layer["Business Logic Layer"]
        subgraph Application_Services["Application Services"]
            S1["PatientService"]
            S2["PatientOrderService"]
        end

        subgraph Processes["Business&nbsp;Processes&nbsp;(BDD&nbsp;Style)"]
            direction LR
            P1["AddPatientOrderProcess"]
            P2["PatchPatientOrderProcess"]
        end
        
        subgraph Domain_Services["Domain Services"]
            DS1["OrderPrescriptionPolicy"]
        end
    end

    %% Data Access Layer
    subgraph Data_Access_Layer["Data Access Layer"]
        RedisCache["Cached Repository<br/>(Scrutor Decorator)"]
        Repository["Repository<br/>(EF Core / Dapper)"]
        DB[/"Database (PostgreSQL)"/]
    end

    %% HTTP Pipeline & API Flow
    Client --> M1 --> M2 --> M3 --> M4
    M4 -- "Cache Hit" --> Client
    M4 --> M5 --> M6 --> Endpoint
    
    Endpoint --> S1
    Endpoint --> S2
    S2 --> P1
    S2 --> P2

    %% Simplified Dependencies as requested
    Application_Services ----> Domain_Services
    
    %% Representational connection to Data Access Layer
    Application_Services ==> Data_Access_Layer
    Processes ==> Data_Access_Layer
    Domain_Services ==> Data_Access_Layer

    RedisCache --> Repository --> DB

    %% Styles
    style Processes fill:#1a3a3a,stroke:#00ffcc,stroke-width:2px
    style Domain_Services fill:#2d2d2d,stroke:#f9f,stroke-width:2px,stroke-dasharray: 5 5
```

### 目錄結構範例 (以 C# ASP.NET Core Minimal API Project 為例)

```
ApiEndpoints/                         ← Minimal APIs 端點
├── PatientApiEndpoints
└── OrderApiEndpoints

Services/                             ← 業務邏輯 (目前主要為應用程式服務 Application Service)
├── DomainServices/                   ← 領域服務 (業務領域服務 Domain Services)
|   └── OrderPrescriptionPolicy
├── PatientService
└── OrderService

Processes/                            ← 業務流程封裝 (Method Object Pattern)  
├── Patient/
    └── AddPatientProcess
└── PatientOrder/
    ├── AddPatientOrderProcess
    └── PatchPatientOrderProcess

Repositories/                         ← 資料倉儲抽象化 (Repository Pattern)
├── PatientRepository
├── PatientOrderRepository
└── PatientOrderHistoryRepository

Mappers/                              ← 資料載體對應
├── PatientOrderMapper
└── PatientMapper

Middlewares/                          ← Http 管道中介層 (Http Pipeline Middlewares)
├── GlobalExceptionHandlerMiddleware
├── RequestResponseLoggingMiddleware
├── SlowRequestLoggingMiddleware
└── TraceIdMiddleware

Models/                               ← 貧血模型 (Entities/POCOs)
├── Patient
└── PatientOrder

Infrastructure/                       ← 基礎建設 (Infrastructure)
├── IDbSession
└── IUnitOfWork

Migrations/                           ← 資料庫遷移 (dotnet ef migrations)
├── XxxMigration.cs
├── YyyMigration.cs
└── DbContextModelSnapshot

Dtos/                                 ← 資料傳輸物件 (Data Transfer Objects)
├── PatientDto
├── PatientOrderDto
└── Requests                          ← 請求資料載體 (Request DTOs)
    ├── CreatePatientOrderRequest
    ├── CreatePatientRequest
    ├── GetPatientsRequest
    ├── UpdatePatientRequest
    ├── UpdatePatientOrderRequest
    └── PagedRequest
└── Responses                         ← 回應資料載體 (Response DTOs)
    ├── ApiResult
    └── PagedResult

Options/                              ← 應用程式設定組態 (IOptions Pattern)
└── RequestResponseLoggingOptions

Validators/                           ← 請求資料驗證器 (Request Validators - FluentValidation)
├── CreatePatientRequestValidator
└── CreatePatientOrderRequestValidator
```

### Application Service & Domain Service ==> 意圖化

- Application Service
  - 角色定義：處理請求與回應的主流程 (Orchestrator)。
  - 職責定義：
    - 負責接收 API 請求，將 DTO 轉換為 Domain 需要的參數，並「編排」多個 Policy 與 Process 的執行順序。
    - 不處理具體業務，而是負責交易的控制、日誌與錯誤處理。
  - 依賴：
    - 允許依賴一個或多個 Repository 物件。
    - 允許依賴一個或多個 Process 物件。
    - 允許依賴一個或多個 Policy 物件。
    - 允許依賴 IO 或 Infrastructure 物件。
    - 禁止各個 Application Service 互相依賴。
 
- Process (Domain Service - Action)
  - 角色定義：一個業務流程的單元封裝，業務流程的執行者。
  - 職責定義：
    - 執行狀態改變或計算。
      - 允許更新 Entity 的狀態。
      - 允許將 Entity 的狀態透過 IO 持久化。
    - 應設計為原子操作。
  - 依賴：
    - 允許依賴一個或多個 Repository 物件。
    - 允許依賴一個或多個 Policy 物件驗證規則或取得計算結果。
    - 禁止各個 Process 互相依賴。
    - 禁止反向依賴 Application Service 物件。
   
- Policy (Domain Service - Spec)
  - 角色定義：一個業務邏輯判斷或規則。
  - 職責定義：
    - 獨立的業務規格，純業務邏輯判斷或計算，不應該涉及任何 IO 操作。
    - 等冪性：相同的輸入保證相同的輸出。
    - 無狀態：
      - 內部應該是無狀態 (Stateless)。
      - 嚴格**禁止更新 Entity 參數的狀態** --> 參數應該保持 **「唯獨」** 特性，無法被 Policy 改變。
  - 依賴：
    - 禁止各個 Policy 互相依賴。
    - 禁止反向依賴 Application Service 或 Process 物件。
    - 禁止依賴 Repository 物件。
    - 禁止依賴任何 HttpClient、gRPCClient、RedisClient 或者任形式的 Data Source 客戶端程式碼。
  
```mermaid
flowchart TD
    %% 定義節點並強制設定文字為黑色
    Svc{{"<font color='#000'><b>Application Service</b></font><br/><font color='#000'>(The Orchestrator)</font>"}}
    
    subgraph DomainLogic [Domain Service Layer]
        Proc["<font color='#000'><b>Process</b></font><br/><font color='#000'>(Domain Service - Action)</font>"]
        Pol[["<font color='#000'><b>Policy</b></font><br/><font color='#000'>(Domain Service - Spec)</font>"]]
    end

    %% 調度與依賴關係
    Svc -->|Orchestrate| Pol
    Svc -->|Orchestrate| Proc
    Proc -.->|Reference| Pol

    %% 職責描述並強制設定文字為黑色
    Note1["<font color='#000'>請求回應的主流程順序<br/>管理交易<br/>決定如何調度 Policy 或 Process 物件</font>"] -.-> Svc
    Note2["<font color='#000'>業務流程封裝<br/>一個業務流程單元<br/>執行原子化操作</font>"] -.-> Proc
    Note3["<font color='#000'>業務邏輯封裝<br/>純業務邏輯規則或檢查</font>"] -.-> Pol

    %% 樣式設定 (回復原本配色，並確保邊框清晰)
    style Svc fill:#f9f,stroke:#333,stroke-width:2px
    style Pol fill:#bbf,stroke:#333,stroke-width:2px,stroke-dasharray: 5 5
    style Proc fill:#bfb,stroke:#333,stroke-width:2px
    
    %% 調整註釋框背景為白色以確保文字清晰
    style Note1 fill:#fff,stroke:#ccc
    style Note2 fill:#fff,stroke:#ccc
    style Note3 fill:#fff,stroke:#ccc
```

### 共同架構模式
- **Repository Pattern** - 資料倉儲抽象化
- **Unit of Work Pattern & IDbSession** - 交易管理與資料存取抽象化，並且透過 Polly 實現 Retry & Circuit Breaker (only for C# ASP.NET Core)
- **Service Layer** - 業務邏輯封裝
- **DTO Pattern** - 資料傳輸物件分離
- **Dependency Injection** - 依賴注入
- **Decorator Pattern** - Redis 快取裝飾器

### 快取策略
- **Cache-Aside Pattern** - 查詢時先檢查快取 (Http Output Caching + Redis Cache)
- **Write-Through** - 更新時同步清除快取 (Http Output Caching + Redis Cache)
- **Decorator Pattern** - 透過裝飾器添加 Redis 快取功能，不影響原有 Repository

### Make Your Production Code Like BDD Style

- BDD (Behavior-Driven Development) 強調以行為為導向的開發方式，讓程式碼更具可讀性和可維護性。
  - Given (前置條件)
  - When (行為/動作)
  - Then (結果/期望)
- 最重要的兩點: 
  - **讓程式碼的「意圖」(What - Happy Path) 清晰，而不是「怎麼實作」(How)**。
  - 不要只有在寫 Unit Test 時才使用 BDD 風格，**在撰寫生產程式碼時也應該採用 BDD 風格**，讓程式碼更具可讀性和可維護性。
- 範例: [src/csharp/api/Services/PatientService.cs AddPatientAsync 方法](src/csharp/api/Services/PatientService.cs)

```csharp
public async Task<ApiResult<PatientDto>> AddPatientAsync(CreatePatientRequest request)
{
    Patient? createdPatient = default;
    var patient = MapToEntity(request);

    // BDD Style: Given (Guard Clauses)
    await EnsureEmailUniqueAsync();
    await EnsurePhoneNumberUniqueAsync();
    await EnsurePrescriptionValidAsync();
    
    // BDD Style: When (Action)
    await WhenAddPatientAsync();

    // BDD Style: Then (Assertions)
    ShouldCreatedSuccessfully();
    return SuccessResult(createdPatient!.ToDto());
}
```

- 甚至用 Method Object 的手法繼續重構，最後變成 Fluent API 語意化的寫法，如下。
  - 現在 AddPatientAsync 方法中甚至連區域變數都不用了。
  - 仍然支援 async/await 呼叫。
  - 交易管理 (Unit of Work) 仍然在原始方法中決定。
  - 實際上 `AddPatientProcess` class 已經被上述手法重構成一個 BDD style 的 Domain Service，開始往 DDD 的設計邁進。

```csharp
public async Task<ApiResult<PatientDto>> AddPatientAsync(CreatePatientRequest request)
{
    var process = new AddPatientProcess(
        _loggerFactory.CreateLogger<AddPatientProcess>(),
        request,
        _patientRepository,
        _patientOrderHistoryRepository,
        _orderPrescriptionPolicy,
        _dateTimeOffsetProvider);

    await _unitOfWork.ExecuteStrategyAsync(async () =>
    {
        
        using var _ = await _unitOfWork.BeginTransactionAsync();
        await process
            .Prepare()                                              // Given
            .EnsureEmailUniqueAsync()                               // Given (Guard Clause)
            .ThenAsync(p => p.EnsurePhoneNumberUniqueAsync())       // Given (Guard Clause)
            .ThenAsync(p => p.EnsurePrescriptionValidAsync())       // Given (Guard Clause)
            .ThenAsync(p => p.ExecuteAsync(_unitOfWork))            // When (Action)
            .Then(p => p.ShouldSuccessfully());                     // Then (Assertions)
        await _unitOfWork.CommitTransactionAsync();
    });

    return SuccessResult(process.CreatedPatient!.ToDto());
}
```

- 或者至少類似以下的範例

```csharp
public async Task<ApiResult<PatientDto>> AddPatientAsync(CreatePatientRequest request)
{
    var patient = MapToEntity(request);
    
    // Guard Clauses
    if (!await IsEmailDuplicatedAsync())
    {
        _logger.LogWarning("Email {Email} is already in use.", patient.Email);
        return FailureResult<PatientDto>(ApiCode.OperationFailed, "Email is already in use.");
    }

    // Guard Clauses
    if (!await IsPhoneNumberDuplicatedAsync())
    {
        _logger.LogWarning("Phone number {PhoneNumber} is already in use.", patient.PhoneNumber);
        return FailureResult<PatientDto>(ApiCode.OperationFailed, "Phone number is already in use.");
    }

    // Guard Clauses
    if (!await IsPrescriptionValidAsync())
    {
        _logger.LogWarning("One or more prescriptions have invalid medication IDs.");
        return FailureResult<PatientDto>(
            ApiCode.OperationFailed, "One or more prescriptions have invalid medication IDs.");
    }

    // When (Action)
    var createdPatient = await _patientRepository.AddAsync(patient);
    await _unitOfWork.SaveChangesAsync();

    // Then (Assertions)
    if (!IsCreatedSuccessfully())
    {
        _logger.LogError("Failed to create patient: {Patient}", patient);
        return FailureResult<PatientDto>(ApiCode.OperationFailed, "Failed to create patient.");
    }

    return SuccessResult(createdPatient.ToDto());
}
```

- 現代的程式碼寫作風格繁多，OOP、Functional Programming、Fluent Style、BDD Style、DDD 等等，每種各有其優缺點，你可以自行挑選一種習慣的風格來撰寫。
- 但最重要的是撰寫出**邏輯正確、高效能、高可讀性易於讓「其他人」看得懂，維護得動的程式碼**，而不是只有你自己看得懂的程式碼。

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
