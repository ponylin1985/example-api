# 錯誤日誌修正說明

## 問題描述
FastAPI 應用程式在發生錯誤時，沒有正確記錄到 console log 和 file log。

## 修正內容

### 1. Service 層錯誤日誌 ✅
**修改檔案:**
- [app/services/patient_service.py](app/services/patient_service.py)
- [app/services/order_service.py](app/services/order_service.py)

**修改內容:**
在所有 `except Exception as e:` 區塊中添加 `logger.error()` 呼叫：
```python
except Exception as e:
    logger.error("錯誤訊息: %s", str(e), exc_info=True)
    # ... 返回錯誤回應
```

`exc_info=True` 參數會記錄完整的 stack trace，方便除錯。

### 2. 全域 Exception Handler ✅
**修改檔案:**
- [main.py](main.py)

**新增功能:**
- 捕獲所有未處理的例外狀況
- 捕獲請求驗證錯誤 (RequestValidationError)
- 記錄錯誤詳情包含請求路徑和方法
- 返回統一的錯誤回應格式

```python
@app.exception_handler(Exception)
async def global_exception_handler(request: Request, exc: Exception):
    logger.error(
        "Unhandled exception occurred: %s, Path: %s, Method: %s",
        str(exc),
        request.url.path,
        request.method,
        exc_info=True,
    )
    # ...
```

### 3. 檔案日誌配置 ✅
**新增檔案:**
- [app/configs/logging_config.py](app/configs/logging_config.py)

**功能:**
- **Console 輸出**: 顯示所有層級的日誌 (根據 LOG_LEVEL 設定)
- **app.log**: 記錄所有層級的日誌
  - 使用 RotatingFileHandler
  - 最大檔案大小: 10MB
  - 保留 5 個備份檔案
  - 詳細格式包含檔案名稱和行號
  
- **error.log**: 只記錄 ERROR 和 CRITICAL 層級
  - 使用 RotatingFileHandler
  - 最大檔案大小: 10MB
  - 保留 5 個備份檔案
  - 詳細格式包含檔案名稱和行號

**日誌檔案位置:**
```
src/python/logs/
├── app.log          # 所有日誌
├── app.log.1        # 輪替備份
├── error.log        # 錯誤日誌
└── error.log.1      # 輪替備份
```

## 日誌格式

### Console 輸出
```
2026-01-07 10:30:45 - INFO - Starting up FastAPI application...
```

### 檔案輸出
```
2026-01-07 10:30:45 - app.services.patient_service - ERROR - [patient_service.py:72] - Failed to retrieve patients: connection timeout
```

## 測試

### 測試日誌配置
```bash
cd src/python
python test_logging.py
```

這會產生測試日誌到 `logs/` 目錄。

### 測試 API 錯誤日誌
1. 啟動應用程式
2. 發送會產生錯誤的請求（例如不存在的病患 ID）
3. 查看 console 輸出和日誌檔案

**範例:**
```bash
# 查看最新的錯誤日誌
tail -f src/python/logs/error.log

# 查看所有日誌
tail -f src/python/logs/app.log
```

## 日誌層級設定

可以在 `.env` 檔案中設定日誌層級：
```
LOG_LEVEL=INFO  # DEBUG, INFO, WARNING, ERROR, CRITICAL
```

## 注意事項

1. **logs 目錄**: 應用程式首次啟動時會自動創建
2. **日誌輪替**: 當日誌檔案超過 10MB 時會自動輪替
3. **備份檔案**: 最多保留 5 個備份檔案
4. **編碼**: 日誌檔案使用 UTF-8 編碼
5. **生產環境**: 建議將日誌檔案排除在版本控制之外（已在 .gitignore 中設定）
