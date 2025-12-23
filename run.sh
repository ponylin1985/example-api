#!/bin/bash

GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

echo -e "${YELLOW}Checking environment configuration...${NC}"

if [ ! -f .env ]; then
    echo -e "${RED}Error: .env file not found!${NC}"
    echo "Please create a .env file based on .env.example"
    exit 1
fi

echo -e "${YELLOW}Starting PostgreSQL container...${NC}"
docker-compose --env-file .env -f ./docker/pg-docker-compose.yml up -d

if [ $? -ne 0 ]; then
    echo -e "${RED}Failed to start PostgreSQL container.${NC}"
    exit 1
fi

echo -e "${YELLOW}Waiting for PostgreSQL to be ready...${NC}"
MAX_RETRIES=30
COUNT=0

while [ $COUNT -lt $MAX_RETRIES ]; do
    if docker exec pgsql pg_isready > /dev/null 2>&1; then
        echo -e "${GREEN}PostgreSQL is ready!${NC}"
        break
    fi
    
    echo -n "."
    sleep 2
    COUNT=$((COUNT+1))
done

if [ $COUNT -eq $MAX_RETRIES ]; then
    echo -e "${RED}Timeout waiting for PostgreSQL to become ready.${NC}"
    exit 1
fi

# 載入 .env 變數
export $(grep -v '^#' .env | xargs)

# 設定 User Secrets
echo -e "${YELLOW}Configuring User Secrets for database migration...${NC}"
CONNECTION_STRING="Host=localhost;Database=${POSTGRES_USER};Username=${POSTGRES_USER};Password=${POSTGRES_PASSWORD};Pooling=true;MinPoolSize=10;MaxPoolSize=100"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "$CONNECTION_STRING" --project src/api/api.csproj

if [ $? -ne 0 ]; then
    echo -e "${RED}Failed to set User Secrets.${NC}"
    exit 1
fi

# 執行 EF Core Migration
echo -e "${YELLOW}Applying database migrations...${NC}"
dotnet ef database update --project src/api/api.csproj

if [ $? -ne 0 ]; then
    echo -e "${RED}Failed to apply database migrations.${NC}"
    exit 1
else
    echo -e "${GREEN}Database migrations applied successfully!${NC}"
fi

echo -e "${YELLOW}Starting API and Redis containers...${NC}"
docker-compose --env-file .env -f ./docker/api-docker-compose.yml up -d --build

if [ $? -eq 0 ]; then
    echo -e "${GREEN}All services started successfully!${NC}"
    echo -e "API is running at: http://localhost:5000"
    echo -e "Swagger UI: http://localhost:5000/swagger"
else
    echo -e "${RED}Failed to start API containers.${NC}"
    exit 1
fi
