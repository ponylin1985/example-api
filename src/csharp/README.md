# ASP.NET Core API

這是使用 ASP.NET Core 10 開發的 API 服務。

## 技術棧

- **Framework**: ASP.NET Core 10 (Minimal API)
- **Database**: PostgreSQL + EF Core
- **Cache**: Redis
- **Logging**: Serilog
- **API Doc**: Swagger/OpenAPI

## 架構

- Minimal API Endpoints
- Repository Pattern
- Service Layer
- Decorator Pattern (Caching)
- Dependency Injection

---

## 如何執行應用程式

### 方法一:  使用 Docker

**必要安裝:**
- docker
- docker-compose
- dotnet ef cli

**步驟:**

1. 在專案根目錄建立 `.env` 檔案，填寫 pg 和 redis 的帳號密碼，如範例 `.env.example`

2. 執行啟動腳本：
   ```bash
   # 在專案根目錄執行
   ./run-csharp.sh
   ```

3. 存取 API：
   - Swagger UI: http://localhost:5000/swagger
   - API Base URL: http://localhost:5000/api

4. 停止服務：
   ```bash
   ./stop-csharp.sh
   ```

---

### 方法二: 使用原始碼

**必要安裝:**
- dotnet sdk 10
- dotnet ef cli

**前置準備:**
- 請先確定你有 pg 和 redis
- 請確定你的 pg 已經有被初始化過
- 請確定 [src/csharp/api/appsettings.Development.json](api/appsettings.Development.json) 中的連線字串是正確的

**步驟:**

1. 初始化資料庫：
   ```bash
   dotnet ef database update --project src/csharp/api/api.csproj
   ```

2. 建置專案：
   ```bash
   dotnet build src/csharp/jubo-example-api.sln
   ```

3. 執行應用程式：
   ```bash
   dotnet run --project src/csharp/api/api.csproj
   ```

4. 存取 API：
   - Swagger UI: http://localhost:5000/swagger

---

## 專案結構

```
src/csharp/
├── api/
│   ├── ApiEndpoints/      # API 端點定義
│   ├── Data/              # EF Core DbContext
│   ├── DateTimeOffsetProviders/
│   ├── Dtos/              # 資料傳輸物件
│   ├── Enums/
│   ├── Extensions/
│   ├── Infrastructure/
│   ├── Middlewares/
│   ├── Models/            # 資料模型
│   ├── Options/
│   ├── Repositories/      # 資料存取層
│   ├── Services/          # 業務邏輯層
│   ├── Validators/
│   └── Program.cs         # 應用程式進入點
```