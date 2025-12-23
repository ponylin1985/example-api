#!/bin/bash

GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

echo -e "${YELLOW}Stopping services...${NC}"

echo -e "${YELLOW}Stopping API and Redis containers...${NC}"
if [ -f docker/api-docker-compose.yml ]; then
    docker-compose --env-file .env -f ./docker/api-docker-compose.yml down
else
    echo -e "${RED}Warning: docker/api-docker-compose.yml not found.${NC}"
fi

echo -e "${YELLOW}Stopping PostgreSQL container...${NC}"
if [ -f docker/pg-docker-compose.yml ]; then
    docker-compose --env-file .env -f ./docker/pg-docker-compose.yml down
else
    echo -e "${RED}Warning: docker/pg-docker-compose.yml not found.${NC}"
fi

echo -e "${YELLOW}Removing API image (jubo-example-api:1.0.0)...${NC}"
if docker image inspect jubo-example-api:1.0.0 > /dev/null 2>&1; then
    docker rmi jubo-example-api:1.0.0
    if [ $? -eq 0 ]; then
        echo -e "${GREEN}Image jubo-example-api:1.0.0 removed successfully.${NC}"
    else
        echo -e "${RED}Failed to remove image jubo-example-api:1.0.0 (it might be in use).${NC}"
    fi
else
    echo -e "${YELLOW}Image jubo-example-api:1.0.0 not found, skipping.${NC}"
fi

echo -e "${GREEN}All services stopped and cleaned up!${NC}"
