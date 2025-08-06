#!/bin/bash

# Script para iniciar o RPG - Backend e Cliente
# Autor: Script gerado automaticamente
# Data: $(date)

set -e  # Para o script se houver erro

# Cores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Função para log colorido
log_info() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

log_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

log_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

log_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Função para verificar se Docker está instalado
check_docker() {
    if ! command -v docker &> /dev/null; then
        log_error "Docker não está instalado. Por favor, instale o Docker primeiro."
        exit 1
    fi
    
    if ! command -v docker-compose &> /dev/null && ! docker compose version &> /dev/null; then
        log_error "Docker Compose não está disponível. Por favor, instale o Docker Compose."
        exit 1
    fi
}

# Função para verificar se .NET está instalado
check_dotnet() {
    if ! command -v dotnet &> /dev/null; then
        log_error ".NET não está instalado. Por favor, instale o .NET 9.0 SDK."
        exit 1
    fi
}

# Função para aguardar serviço estar pronto
wait_for_service() {
    local host=$1
    local port=$2
    local service_name=$3
    local timeout=${4:-60}
    
    log_info "Aguardando $service_name estar pronto em $host:$port..."
    
    for i in $(seq 1 $timeout); do
        if nc -z $host $port 2>/dev/null; then
            log_success "$service_name está pronto!"
            return 0
        fi
        sleep 1
        echo -n "."
    done
    
    log_error "$service_name não ficou pronto em $timeout segundos"
    return 1
}

# Função para parar serviços em caso de erro ou saída
cleanup() {
    log_warning "Parando serviços..."
    cd ../RPGServer
    docker-compose down
    exit 0
}

# Captura sinais para cleanup
trap cleanup SIGINT SIGTERM

# Verificações iniciais
log_info "Verificando dependências..."
check_docker
check_dotnet

# Diretório atual (RPGConsole)
CURRENT_DIR=$(pwd)

# 1. Iniciar Backend (Docker)
log_info "Iniciando backend (PostgreSQL + RPG Server)..."
cd ../RPGServer

# Para containers existentes se estiverem rodando
docker-compose down 2>/dev/null || true

# Inicia os serviços
log_info "Executando docker-compose up..."
docker-compose up -d

# Aguarda PostgreSQL estar pronto
if ! wait_for_service localhost 5432 "PostgreSQL" 60; then
    log_error "PostgreSQL não iniciou corretamente"
    cleanup
fi

# Aguarda RPG Server estar pronto
if ! wait_for_service localhost 5000 "RPG Server" 60; then
    log_error "RPG Server não iniciou corretamente"
    cleanup
fi

log_success "Backend iniciado com sucesso!"
log_info "PostgreSQL: localhost:5432"
log_info "RPG Server: localhost:5000"
log_info "PgAdmin: http://localhost:5050 (admin@rpg.com / admin)"

# 2. Compilar e executar Cliente
log_info "Compilando cliente RPG..."
cd "$CURRENT_DIR"

# Build do projeto
if ! dotnet build src/RPGConsole.csproj; then
    log_error "Falha ao compilar o cliente"
    cleanup
fi

log_success "Cliente compilado com sucesso!"

# Aguarda um pouco para garantir que tudo está estável
log_info "Aguardando estabilização dos serviços..."
sleep 3

# Executa o cliente
log_info "Iniciando cliente RPG..."
log_warning "Pressione Ctrl+C para parar todos os serviços"
echo ""

# Executa o cliente
dotnet run --project src/RPGConsole.csproj

# Se chegou aqui, o cliente foi fechado normalmente
cleanup
