# Example API - Python FastAPI Implementation

這是用 Python FastAPI 實作的 Example API，API Contract 與 ASP.NET Core 版本完全一致。

## 📋 專案結構

```
src/python/
├── app/
│   ├── __init__.py
│   ├── config.py              # 配置設定
│   ├── database.py            # 資料庫連接
│   ├── models.py              # SQLAlchemy 模型
│   ├── api/                   # API 端點
│   │   ├── __init__.py
│   │   ├── patient_endpoints.py
│   │   └── order_endpoints.py
│   ├── schemas/               # Pydantic schemas
│   │   ├── __init__.py
│   │   ├── dtos.py
│   │   ├── requests.py
│   │   └── responses.py
│   ├── repositories/          # 資料存取層
│   │   ├── __init__.py
│   │   ├── patient_repository.py
│   │   └── order_repository.py
│   └── services/              # 業務邏輯層
│       ├── __init__.py
│       ├── patient_service.py
│       └── order_service.py
├── main.py                    # FastAPI 應用程式進入點
├── requirements.txt           # Python 依賴套件
├── .env.example              # 環境變數範例
└── .gitignore                # Git 忽略檔案
```

## 🚀 快速開始

### 1. 安裝依賴套件

```bash
cd src/python
python3 -m venv venv
source venv/bin/activate  # Windows: venv\Scripts\activate
pip install -r requirements.txt
```

### 2. 設定環境變數

本專案使用根目錄的 `.env` 檔案（與 C# 和 Node.js 版本共用）。

確保根目錄的 `.env` 檔案包含以下設定：

```
POSTGRES_USER=your_username
POSTGRES_PASSWORD=your_password
```

Python FastAPI 會自動讀取這些設定並建構資料庫連線字串。

### 3. 啟動資料庫

確保 PostgreSQL 資料庫已啟動並建立好資料表。如果使用 Docker：

```bash
# 從專案根目錄執行
cd ../../
docker-compose --env-file .env -f ./docker/pg-docker-compose.yml up -d
```

### 4. 啟動 API 服務

```bash
cd src/python
python main.py
```

或使用 uvicorn：

```bash
uvicorn main:app --host 0.0.0.0 --port 5002 --reload
```

### 5. 存取 API

- API: http://localhost:5002
- 互動式 API 文件 (Swagger): http://localhost:5002/docs
- 替代 API 文件 (ReDoc): http://localhost:5002/redoc
- Health Check: http://localhost:5002/healthz

## 📡 API 端點

### Patient Management (病患管理)

- `GET /api/patients` - 查詢病患列表（支援日期範圍、分頁）
- `GET /api/patients/{id}` - 查詢單一病患
- `POST /api/patients` - 新增病患

### Order Management (訂單管理)

- `GET /api/orders/{id}` - 查詢單一訂單
- `POST /api/orders` - 新增訂單
- `PUT /api/orders/{id}` - 更新訂單訊息

## 🔧 開發工具

### 使用 api.http 測試

修改 `api.http` 檔案中的變數：

```http
@baseUrl = http://localhost:5002
```

然後使用 VS Code REST Client 擴充套件執行請求。

## 📝 API Contract

此實作與 ASP.NET Core 版本的 API Contract 完全一致：

- 相同的端點路徑
- 相同的 HTTP 方法
- 相同的請求/回應格式
- 相同的錯誤處理
- 相同的驗證規則

## 🛠️ 技術棧

- **FastAPI** - 現代、快速的 Python Web 框架
- **SQLAlchemy** - ORM 框架（支援 async）
- **asyncpg** - PostgreSQL 非同步驅動
- **Pydantic** - 資料驗證和設定管理
- **Uvicorn** - ASGI 伺服器

## 📚 相關連結

- [FastAPI 文件](https://fastapi.tiangolo.com/)
- [SQLAlchemy 文件](https://docs.sqlalchemy.org/)
- [Pydantic 文件](https://docs.pydantic.dev/)
