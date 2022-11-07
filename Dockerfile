#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
PUB

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Turnup/Turnup.csproj", "Turnup/"]
RUN dotnet restore "Turnup/Turnup.csproj"
COPY . .
WORKDIR "/src/Turnup"
RUN dotnet build "Turnup.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Turnup.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Turnup.dll"]