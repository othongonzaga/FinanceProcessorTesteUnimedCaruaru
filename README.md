
# 🏦 FinanceProcessor

Sistema de Processamento de Transações Financeiras Avançado, construído com .NET 8 e arquitetura em camadas.

---

## 📋 Descrição Geral

Este sistema gerencia múltiplas contas bancárias e suas transações. As operações disponíveis incluem:

- Abertura de contas  
- Créditos e débitos  
- Transferências entre contas
- Consultas de saldo e extrato  
- Filtros por tipo de transação  
- Cálculo de saldo histórico  
- Identificação da transação mais valiosa  

> O projeto atualmente **não utiliza banco de dados**: todos os dados residem em memória, tornando-o leve e ideal para simulação e testes locais.

---

## 🏗️ Arquitetura

```
FinanceProcessor/
│
├── FinanceProcessor.Domain/       
│   ├── Entities/
│   ├── Enums/
│   ├── Exceptions/
│
├── FinanceProcessor.Application/ 
│   ├── Interfaces/
│   ├── Services/
│
├── FinanceProcessor.Infrastructure/ 
│   └── Repositories/
│
├── FinanceProcessor.Console/  
│   └── Program.cs
│
├── FinanceProcessor.sln        
```

---

## 🚀 Como Executar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- Visual Studio 2022 ou superior  

### Rodando com Visual Studio

1. Abra o arquivo `FinanceProcessor.sln`  
2. Defina `FinanceProcessor.Console` como projeto de inicialização  
3. Execute com `Ctrl + F5`  

### Rodando via terminal

```bash
cd FinanceProcessor.Console
dotnet run
```

---

## 📚 Funcionalidades

| # | Funcionalidade            | Descrição                                            |
|---|--------------------------|-----------------------------------------------------|
| 1 | Abrir Conta              | Cria uma nova conta com agência, número, moeda e saldo inicial |
| 2 | Consultar Saldo          | Retorna o saldo atual da conta                       |
| 3 | Registrar Crédito        | Adiciona um valor positivo ao saldo da conta        |
| 4 | Registrar Débito         | Subtrai valor do saldo se houver fundos suficientes |
| 5 | Transferência            | Move valor entre duas contas                        |
| 6 | Obter Extrato            | Lista transações entre duas datas                    |
| 7 | Listar por Tipo          | Filtra transações por tipo (crédito, débito, etc.)  |
| 8 | Saldo por Data           | Calcula saldo acumulado até uma data específica      |
| 9 | Transação Mais Valiosa   | Identifica a transação de maior valor de um tipo e período |

---

## 💡 Exemplos de Execução

- Abrir conta para Cliente A (Ag: 001, Conta: 12345-6, Moeda: BRL, Saldo: 0)  
- Creditar R$ 1000 em A ("Depósito Inicial")  
- Debitar R$ 50 de A ("Conta de Luz")  
- Abrir conta B (Ag: 002, Conta: 98765-4, Moeda: BRL, Saldo: 0)  
- Transferir R$ 200 de A para B  
- Tentar debitar R$ 1000 de A (falhará)  
- Gerar extrato de A no mês atual  
- Calcular saldo de A no dia anterior  

---

## 🛡️ Regras de Negócio Implementadas

- Transações devem ter valor positivo  
- Débitos e transferências checam saldo disponível  
- A moeda da transação deve coincidir com a da conta  
- IDs das contas e transações são únicos (Guid)  
- Transferências são atômicas (débito + crédito)  

---

## 💾 (Bônus) Persistência: Como Abordar

Atualmente o sistema utiliza um repositório em memória, ótimo para testes. Para produção, recomenda-se usar um banco relacional como PostgreSQL.

### Estratégia de Implementação

- Adicionar o pacote `Microsoft.EntityFrameworkCore` e `Npgsql.EntityFrameworkCore.PostgreSQL`  
- Criar um `FinanceContext` com `DbSet<Conta>` e `DbSet<Transacao>`  
- Implementar a interface `IRepositorioContas` com `RepositorioContasPostgres`  
- Configurar a `ConnectionString` em `appsettings.json`  
- Usar `docker-compose.yml` com serviço do PostgreSQL e aplicar migrações com `dotnet ef`

Exemplo `docker-compose.yml`:

```yaml
version: '3.9'
services:
  postgres:
    image: postgres:16
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
      POSTGRES_DB: finance
    ports:
      - "5432:5432"
    volumes:
      - pgdata:/var/lib/postgresql/data

volumes:
  pgdata:
```

> A separação por interfaces permite que essa troca seja feita sem modificar a lógica de negócio da aplicação.

---

## 🧱 Extensibilidade

O projeto está preparado para:

- Adicionar novos tipos de transação (ex: cashback, cobrança, etc.)  
- Suporte a câmbio ou múltiplas moedas  
- API REST (adicionando um novo projeto WebAPI)  
- Interface gráfica (WPF, MAUI, Blazor etc.)  
- Integração com banco relacional (PostgreSQL, SQL Server, etc.)  

---

## ✍️ Autor: Othon Gonzaga

Criado com 💚 e foco em boas práticas de desenvolvimento .NET:

- Clean Architecture  
- SOLID  
- Separação de Responsabilidades  
- Padrões de Repositório e Injeção de Dependência  

---
