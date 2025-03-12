# Note App Architecture

## Overview

The Note App is a full-stack web application built with ASP.NET Core 8 for the backend and Vue.js for the frontend. It follows a modern, layered architecture that separates concerns and promotes maintainability and scalability.

The application is designed to allow users to create, read, update, and delete notes. It features user authentication, secure API endpoints, and a responsive user interface.

## System Architecture Diagram

```
+-------------------+        +-------------------+
|                   |        |                   |
|  Frontend (Vue.js)|<------>|  Backend (.NET 8) |<------>| Database |
|                   |  HTTP  |                   |   SQL   |          |
+-------------------+        +-------------------+        +----------+
```

## Backend Architecture

The backend follows a layered architecture pattern with the following components:

### Controllers Layer

Controllers handle HTTP requests and responses, defining the API endpoints of the application.

- **AuthController**: Manages user authentication (login, register, logout)
- **NotesController**: Handles CRUD operations for notes

Controllers are responsible for:
- Validating input
- Handling HTTP status codes
- Calling the appropriate service methods
- Returning appropriate responses

### Services Layer

Services contain the business logic of the application.

- **AuthService**: Implements authentication logic including token generation
- **NotesService**: Implements business rules for note operations

Services are responsible for:
- Implementing business rules and logic
- Coordinating between multiple repositories if needed
- Handling cross-cutting concerns
- Ensuring data consistency

### Repositories Layer

Repositories provide an abstraction over data access.

- **UserRepository**: Handles data operations for users
- **NotesRepository**: Handles data operations for notes

Repositories are responsible for:
- CRUD operations against the database
- Translating between domain models and database entities
- Isolating the application from changes in the data source

### Data Access Layer

This layer interacts directly with the database.

- Uses Entity Framework Core as the ORM
- Defines DbContext and entity configurations
- Handles database migrations and schema updates

## Frontend Architecture

The frontend is built with Vue.js and follows a component-based architecture.

### Components

Components are the building blocks of the UI.

- **Layout Components**: Define the overall structure (headers, footers)
- **Note Components**:
  - `ListNote.vue`: Displays a list of notes
  - `NotePanel.vue`: Container for note-related components

- **Auth Components**:
  - `LoginView.vue`: Handles user login
  - `UserView.vue`: Displays user information

### Routing

Routing is handled by Vue Router.

- Defines routes for different pages (login, notes, user profile)
- Implements navigation guards for protected routes
- Handles route parameters (like note IDs)

Key routes:
- `/login`: Login page
- `/notes`: Notes dashboard
- `/user`: User profile

### State Management

State management is implemented using Pinia stores.

- **authStore**: Manages authentication state
  - User information
  - JWT token
  - Authentication status
  - Login/logout functionality

- **noteStore**: Manages notes data
  - CRUD operations for notes
  - Note filtering and sorting
  - Current note selection

## Authentication Flow

1. **Registration**:
   - User submits registration form with username, email, and password
   - Backend validates the input
   - Password is hashed and stored
   - JWT token is generated and returned to the client

2. **Login**:
   - User submits login credentials
   - Backend validates credentials against stored data
   - If valid, a JWT token is generated and returned to the client
   - Frontend stores the token in localStorage and in the Pinia store

3. **Authentication Verification**:
   - For protected routes, the frontend sends the JWT token in the Authorization header
   - Backend middleware validates the token
   - If invalid or expired, returns 401 Unauthorized
   - Frontend redirects to login page if authentication fails

4. **Logout**:
   - Frontend removes the token from localStorage and the Pinia store
   - User is redirected to the login page

## Data Flow

### Create Note Flow

1. User enters note data and clicks Save
2. Frontend component calls the noteStore's createNote action
3. noteStore makes an HTTP POST request to the backend API
4. NotesController receives the request and validates input
5. NotesController calls the NotesService
6. NotesService processes the note and calls the NotesRepository
7. NotesRepository saves the note to the database
8. Response flows back through the layers
9. Frontend updates the UI to show the new note

### Read Notes Flow

1. User navigates to the notes page
2. Frontend router renders the NotesLayout component
3. On mount, the component calls the noteStore's getNotes action
4. noteStore makes an HTTP GET request to the backend API
5. NotesController handles the request
6. NotesController calls the NotesService
7. NotesService calls the NotesRepository
8. NotesRepository retrieves notes from the database
9. Response flows back through the layers
10. Frontend renders the notes in the UI

## Security Considerations

- Authentication uses JWT tokens
- Sensitive data like passwords are hashed
- API endpoints enforce authentication where necessary
- Frontend routes have navigation guards to protect private content
- CORS is configured to allow only specific origins
- Input validation is performed on both frontend and backend

## Deployment Architecture

The application can be deployed in various environments:

- **Development**: Local machines with separate frontend and backend servers
- **Testing**: Integrated environment for QA
- **Production**: Properly secured and optimized for performance

## Future Architectural Considerations

- Implement refresh tokens for better security
- Add WebSocket support for real-time updates
- Implement caching for better performance
- Consider microservices architecture for scaling specific features

