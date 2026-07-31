# Build context = repository root (Render default when Dockerfile path is Backend/IMSBackend/Dockerfile)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY Backend/IMSBackend/IMSBackend.csproj Backend/IMSBackend/
RUN dotnet restore Backend/IMSBackend/IMSBackend.csproj
COPY Backend/IMSBackend/ Backend/IMSBackend/
RUN dotnet publish Backend/IMSBackend/IMSBackend.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
# Render sets PORT at runtime; Program.cs binds 0.0.0.0:$PORT
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "IMSBackend.dll"]
