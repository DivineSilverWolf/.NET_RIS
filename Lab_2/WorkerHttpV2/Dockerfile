#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WorkerHttpV2/WorkerHttpV2.csproj", "WorkerHttpV2/"]
COPY ["Md5_Selection/Md5_Selection.csproj", "Md5_Selection/"]
COPY ["MessagesBetweenManagerAndWorker/MessagesBetweenManagerAndWorker.csproj", "MessagesBetweenManagerAndWorker/"]
RUN dotnet restore "./WorkerHttpV2/./WorkerHttpV2.csproj"
COPY . .
WORKDIR "/src/WorkerHttpV2"
RUN dotnet build "./WorkerHttpV2.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WorkerHttpV2.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkerHttpV2.dll"]
