# Етап зборки
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["RecipeApi.csproj", "./"]
RUN dotnet restore "RecipeApi.csproj"
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Етап запуску
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
# Рядок нижче говорить серверу слухати порт, який виділить Render
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "RecipeApi.dll"]