FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY TaskManagementSystem.TaskService.sln ./
COPY TaskManagementSystem.TaskService/TaskManagementSystem.TaskService.csproj TaskManagementSystem.TaskService/
COPY TaskManagementSystem.SharedLib/TaskManagementSystem.SharedLib.csproj       TaskManagementSystem.SharedLib/
COPY TaskManagementSystem.GrpcLib/TaskManagementSystem.GrpcLib.csproj           TaskManagementSystem.GrpcLib/

RUN dotnet restore TaskManagementSystem.TaskService.sln

COPY . .

RUN dotnet restore TaskManagementSystem.TaskService/TaskManagementSystem.TaskService.csproj

RUN dotnet publish TaskManagementSystem.TaskService/TaskManagementSystem.TaskService.csproj \
    -c Release -o /app/out

# ---------------- runtime ----------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "TaskManagementSystem.TaskService.dll"]
