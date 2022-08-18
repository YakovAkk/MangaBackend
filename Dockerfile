#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Data/Data.csproj", "Data/"]
COPY ["MangaBackend/MangaBackend.csproj", "MangaBackend/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["Repositories/Repositories.csproj", "Repositories/"]
RUN dotnet restore "MangaBackend/MangaBackend.csproj"
COPY /logs /src/logs
COPY . .
WORKDIR "/src/MangaBackend"
RUN dotnet build "MangaBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MangaBackend.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MangaBackend.dll"]