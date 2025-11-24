# Stock Market Web API

## Overview
This project is a **.NET 7 Web API** designed for managing stocks, portfolios, and comments. It includes **JWT-based authentication**, **ASP.NET Identity** for user management, and **Swagger/OpenAPI** for API documentation.

## Features
- CRUD operations for Stocks, Portfolios, and Comments
- User authentication and authorization using **JWT**
- ASP.NET Identity integration for user management
- Swagger UI for API testing and documentation
- Entity Framework Core with SQL Server

## Technologies Used
- .NET 7
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- ASP.NET Identity
- JWT Authentication
- Swagger / OpenAPI

## Project Structure
- **Data Layer**: `ApplicationDBContext` for EF Core
- **Repositories**: `IStockRepository`, `ICommentRepository`, `IPortfolioRepository`
- **Services**: `IStockService`, `ICommentService`, `IPortfolioService`, `ITokenService`
- **Models**: `AppUser`, `Stock`, `Comment`, `Portfolio`

## Setup Instructions
1. **Clone the repository**:
   ```bash
   git clone <your-repo-url>
   cd <your-project-folder>
   ```

2. **Update the connection string** in `appsettings.json`:
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=<your-server>;Database=StockMarket;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

3. **Configure JWT settings** in `appsettings.json`:
   ```json
   "Jwt": {
       "Issuer": "your-issuer",
       "Audience": "your-audience",
       "Key": "your-secret-key"
   }
   ```

4. **Apply migrations and update the database**:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. **Run the application**:
   ```bash
   dotnet run
   ```

## Authentication
- The API uses **JWT Bearer tokens**.
- Obtain a token by calling the login endpoint.
- Include the token in the `Authorization` header:
   ```http
   Authorization: Bearer <your-token>
   ```

## Swagger Usage
- Swagger UI is enabled in Development environment.
- Access it at:
   ```
   https://localhost:<port>/swagger
   ```

## Security
- Passwords require digits (configured in Identity options).
- JWT tokens are validated for issuer, audience, lifetime, and signing key.

## License
This project is licensed under the MIT License.


## Example API Endpoints

### 1. Register a New User
**POST** `/api/account/register`
```http
Request:
POST /api/account/register
Content-Type: application/json
{
  "username": "john_doe",
  "email": "john@example.com",
  "password": "Password123!"
}

Response:
201 Created
{
  "message": "User registered successfully"
}
```

### 2. Login and Get JWT Token
**POST** `/api/account/login`
```http
Request:
POST /api/account/login
Content-Type: application/json
{
  "username": "john_doe",
  "password": "Password123!"
}

Response:
200 OK
{
  "token": "<jwt-token>",
  "expiresIn": 3600
}
```

### 3. Get All Stocks
**GET** `/api/stocks`
```http
Request:
GET /api/stocks
Authorization: Bearer <jwt-token>

Response:
200 OK
[
  {
    "id": 1,
    "symbol": "AAPL",
    "price": 150.25
  },
  {
    "id": 2,
    "symbol": "MSFT",
    "price": 310.10
  }
]
```

### 4. Add a Comment to a Stock
**POST** `/api/comments`
```http
Request:
POST /api/comments
Authorization: Bearer <jwt-token>
Content-Type: application/json
{
  "stockId": 1,
  "title": "Great Stock!",
  "content": "Apple is performing well this quarter."
}

Response:
201 Created
{
  "id": 10,
  "stockId": 1,
  "title": "Great Stock!",
  "content": "Apple is performing well this quarter.",
  "appUserId": "<user-id>"
}
```


## Docker Setup

You can run this API using Docker for easier deployment.

### 1. Create a Dockerfile
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["api.csproj", "."]
RUN dotnet restore "api.csproj"
COPY . .
RUN dotnet build "api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api.dll"]
```

### 2. Build and Run the Docker Image
```bash
docker build -t stock-api .
docker run -d -p 8080:80 --name stock-api-container stock-api
```

Access the API at:
```
http://localhost:8080/swagger
```
