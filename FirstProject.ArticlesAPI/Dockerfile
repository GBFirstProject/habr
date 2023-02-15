FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FirstProject.ArticlesAPI.csproj", "./"]
RUN dotnet restore "./FirstProject.ArticlesAPI.csproj"
COPY . .
RUN dotnet build "FirstProject.ArticlesAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "FirstProject.ArticlesAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENV ASPNETCORE_Kestrel__Certificates__Default__Password=1234
ENV ASPNETCORE_URLS=https://+:5033;http://+:7033
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx

ENTRYPOINT ["dotnet", "FirstProject.ArticlesAPI.dll"]
