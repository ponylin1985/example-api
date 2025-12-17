# ASP.NET Core API for Jubo interview

-----------------

- 專案用途: 這是一個使用 ASP.NET Core 10 開發的 API 服務，主要用途是提供給「智齡科技」公司當作面試使用。
  - 因為是面試用途的專案，因此不會有過多跟面試題目無關的 API 功能、CI/CD pipeline 建置以及完整的 DevOps 腳本等等。
  - 因為是面試用途的專案，以下 README.md 內容不會解釋用了什麼技術、為什麼要使用這種技術、並且不會有完整的自動化腳本、參數管理，也不會有對必要安裝套件的完整安裝說明，僅提供最低限度說明。
  - 因為我本人應徵的是資深後端工程師職位，因此，此專案中不會有任何前端的程式碼。

-----------------

## 如何執行應用程式

### 準備 pg 資料庫

- 可以用任意方式準備 pg 資料庫，或者依照以下的方式在本機產生一個 pg 的 container 當作資料庫。

- 必要安裝
  - docker
  - docker-compose
  - dotnet ef cli

- step 1. 建立 pg 資料庫。
  - 在專案根目錄建立 `.env` 檔案，填寫 pg 的帳號密碼，如範例 [.env.example](.env.example)
  - 在專案根目錄先執行 `docker-compose --env-file .env -f ./docker/pg-docker-compose.yml up -d`
    - 會在本機跑起一個名稱為 pgsql 的 container 當作資料庫。

- step 2. 設定 migration cli 的資料庫連線。
  - 確保 [appsettings.Development.json](src/api/appsettings.Development.json) 中的連線字串是正確的。
    - 注意: 因為是在本機執行 `dotnet ef` 指令，連線字串的 Host 應該是 `localhost`。

- step 3. 初始化資料庫。
  - 在專案根目錄執行 `dotnet ef database update --project src/api/api.csproj`。


### 方法一: 使用 docker

- 必要安裝
  - docker
  - docker-compose

- step 1. 確保資料庫連線正確。
  - 調整 [appsettings.json](src/api/appsettings.json) 中的連線字串是正確的。
    - 注意: 
      - 若是在 docker container 內執行 API，因為已設定共用網路，Host 請直接設定為 `pgsql` (即資料庫 container 名稱)，不需區分作業系統。
  - 或者你不想使用這一份 [appsettings.json](src/api/appsettings.json) 設定檔，則請自行調整 [api-docker-compose.yml](docker/api-docker-compose.yml) 的設定。

- step 2. 啟動應用程式。
  - 在專案根目錄先執行 `docker-compose -f ./docker/api-docker-compose.yml up -d`
    - 會在本機跑起一個名稱為 jubo-api 的 container。

### 方法二: 使用原始碼

- 必要安裝
  - dotnet sdk 10

- step 1.
  - 在專案根目錄執行 `dotnet build src/jubo-example-api.sln`。

- step 2.
  或者你想省略 step 1，直接執行 `dotnet run --project ./src/api/api.csproj` 也可以。

-----------------

## 呼叫 API 服務

- 使用以上兩種方法把 ASP.NET Core 應用程式啟動後。
- 在瀏覽器上輸入 http://localhost:5000/swagger 可以開啟 online swagger 頁面。
