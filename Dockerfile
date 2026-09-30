# ---------- Build ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY TraversalCoreProje.sln ./
COPY EntityLayer/EntityLayer.csproj EntityLayer/
COPY DataAccessLayer/DataAccessLayer.csproj DataAccessLayer/
COPY BusinessLayer/BusinessLayer.csproj BusinessLayer/
COPY TraversalCoreProje/TraversalCoreProje.csproj TraversalCoreProje/
RUN dotnet restore TraversalCoreProje/TraversalCoreProje.csproj
COPY . .
RUN dotnet publish TraversalCoreProje/TraversalCoreProje.csproj -c Release -o /app --no-restore

# ---------- Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_ENVIRONMENT=Production \
    Demo__Enabled=true \
    Demo__ResetOnStartup=true
EXPOSE 8080
# Render/Heroku setzen $PORT – lokal wird 8080 verwendet
CMD ["sh", "-c", "ASPNETCORE_URLS=http://+:${PORT:-8080} dotnet TraversalCoreProje.dll"]
