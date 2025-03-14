# Note App

A modern, full-stack note-taking application built with .NET 8 and Vue.js.

![Note App Screenshot](docs/screenshots/app-screenshot.png)

## Features

- **User Authentication**: Secure sign-up, login, and token-based authentication
- **Note Management**: Create, read, update, and delete personal notes
- **Responsive Design**: Works on desktop and mobile devices
- **Real-time Validation**: Instant feedback on user inputs
- **Secure API**: JWT-based API with proper authorization

## Tech Stack

### Backend
- **.NET 8**: Latest .NET framework for building modern, high-performance APIs
- **Entity Framework Core**: ORM for database interactions
- **ASP.NET Core Identity**: Authentication and authorization
- **Swagger/OpenAPI**: API documentation and testing
- **Microsoft SQL Server**: Database storage

### Frontend
- **Vue.js 3**: Progressive JavaScript framework with Composition API
- **Pinia**: State management for Vue applications
- **Vue Router**: Navigation and routing
- **Tailwind CSS**: Utility-first CSS framework for styling
- **Axios**: HTTP client for API requests

## Architecture Overview

The application follows a modern client-server architecture:

```
┌─────────────────┐       ┌─────────────────┐       ┌─────────────────┐
│                 │       │                 │       │                 │
│    Vue.js       │◄─────►│    ASP.NET      │◄─────►│   SQL Server    │
│    Frontend     │  API  │    Backend      │  EF   │   Database      │
│                 │       │                 │       │                 │
└─────────────────┘       └─────────────────┘       └─────────────────┘
```

For more details, see the [Architecture Documentation](ARCHITECTURE.md).

## Setup Instructions

### Prerequisites
- [.NET SDK 8.0](https://dotnet.microsoft.com/download) or later
- [Node.js](https://nodejs.org/) (v16 or later)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)

### Backend Setup

1. Navigate to the backend directory:
   ```
   cd backend/NoteApi
   ```

2. Restore the .NET packages:
   ```
   dotnet restore
   ```

3. Update the database connection string in `appsettings.json` or `appsettings.Development.json`

4. Apply migrations to create the database:
   ```
   dotnet ef database update
   ```

5. Local configuration

   ```
   mv backend/NoteApi/Properties/sample-launchSettings.json backend/NoteApi/Properties/launchSettings.json 
   ```

6. Run the application:
   ```
   dotnet run --launch-profile NoteApi
   ```

The API will be available at `http://localhost:5289`.

### Local Database Setup with Docker

1. Pull ms sql
```
   docker pull mcr.microsoft.com/mssql/server:2022-latest
```
2. Create a new container
```
   docker run -d --name sqlserver \
      -e "ACCEPT_EULA=Y" \
      -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd" \
      -p 1433:1433 \
      mcr.microsoft.com/mssql/server:2022-latest
```
3. Create database
```
docker exec sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd -Q "CREATE DATABASE NoteDb"
```

4. Create user then assign to db
```
docker exec sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd -Q "CREATE DATABASE NoteDb" -C
```

5. Stop or start container
```
docker stop|start sqlserver
```
### Frontend Setup

1. Navigate to the frontend directory:
   ```
   cd frontend/note-app
   ```

2. Install the dependencies:
   ```
   npm install
   ```

3. Create a `.env` file with the following content:
   ```
   VITE_API_URL=http://localhost:5289/api
   ```

4. Start the development server:
   ```
   npm run dev
   ```

The application will be available at `http://localhost:5173`.

## Running the Application

1. Start the backend server (if not already running):
   ```
   cd backend/NoteApi
   dotnet run
   ```

2. Start the frontend development server (in a separate terminal):
   ```
   cd frontend/note-app
   npm run dev
   ```

3. Open your browser and navigate to `http://localhost:5173`

4. Register a new account or log in with existing credentials

## Screenshots

*Screenshots will be added soon.*

## API Documentation

The API documentation is available via Swagger when the backend is running:

- Development: `http://localhost:5289/swagger`

For a detailed description of all endpoints, see the [API Documentation](API.md).


## Remaining bugs

- [FE] Click view note then click new note. Not show new note form
- [FE] Click reload after login still show another check layout
- [FE] Click edit note on the list show view note
- [FE] Update note title,content should use trim

- [BE] Dealing with return null in create a record
- [BE] Refactor confusing naming on dbconnection

## Impovement

- [BE][AppSetting] Controlling env for production and local run
- [BE][Dapper] Learng how to organize large scale query


## Contributing

Contributions are welcome! Please see the [Contributing Guidelines](CONTRIBUTING.md) for more information.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

