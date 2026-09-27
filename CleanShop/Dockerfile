# ---------- Build ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia só os .csproj primeiro para aproveitar o cache do restore entre builds
COPY Application/CleanShop.Application.DTO/CleanShop.Application.DTO.csproj Application/CleanShop.Application.DTO/
COPY Application/CleanShop.Application.Interface/CleanShop.Application.Interface.csproj Application/CleanShop.Application.Interface/
COPY Application/CleanShop.Application.Main/CleanShop.Application.Main.csproj Application/CleanShop.Application.Main/
COPY Application/CleanShop.Application.Validator/CleanShop.Application.Validator.csproj Application/CleanShop.Application.Validator/
COPY Domain/CleanShop.Domain.Core/CleanShop.Domain.Core.csproj Domain/CleanShop.Domain.Core/
COPY Domain/CleanShop.Domain.Entity/CleanShop.Domain.Entity.csproj Domain/CleanShop.Domain.Entity/
COPY Domain/CleanShop.Domain.Interface/CleanShop.Domain.Interface.csproj Domain/CleanShop.Domain.Interface/
COPY Infrastructure/CleanShop.Infrastructure.Data/CleanShop.Infrastructure.Data.csproj Infrastructure/CleanShop.Infrastructure.Data/
COPY Infrastructure/CleanShop.Infrastructure.Interface/CleanShop.Infrastructure.Interface.csproj Infrastructure/CleanShop.Infrastructure.Interface/
COPY Infrastructure/CleanShop.Infrastructure.Repository/CleanShop.Infrastructure.Repository.csproj Infrastructure/CleanShop.Infrastructure.Repository/
COPY Transversal/CleanShop.Transversal.Common/CleanShop.Transversal.Common.csproj Transversal/CleanShop.Transversal.Common/
COPY Transversal/CleanShop.Transversal.Logging/CleanShop.Transversal.Logging.csproj Transversal/CleanShop.Transversal.Logging/
COPY Transversal/CleanShop.Transversal.Mapper/CleanShop.Transversal.Mapper.csproj Transversal/CleanShop.Transversal.Mapper/
COPY WebApi/CleanShop.WebApi/CleanShop.WebApi.csproj WebApi/CleanShop.WebApi/
RUN dotnet restore WebApi/CleanShop.WebApi/CleanShop.WebApi.csproj

COPY . .
RUN dotnet publish WebApi/CleanShop.WebApi/CleanShop.WebApi.csproj -c Release -o /app/publish --no-restore /p:UseAppHost=false

# ---------- Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Pasta dos logs do Serilog (arquivo) com permissão para o usuário sem privilégios da imagem
RUN mkdir -p /app/logs && chown $APP_UID /app/logs

COPY --from=build /app/publish .

USER $APP_UID
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CleanShop.WebApi.dll"]
