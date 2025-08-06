#!/bin/bash

# Script para parar o RPG Backend
# Autor: Script gerado automaticamente

# Cores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

log_info() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

log_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

log_info "Parando serviços RPG..."

# Vai para o diretório do servidor
cd ../RPGServer

# Para os containers Docker
docker-compose down

log_success "Serviços parados com sucesso!"
