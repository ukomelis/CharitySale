# CharitySale Project
## Overview
The CharitySale project is a web application designed to facilitate charity sales events. It consists of:
1. **Backend API** (.NET 9.0 ASP.NET Core API)
2. **Frontend Client** (React 19.0.0 application)

The application allows users to manage items for sale, process transactions, and track sales data for charity events.
## Project Structure
- **CharitySale.Api** - Backend ASP.NET Core API
- **charity-sale-client** - React frontend application
- **CharitySale.Shared** - Shared models and resources

## Technologies Used
### Backend
- .NET 9.0
- ASP.NET Core MVC
- Entity Framework Core with PostgreSQL
- SignalR for real-time updates
- AutoMapper
- Swagger for API documentation

### Frontend
- React 19.0.0
- React Router 7.4.0
- Bootstrap 5.3.3
- Axios 1.8.4
- SignalR client (@microsoft/signalr 8.0.7)
- FontAwesome icons

## Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (for npm)
- [PostgreSQL](https://www.postgresql.org/)
- [Docker](https://www.docker.com/get-started) and [Docker Compose](https://docs.docker.com/compose/install/) (for containerized deployment)

## Getting Started

### Frontend Configuration (.env)
The frontend React application uses the following environment variables:

```json
REACT_APP_API_BASE_URL=http://localhost:8080/api
REACT_APP_SIGNALR_HUB_URL=http://localhost:8080/charitySaleHub
```

### Backend Configuration (`appsettings.json`)
The backend API uses the following configurations:

```json
  "ConnectionStrings": {
    "CharitySaleDb": "Host=db;Port=5432;Database=charitysale;User ID=postgres;Password=Passw0rd123!"
  },
  "ClientApp": {
    "BaseUrl": "http://localhost:3000"
  }
```

### Running API and PostgreSQL with Docker
The project includes Docker configuration for easy deployment of both the API and PostgreSQL database.

Docker will build API image in Release configuration

1. Make sure Docker and Docker Compose are installed on your machine
2. Navigate to the project's root directory (where the docker-compose.yml file is located)
3. Run the following command to build and start the containers:
``` bash
docker-compose up -d
```
This will:
- Build the API container based on the Dockerfile
- Pull the PostgreSQL image
- Start both services
- Configure the database with the provided credentials
- Make the API available on port 8080
- Make the PostgreSQL database available on port 5432

1. Access the API at `http://localhost:8080` and Swagger at `http://localhost:8080/swagger`
2. To stop the containers, run:
``` bash
docker-compose down
```
1. If you want to remove all containers and data volumes, run:
``` bash
docker-compose down -v
```
#### Docker Configuration Details
The docker-compose.yml file defines two services:
1. **api**: The .NET API service
    - Builds from the Dockerfile in the project root
    - Runs on port 8080
    - Depends on the PostgreSQL database
    - Uses Release configuration

2. **db**: PostgreSQL database
    - Uses PostgreSQL 16 image
    - Persists data using Docker volumes

Both services are connected through a Docker bridge network called `charity-network`.

### Running the Frontend Application
1. Navigate to the React client directory
``` bash
cd charity-sale-client
```
1. Install dependencies
``` bash
npm install
```
1. Start the development server
``` bash
npm start
```
The frontend application will start on `http://localhost:3000`

## API Documentation
The API includes Swagger documentation, available at `/swagger` when the API is running. Use this interface to explore available endpoints.
## Features
- Item management (add, edit, delete, view)
- Sales processing
- Category management
- Real-time updates using SignalR
- Responsive frontend interface

## Database Migration
The application automatically applies migrations and seeds initial data on startup through the `DatabaseInitializer.InitializeAsync()` method.