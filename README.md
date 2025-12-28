# Example API - Multi-Language Implementation

這是一個展示如何用不同程式語言實作相同 API 規格的專案。

## 📁 專案結構

```
example-api/
├── csharp/          # C# ASP.NET Core 10 實作
├── nodejs/          # Node.js + Express + TypeScript 實作 (開發中)
├── docker/          # Docker 相關檔案
└── Scripts          # 執行腳本
```

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

## 🚀 C# ASP.NET Core 版本

請參考 [csharp/README.md](csharp/README.md)

### 快速啟動

```bash
./run-csharp.sh
open http://localhost:5000/swagger
```

---

## 🚀 Node.js Express 版本

請參考 [nodejs/README.md](nodejs/README.md)

### 快速啟動

```bash
./run-nodejs.sh
open http://localhost:5000/api-docs
```

---

## 🗄️ 資料庫

兩個版本共用同一個 PostgreSQL 資料庫。

### 啟動資料庫

```bash
docker-compose --env-file .env -f ./docker/pg-docker-compose. yml up -d
```

### Schema

- `patient` - 病患資料表
- `order` - 訂單資料表

---

## 🛑 停止所有服務

```bash
./stop.sh
```

---

## 📝 備註

- 這是一個學習/比較用途的專案
- 兩個版本實作相同的 API 規格
- 可以用來對照不同語言/框架的實作方式
