# Testes do Sistema RPG

Este diretório contém uma suíte abrangente de testes para validar o sistema de combate, loot e mecânicas do RPG.

## 📋 Estrutura dos Testes

### Testes de Unidade (`UnitTests/`)

#### `MonstroBasicTests.cs`
- ✅ Criação básica de monstros
- ✅ Sistema de dano e vida
- Testes fundamentais para garantir funcionamento básico

#### `MonstroCompleteTests.cs`
- 🏹 **Sistema de Combate**: Verifica dano, morte, e variação de ataques
- 💰 **Sistema de Loot**: Testa probabilidades de drop e itens específicos
- 📊 **Configuração de Loot**: Valida que monstros têm chances corretas
- 🎲 **Aleatoriedade**: Confirma variação nos danos e drops

### Testes de Integração (`IntegrationTests/`)

#### `CombateIntegrationTests.cs`
- ⚔️ **Combate Completo**: Fluxo completo jogador vs monstro
- 👥 **Múltiplos Monstros**: Combate contra vários inimigos
- 💀 **Morte do Jogador**: Verifica fim de batalha por morte
- 🧮 **Cálculo de Dano**: Testa serviços de batalha
- 🏆 **Processamento de Vitória**: Distribuição de XP e loot

## 🎯 O que os Testes Cobrem

### Sistema de Loot ✅
- **Amazon**: 100% Moedas, 75% Colar da Amazonas, 50% Poção de Vida
- **Goblin**: 100% Moedas, 50% Poção de Vida
- **Probabilidades**: Testa se os drops respeitam as chances configuradas
- **Múltiplos Drops**: Verifica quantidade correta de itens

### Sistema de Combate ✅
- **Dano Variável**: Confirma que ataques têm variação aleatória
- **Sistema de Defesa**: Testa redução de dano por defesa
- **Morte**: Verifica transição vida > 0 para vida = 0
- **Vida**: Confirma que monstros nascem vivos e com vida positiva

### Sistema de Experiência ✅
- **Ganho de XP**: Verifica distribuição correta de experiência
- **Múltiplos Monstros**: Soma XP de vários inimigos derrotados
- **Sem XP para Vivos**: Garante que monstros vivos não dão XP

### Integração Completa ✅
- **Fluxo Jogador vs Monstro**: Teste completo de um combate
- **Processamento de Vitória**: Distribuição automática de recompensas
- **Fim de Batalha**: Detecção correta de condições de fim

## 🚀 Como Executar

### Execução Simples
```bash
cd /home/github/RPGConsole/Tests
./run_tests.sh
```

### Execução Manual
```bash
cd /home/github/RPGConsole/Tests
dotnet test --verbosity normal
```

### Testes Específicos
```bash
# Apenas testes de monstros
dotnet test --filter "ClassName=MonstroTests"

# Apenas testes de integração
dotnet test --filter "IntegrationTests"

# Testes relacionados a Amazon
dotnet test --filter "FullyQualifiedName~Amazon"
```

## 📊 Cobertura dos Testes

Os testes cobrem os seguintes aspectos críticos:

### ✅ Funcionalidades Testadas
- **Criação de Monstros**: Todas as propriedades iniciais
- **Sistema de Dano**: Cálculo, aplicação e redução por defesa
- **Sistema de Morte**: Transição para estado morto
- **Sistema de Loot**: Probabilidades e tipos de itens
- **Sistema de Experiência**: Distribuição e cálculo
- **Batalhas**: Fluxos completos de combate
- **Serviços**: BatalhaService e todas suas funções

### 🎲 Aspectos Aleatórios Testados
- **Variação de Dano**: Confirma que há randomização
- **Probabilidades de Loot**: Testa com múltiplas execuções
- **Críticos e Falhas**: Verifica eventos aleatórios

### 🔧 Robustez
- **Casos Extremos**: Dano massivo, vida zero, etc.
- **Estados Inválidos**: Monstros mortos, jogadores mortos
- **Integração**: Múltiplos sistemas trabalhando juntos

## 📈 Resultados Esperados

Quando todos os testes passam, você tem garantia de que:

1. **⚔️ Combate funciona corretamente**
2. **💰 Loot é distribuído conforme configurado**
3. **📈 Experiência é calculada adequadamente**
4. **🎮 Fluxo de jogo está integrado**
5. **🛡️ Sistema é robusto contra casos extremos**

## 🐛 Troubleshooting

### Testes Falhando
- Verifique se as dependências estão atualizadas: `dotnet restore`
- Confirme que o projeto RPGShared compila: `cd ../../RPGShared && dotnet build`
- Execute testes individuais para isolar problemas

### Problemas de Console
- Os testes capturam output do Console para não poluir a saída
- Se houver problemas relacionados ao Console, verifique se os `using` statements estão corretos

### Aleatoriedade
- Alguns testes dependem de probabilidades e podem ocasionalmente falhar
- Execute novamente se suspeitar de falha por aleatoriedade
- Testes são configurados com múltiplas execuções para minimizar isso

---

**🎯 Objetivo**: Garantir que o sistema de combate, loot e experiência do RPG funciona perfeitamente!

**✅ Status**: Todos os testes passando - Sistema validado e funcional!
