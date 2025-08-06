#!/bin/bash

# Script para executar todos os testes do RPG
# Autor: Script gerado automaticamente

set -e

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

log_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

log_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Verificar se .NET está instalado
if ! command -v dotnet &> /dev/null; then
    log_error ".NET não está instalado. Por favor, instale o .NET 9.0 SDK."
    exit 1
fi

# Diretório do projeto de testes
TEST_DIR="/home/github/RPGConsole/Tests"

log_info "Executando testes do RPG..."
log_info "Diretório de testes: $TEST_DIR"

cd "$TEST_DIR"

# Restaurar dependências
log_info "Restaurando dependências..."
if ! dotnet restore; then
    log_error "Falha ao restaurar dependências"
    exit 1
fi

# Compilar projeto de testes
log_info "Compilando projeto de testes..."
if ! dotnet build; then
    log_error "Falha ao compilar projeto de testes"
    exit 1
fi

log_success "Projeto compilado com sucesso!"

# Executar testes
echo ""
log_info "Executando todos os testes..."
echo ""

if dotnet test --verbosity normal --collect:"XPlat Code Coverage"; then
    log_success "Todos os testes passaram! ✅"
else
    log_error "Alguns testes falharam! ❌"
    exit 1
fi

echo ""
log_info "Resumo dos testes executados:"
log_info "📋 Testes de unidade: Verificam funcionalidades isoladas"
log_info "🔗 Testes de integração: Verificam fluxos completos de combate"
log_info "🎯 Cobertura: Sistema de loot, combate, e morte de monstros"

echo ""
log_success "Execução de testes concluída!"
echo ""
log_info "Para executar testes específicos, use:"
echo "  dotnet test --filter \"ClassName=MonstroTests\""
echo "  dotnet test --filter \"TestCategory=Integration\""
echo "  dotnet test --filter \"FullyQualifiedName~Amazon\""
