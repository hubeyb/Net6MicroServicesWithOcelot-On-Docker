#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["KloiaCase.ApiGateway/KloiaCase.ApiGateway.csproj", "KloiaCase.ApiGateway/"]
RUN dotnet restore "KloiaCase.ApiGateway/KloiaCase.ApiGateway.csproj"
COPY . .
WORKDIR "/src/KloiaCase.ApiGateway"
RUN dotnet build "KloiaCase.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KloiaCase.ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KloiaCase.ApiGateway.dll"]