FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["./MangaApi/MangaApi.csproj", "MangaApi/"]
COPY ["./EmailService/EmailingService.csproj", "EmailService/"]
COPY ["./FileService/FileService.csproj", "FileService/"]
COPY ["./Services/Services.csproj", "Services/"]
COPY ["./Data/Data.csproj", "Data/"]
COPY ["./Services.Core/Services.Shared.csproj", "Services.Core/"]
COPY ["./ValidateService/ValidateService.csproj", "ValidateService/"]
COPY ["./WrapperService/WrapperService.csproj", "WrapperService/"]
RUN dotnet restore "MangaApi/MangaApi.csproj"
COPY . .
WORKDIR "/src/MangaApi"
RUN dotnet build "MangaApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MangaApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MangaApi.dll"]