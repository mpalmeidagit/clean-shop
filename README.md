# Clean Shop

API REST de e-commerce construída com **ASP.NET Core (.NET 10)** seguindo os princípios da
**Clean Architecture**. O projeto usa o banco de exemplo **Northwind** (SQL Server) e expõe
operações de clientes e autenticação com **JWT**.

## Arquitetura

A solution (`CleanShop/CleanShop.slnx`) é dividida em camadas, cada uma com projetos por responsabilidade:

| Camada | Projetos | Responsabilidade |
|---|---|---|
| **Domain** | `Domain.Entity`, `Domain.Interface`, `Domain.Core` | Entidades e regras de negócio |
| **Application** | `Application.DTO`, `Application.Interface`, `Application.Main`, `Application.Validator` | Casos de uso, DTOs e validações |
| **Infrastructure** | `Infrastructure.Data`, `Infrastructure.Interface`, `Infrastructure.Repository` | Acesso a dados com Dapper + SQL Server |
| **Transversal** | `Transversal.Common`, `Transversal.Logging`, `Transversal.Mapper` | Recursos compartilhados: JWT, logs (Serilog), AutoMapper |
| **WebApi** | `CleanShop.WebApi` | Controllers, Swagger, CORS e autenticação |
| **Test** | `Test.UnitTests`, `Test.IntegrationTests` | Testes (MSTest) |

**Tecnologias:** .NET 10 · ASP.NET Core · Dapper · SQL Server · AutoMapper · Serilog · JWT · Swagger (Swashbuckle) · Docker

---

## Configuração (segredos no `.env`)

Senhas, connection strings e a chave do JWT **não ficam no `appsettings.json`**. Eles ficam em um
arquivo `.env` que existe só na sua máquina e está no `.gitignore`.

```bash
cd CleanShop
cp .env.example .env
```

Depois, edite o `.env` e preencha:

| Variável | Uso |
|---|---|
| `MSSQL_SA_PASSWORD` | Senha do usuário `sa` do SQL Server no Docker (senha forte) |
| `ConnectionStrings__NorthwindConnection` | Banco ao rodar localmente (`dotnet run` / Visual Studio): `localhost,1433`, com a mesma senha do `sa` |
| `Jwt__Key` | Chave de assinatura dos tokens (mínimo 32 caracteres, aleatória) |

> `__` (dois underlines) equivale ao `:` do `appsettings.json`. Ex.: `Jwt__Key` = `Jwt:Key`.
> Em produção, defina essas mesmas variáveis no servidor/container (ou use um cofre como Azure Key Vault).

Para gerar uma chave JWT aleatória:

```bash
openssl rand -base64 64
```

---

## Executando localmente (API fora do Docker)

O banco roda no Docker (veja [Banco de dados no Docker](#banco-de-dados-no-docker)). Suba só o SQL Server
e rode a API pelo Visual Studio ou pelo terminal:

```bash
cd CleanShop
docker compose up -d sqlserver sqlserver-init
dotnet run --project WebApi/CleanShop.WebApi --launch-profile https
```

Swagger: <https://localhost:7086/swagger>

---

## Docker

Todos os comandos abaixo são executados na pasta `CleanShop/` (onde estão o `Dockerfile` e o `docker-compose.yml`).

O `docker-compose.yml` sobe três serviços:

| Serviço | O que faz |
|---|---|
| `sqlserver` | SQL Server 2025 com o banco Northwind. Dados salvos no volume `sqlserver-data` |
| `sqlserver-init` | Na primeira subida, restaura o Northwind a partir do backup e encerra |
| `cleanshop-api` | A API, que só inicia depois que o banco está pronto |

### Banco de dados no Docker

O banco é restaurado a partir de `CleanShop/database/backup/Northwind.bak`. Esse arquivo contém dados e
**não vai para o Git**, então cada pessoa gera o seu a partir de um SQL Server que já tenha o Northwind.
Com [sqlcmd](https://learn.microsoft.com/sql/tools/sqlcmd/sqlcmd-utility):

```bash
sqlcmd -S "SEU-SERVIDOR\SQLEXPRESS" -E -C -Q "BACKUP DATABASE Northwind TO DISK='C:\caminho\clean-shop\CleanShop\database\backup\Northwind.bak' WITH COPY_ONLY, INIT"
```

> O backup precisa ser de uma versão igual ou anterior ao SQL Server 2025 (versão da imagem usada).

Na primeira vez que o `docker compose up` roda, o `sqlserver-init` restaura o backup
(script `database/restore-northwind.sql`). Nas próximas vezes, o banco já está no volume e nada é restaurado.

**Conectar pelo SSMS / Azure Data Studio:** servidor `localhost,1433`, autenticação SQL Server,
usuário `sa` e a senha de `MSSQL_SA_PASSWORD` do `.env`.

**Restaurar do zero** (apaga o banco do container e restaura o backup de novo):

```bash
docker compose down -v
docker compose up -d
```

### Com Docker Compose (recomendado)

```bash
# Criar a imagem da API e subir tudo (banco + API) em segundo plano
docker compose up -d --build

# Ver os containers (inclui o sqlserver-init, que fica "Exited (0)" depois de restaurar)
docker compose ps -a

# Acompanhar os logs (de todos ou de um serviço)
docker compose logs -f
docker compose logs -f cleanshop-api

# Parar e remover os containers (o banco continua salvo no volume)
docker compose down

# Apenas parar / iniciar novamente (sem remover)
docker compose stop
docker compose start

# Reiniciar
docker compose restart
```

Swagger: <http://localhost:8080/swagger>

Os logs em arquivo do Serilog ficam em `CleanShop/logs/`, mapeados para `/app/logs` no container.

### Com comandos Docker (sem Compose)

```bash
# Criar a imagem
docker build -t cleanshop-api:latest .

# Executar só a API (lê os segredos do .env)
docker run -d --name cleanshop-api -p 8080:8080 --env-file .env -e ASPNETCORE_ENVIRONMENT=Development cleanshop-api:latest
```

Sem o Compose, o banco precisa estar no ar (`docker compose up -d sqlserver sqlserver-init`), e a connection
string deve apontar para `host.docker.internal,1433`, porque dentro do container `localhost` é o próprio container.
Para isso, sobrescreva com `-e ConnectionStrings__NorthwindConnection="Server=host.docker.internal,1433;..."`.

### Comandos úteis

```bash
# Listar containers em execução
docker ps

# Listar todos os containers (inclusive parados)
docker ps -a

# Listar imagens
docker images

# Ver logs de um container
docker logs -f cleanshop-api

# Parar / iniciar um container
docker stop cleanshop-api
docker start cleanshop-api

# Remover um container (precisa estar parado)
docker rm cleanshop-api

# Remover a imagem
docker rmi cleanshop-api:latest

# Abrir um terminal dentro do container
docker exec -it cleanshop-api sh

# Rodar uma consulta no banco do container
docker exec -it cleanshop-sqlserver /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa -P "<senha do sa>" -Q "SELECT COUNT(*) FROM Northwind.dbo.Customers"

# Listar volumes (o banco fica em cleanshop_sqlserver-data)
docker volume ls

# Limpar containers parados, redes e imagens sem uso
docker system prune
```
