# Scripts de Execução RPG

Este diretório contém scripts para facilitar a execução do sistema RPG no Linux.

## Pré-requisitos

Antes de executar os scripts, certifique-se de ter instalado:

- **Docker** e **Docker Compose**
- **.NET 9.0 SDK**
- **netcat** (nc) - geralmente já vem instalado na maioria das distribuições Linux

### Instalação dos pré-requisitos no Ubuntu/Debian:

```bash
# Docker
sudo apt update
sudo apt install docker.io docker-compose-v2 netcat-openbsd

# .NET 9.0
wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt update
sudo apt install dotnet-sdk-9.0
```

## Scripts Disponíveis

### `start_rpg.sh` - Iniciar o Sistema Completo

Este script executa todo o processo de inicialização:

1. Verifica se Docker e .NET estão instalados
2. Inicia o backend (PostgreSQL + RPG Server) via Docker
3. Aguarda os serviços ficarem prontos
4. Compila o cliente RPG
5. Executa o cliente

**Como usar:**
```bash
cd /home/github/RPGConsole
./start_rpg.sh
```

### `stop_rpg.sh` - Parar o Backend

Para parar apenas os serviços do backend (Docker containers):

```bash
cd /home/github/RPGConsole
./stop_rpg.sh
```

### `Tests/run_tests.sh` - Executar Testes 🧪

Script para executar todos os testes de unidade e integração:

```bash
cd /home/github/RPGConsole/Tests
./run_tests.sh
```

**O que os testes verificam:**
- ⚔️ **Sistema de Combate**: Dano, morte, variação de ataques
- 💰 **Sistema de Loot**: Probabilidades de drop, itens específicos
- 📈 **Sistema de Experiência**: Distribuição correta de XP
- 🔗 **Integração**: Fluxos completos de combate
- 🎲 **Aleatoriedade**: Variação de danos e drops

## Uso

### Execução Normal
```bash
# Navegar para o diretório
cd /home/github/RPGConsole

# Executar o sistema completo
./start_rpg.sh
```

### Executar Testes
```bash
# Navegar para o diretório de testes
cd /home/github/RPGConsole/Tests

# Executar todos os testes
./run_tests.sh
```

### Parar o Sistema
- **Durante a execução**: Pressione `Ctrl+C` - isso irá parar tanto o cliente quanto o backend
- **Apenas backend**: Execute `./stop_rpg.sh`

### Serviços Disponíveis
Após executar `start_rpg.sh`, você terá acesso a:
- **RPG Server (gRPC)**: localhost:5000
- **PostgreSQL**: localhost:5432
- **PgAdmin**: http://localhost:5050
  - Email: admin@rpg.com
  - Senha: admin

## Logs e Debug

O script `start_rpg.sh` fornece logs coloridos para facilitar o acompanhamento:
- 🔵 **[INFO]**: Informações gerais
- 🟢 **[SUCCESS]**: Operações bem-sucedidas  
- 🟡 **[WARNING]**: Avisos
- 🔴 **[ERROR]**: Erros

## Troubleshooting

### Problema: "Docker não está instalado"
- Instale o Docker seguindo as instruções dos pré-requisitos

### Problema: "Port already in use"
- Execute `./stop_rpg.sh` para parar containers anteriores
- Ou use: `docker-compose down` no diretório RPGServer

### Problema: ".NET não está instalado"
- Instale o .NET 9.0 SDK seguindo as instruções dos pré-requisitos

### Problema: "nc command not found"
- Instale netcat: `sudo apt install netcat-openbsd`

### Problema: Serviços não ficam prontos
- Verifique se as portas 5000, 5001 e 5432 não estão sendo usadas por outros processos
- Aguarde mais tempo - primeira execução pode demorar para baixar imagens Docker

## Estrutura do Sistema

```
RPGConsole/          # Cliente do jogo
├── start_rpg.sh     # Script principal de inicialização
├── stop_rpg.sh      # Script para parar backend
└── src/             # Código fonte do cliente

RPGServer/           # Backend do jogo
├── docker-compose.yml  # Configuração Docker
├── RPGServer/          # Código fonte do servidor
└── postgres/           # Scripts de inicialização do BD

RPGShared/           # Código compartilhado
└── Models/          # Modelos do domínio
```
