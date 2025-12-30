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

echo -e "${YELLOW}Starting Node.js API...${NC}"
cd src/nodejs

if [ ! -d "node_modules" ]; then
    echo -e "${YELLOW}Installing dependencies...${NC}"
    npm install
fi

npm run dev
