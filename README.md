# BMPTec Challenge - Chu Bank API

## Como executar

Clone o repositório

```bash
  git clone https://github.com/joao-bagatoli/bmptec-backend-challenge.git
```

```bash
  cd bmptec-backend-challenge
```

### Executar com Docker

#### Suba as aplicações com o Docker Compose

```bash
  docker compose -f docker-compose.yml up -d --build
```

#### A aplicação estará disponível pelo Swagger em: [link](http://localhost:8080/swagger/index.html)

### Executar localmente (sem Docker)

Via CLI

```bash
  dotnet run --project src/Chu.Bank.Api/Chu.Bank.Api.csproj
```

Via Debug, abra a solução em uma IDE e execute o projeto

```bash
  Chu.Bank.Api
```

#### A aplicação estará disponível pelo Swagger em: [link](https://localhost:7061/swagger/index.html)

### Executar os testes

Via CLI

```bash
  dotnet test Chu.Bank.sln
```

Via Debug, abra a solução em uma IDE e execute o projeto de testes

```bash
  Chu.Bank.UnitTests
```
