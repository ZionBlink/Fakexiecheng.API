#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Fakexiecheng.API/Fakexiecheng.API.csproj", "Fakexiecheng.API/"]
RUN dotnet restore "Fakexiecheng.API/Fakexiecheng.API.csproj"
COPY . .
WORKDIR "/src/Fakexiecheng.API"
RUN dotnet build "Fakexiecheng.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fakexiecheng.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fakexiecheng.API.dll"]