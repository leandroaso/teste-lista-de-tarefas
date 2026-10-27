FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ListaDeTarefas.sln ./
COPY Tarefas.Api/Tarefas.Api.csproj Tarefas.Api/
COPY Tarefas.Application/Tarefas.Application.csproj Tarefas.Application/
COPY Tarefas.Domain/Tarefas.Domain.csproj Tarefas.Domain/
COPY Tarefas.Infrastructure/Tarefas.Infrastructure.csproj Tarefas.Infrastructure/

RUN dotnet restore Tarefas.Api/Tarefas.Api.csproj

COPY . .
RUN dotnet publish Tarefas.Api/Tarefas.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Tarefas.Api.dll"]
