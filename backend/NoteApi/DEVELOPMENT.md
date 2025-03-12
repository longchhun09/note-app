# Development Guide

This document provides detailed instructions for setting up and working with the Note App project in a development environment.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Backend Setup (.NET 8)](#backend-setup-net-8)
- [Frontend Setup (Vue.js)](#frontend-setup-vuejs)
- [Database Setup](#database-setup)
- [Debugging Instructions](#debugging-instructions)
- [Testing Procedures](#testing-procedures)
- [Common Development Workflows](#common-development-workflows)

## Prerequisites

Before you begin, ensure you have the following installed on your development machine:

- **Git** - For version control
- **.NET 8 SDK** - For backend development
- **Node.js** (v16.0 or higher) and **npm** - For frontend development
- **Visual Studio Code** (recommended) or other IDE with C# and Vue.js support
  - Recommended extensions:
    - C# for Visual Studio Code (ms-dotnettools.csharp)
    - Vue Language Features (Volar)
    - ESLint
    - Prettier
- **SQL Server** (LocalDB, Express, or Docker instance) - For database development
- **Postman** or similar tool - For API testing

## Backend Setup (.NET 8)

### Clone the Repository

```bash
git clone https://github.com/yourusername/note-app.git
cd note-app
```

### Restore Dependencies and Build

```bash
cd backend/NoteApi
dotnet restore
dotnet build
```

### Configure the Database Connection

1. Open the `appsettings.Development.json` file and update the connection string as needed:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NoteApp;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### Apply Database Migrations

```bash
dotnet ef database update
```

If you need to create a new migration after making changes to models:

```bash
dotnet ef migrations add [MigrationName]
dotnet ef database update
```

### Run the Backend

```bash
dotnet run
```

The API will be available at `http://localhost:5289` by default.

## Frontend Setup (Vue.js)

### Install Dependencies

```bash
cd frontend/note-app
npm install
```

### Configure Environment Variables

Create a `.env.local` file in the frontend directory:

```
VITE_API_URL=http://localhost:5289/api
```

### Run the Development Server

```bash
npm run dev
```

The frontend application will be available at `http://localhost:5173` by default.

## Database Setup

### SQL Server Setup

1. If you're using SQL Server LocalDB (included with Visual Studio):
   - It should be ready to use with the connection string provided in the appsettings.json

2. If you're using SQL Server Express:
   - Install SQL Server Express
   - Update the connection string accordingly

3. If you're using Docker:
   
   ```bash
   docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
   ```
   
   Update the connection string:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost,1433;Database=NoteApp;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True"
   }
   ```

### Seeding Initial Data

The application includes a data seeder that runs automatically when the backend is started. If you want to manually seed data:

```bash
dotnet run seeddata
```

## Debugging Instructions

### Debugging the Backend (.NET)

#### Using Visual Studio Code

1. Open the backend project in VS Code
2. Make sure you have the C# extension installed
3. Use the included launch configurations in `.vscode/launch.json`
4. Set breakpoints in your code
5. Press F5 or use the Debug menu to start debugging

For macOS users facing vsdbg compatibility issues:

```
If you encounter this error on macOS: "Unable to load shared library 'vsdbg'", you have a few options:
1. Use an older version of the C# extension (1.25.0 or earlier)
2. Upgrade your macOS to version 13.0 or higher
3. Use the alternative debugging methods in the launch.json
```

#### Using Visual Studio

1. Open the solution file `note-app.sln`
2. Set breakpoints in your code
3. Press F5 or use Debug > Start Debugging

### Debugging the Frontend (Vue.js)

#### Using Browser DevTools

1. Run the frontend development server with `npm run dev`
2. Open Chrome DevTools (F12 or Right-click > Inspect)
3. Go to the Sources tab
4. Navigate to the webpack:// source
5. Set breakpoints in your Vue components
6. The source maps should allow you to debug TypeScript directly

#### Using VS Code

1. Use the included launch configurations for Chrome debugging
2. Make sure to have the frontend running (`npm run dev`)
3. Set breakpoints in your Vue components
4. Start debugging with F5

## Testing Procedures

### Backend Testing

The backend uses xUnit for testing.

#### Running Backend Tests

```bash
cd backend/NoteApi.Tests
dotnet test
```

#### Writing New Backend Tests

1. Create a new test class in the appropriate directory
2. Follow the existing pattern for tests:

```csharp
public class YourServiceTests
{
    [Fact]
    public async Task YourMethod_ShouldDoSomething_WhenCondition()
    {
        // Arrange
        
        // Act
        
        // Assert
    }
}
```

### Frontend Testing

The frontend uses Vitest and Vue Test Utils for testing.

#### Running Frontend Tests

```bash
cd frontend/note-app
npm run test
```

#### Writing New Frontend Tests

1. Create a `YourComponent.spec.ts` file next to the component you want to test
2. Follow the existing pattern for tests:

```typescript
import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import YourComponent from './YourComponent.vue'

describe('YourComponent', () => {
  it('renders properly', () => {
    const wrapper = mount(YourComponent, {
      props: { /* your props here */ }
    })
    
    expect(wrapper.text()).toContain('Expected text')
  })
})
```

## Common Development Workflows

### Adding a New API Endpoint

1. Identify which controller the endpoint belongs to (or create a new one)
2. Add a new action method to the controller
3. Implement the necessary service methods
4. Update the repository if needed
5. Test the endpoint with Postman

Example:

```csharp
[HttpGet("featured")]
public async Task<IActionResult> GetFeaturedNotes()
{
    try
    {
        var userId = GetUserIdFromClaims();
        var notes = await _noteService.GetFeaturedNotesAsync(userId);
        return Ok(notes);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error retrieving featured notes");
        return StatusCode(500, "An error occurred while retrieving featured notes");
    }
}
```

### Adding a New Vue Component

1. Create a new `.vue` file in the appropriate components directory
2. Implement the component
3. Import and use the component where needed
4. Add tests if required

Example:

```vue
<template>
  <div class="featured-note">
    <h3>{{ note.title }}</h3>
    <p>{{ note.content }}</p>
  </div>
</template>

<script setup lang="ts">
import { defineProps } from 'vue'
import type { Note } from '@/types/Note'

const props = defineProps<{
  note: Note
}>()
</script>
```

### Authentication Workflow

The application uses JWT for authentication:

1. User registers or logs in through the frontend
2. Backend validates credentials and returns a JWT token
3. Frontend stores the token in the auth store
4. Token is sent with subsequent requests in the Authorization header
5. Protected routes check for valid authentication using route guards

Example authentication request with curl:

```bash
curl -X POST \
  http://localhost:5289/api/auth/login \
  -H 'Content-Type: application/json' \
  -d '{
    "username": "your_username",
    "password": "your_password"
}'
```

### Database Changes Workflow

1. Update the model classes in the backend
2. Create a new migration:
   ```bash
   cd backend/NoteApi
   dotnet ef migrations add [MigrationName]
   ```
3. Review the migration code to ensure it's correct
4. Apply the migration:
   ```bash
   dotnet ef database update
   ```
5. Update any affected repositories, services, or controllers

### Git Workflow

1. Create a new branch for your feature:
   ```bash
   git checkout -b feature/your-feature-name
   ```
2. Make your changes, commit them with descriptive messages
3. Push your branch to the remote repository
4. Create a pull request
5. Once reviewed and approved, merge to main
6. Delete the feature branch

### Deployment Workflow

For local deployments (for testing):

1. Build the frontend:
   ```bash
   cd frontend/note-app
   npm run build
   ```
2. Publish the backend:
   ```bash
   cd backend/NoteApi
   dotnet publish -c Release
   ```
3. Host the backend build on IIS, Azure, or another hosting service
4. Deploy the frontend build to a static file hosting service or CDN

