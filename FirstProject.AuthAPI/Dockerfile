FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FirstProject.AuthAPI.csproj", "./"]
RUN dotnet restore "./FirstProject.AuthAPI.csproj"
COPY . .
RUN dotnet build "FirstProject.AuthAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "FirstProject.AuthAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENV ASPNETCORE_Kestrel__Certificates__Default__Password=1234
ENV ASPNETCORE_URLS=https://+:5001;http://+:7001
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx

ENTRYPOINT ["dotnet", "FirstProject.AuthAPI.dll"]
