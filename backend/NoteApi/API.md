# Note App API Documentation

This document provides comprehensive details about the Note App's REST API endpoints, including authentication and note management functionality.

## Base URL

All endpoints are relative to the base URL:

```
http://localhost:5289/api
```

## Authentication

### Register

Creates a new user account.

- **URL**: `/auth/register`
- **Method**: `POST`
- **Auth Required**: No

**Request Body**:
```json
{
  "username": "your_username",
  "email": "your_email@example.com",
  "password": "your_password",
  "confirmPassword": "your_password"
}
```

**Success Response**:
- **Code**: 200 OK
- **Content**:
```json
{
  "token": "jwt_token_here",
  "username": "your_username",
  "userId": 8
}
```

**Error Responses**:
- **Code**: 400 Bad Request
  - Passwords don't match
  - Username already taken
  - Invalid email format
  - Password doesn't meet requirements

### Login

Authenticates a user and returns a JWT token.

- **URL**: `/auth/login`
- **Method**: `POST`
- **Auth Required**: No

**Request Body**:
```json
{
  "username": "your_username",
  "password": "your_password"
}
```

**Success Response**:
- **Code**: 200 OK
- **Content**:
```json
{
  "token": "jwt_token_here",
  "username": "your_username",
  "userId": 8
}
```

**Error Responses**:
- **Code**: 401 Unauthorized
  - Invalid username or password
- **Code**: 500 Internal Server Error
  - Server error

### Logout

Invalidates the current user's authentication token.

- **URL**: `/auth/logout`
- **Method**: `POST`
- **Auth Required**: Yes (JWT Token)

**Headers**:
```
Authorization: Bearer your_jwt_token
```

**Success Response**:
- **Code**: 200 OK
- **Content**:
```json
{
  "message": "Logged out successfully"
}
```

**Error Responses**:
- **Code**: 401 Unauthorized
  - Invalid or expired token

### Check Authentication

Verifies if the current user's authentication token is valid.

- **URL**: `/auth/check`
- **Method**: `GET`
- **Auth Required**: Yes (JWT Token)

**Headers**:
```
Authorization: Bearer your_jwt_token
```

**Success Response**:
- **Code**: 200 OK
- **Content**:
```json
{
  "isAuthenticated": true,
  "username": "your_username",
  "userId": 8
}
```

**Error Responses**:
- **Code**: 401 Unauthorized
  - Invalid or expired token

## Notes

### Get All Notes

Retrieves all notes for the authenticated user.

- **URL**: `/notes`
- **Method**: `GET`
- **Auth Required**: Yes (JWT Token)

**Headers**:
```
Authorization: Bearer your_jwt_token
```

**Success Response**:
- **Code**: 200 OK
- **Content**:
```json
[
  {
    "id": 1,
    "title": "Note Title",
    "content": "Note content goes here",
    "createdAt": "2023-04-01T12:00:00Z",
    "updatedAt": "2023-04-01T12:00:00Z"
  },
  {
    "id": 2,
    "title": "Another Note",
    "content": "More content here",
    "createdAt": "2023-04-02T14:30:00Z",
    "updatedAt": "2023-04-02T14:30:00Z"
  }
]
```

**Error Responses**:
- **Code**: 401 Unauthorized
  - User ID claim not found
- **Code**: 500 Internal Server Error
  - Server error

### Get Note by ID

Retrieves a specific note by its ID.

- **URL**: `/notes/{id}`
- **Method**: `GET`
- **Auth Required**: Yes (JWT Token)
- **URL Parameters**: `id=[integer]` - ID of the note to retrieve

**Headers**:
```
Authorization: Bearer your_jwt_token
```

**Success Response**:
- **Code**: 200 OK
- **Content**:
```json
{
  "id": 1,
  "title": "Note Title",
  "content": "Note content goes here",
  "createdAt": "2023-04-01T12:00:00Z",
  "updatedAt": "2023-04-01T12:00:00Z"
}
```

**Error Responses**:
- **Code**: 401 Unauthorized
  - User ID claim not found
- **Code**: 404 Not Found
  - Note not found
- **Code**: 403 Forbidden
  - Note belongs to another user
- **Code**: 500 Internal Server Error
  - Server error

### Create Note

Creates a new note for the authenticated user.

- **URL**: `/notes`
- **Method**: `POST`
- **Auth Required**: Yes (JWT Token)

**Headers**:
```
Authorization: Bearer your_jwt_token
```

**Request Body**:
```json
{
  "title": "New Note Title",
  "content": "Content for the new note"
}
```

**Success Response**:
- **Code**: 201 Created
- **Content**:
```json
{
  "id": 3,
  "title": "New Note Title",
  "content": "Content for the new note",
  "createdAt": "2023-04-03T09:15:00Z",
  "updatedAt": "2023-04-03T09:15:00Z"
}
```

**Error Responses**:
- **Code**: 401 Unauthorized
  - User ID claim not found
- **Code**: 400 Bad Request
  - Invalid note data
- **Code**: 500 Internal Server Error
  - Server error

### Update Note

Updates an existing note.

- **URL**: `/notes/{id}`
- **Method**: `PUT`
- **Auth Required**: Yes (JWT Token)
- **URL Parameters**: `id=[integer]` - ID of the note to update

**Headers**:
```
Authorization: Bearer your_jwt_token
```

**Request Body**:
```json
{
  "title": "Updated Note Title",
  "content": "Updated content for the note"
}
```

**Success Response**:
- **Code**: 200 OK
- **Content**:
```json
{
  "id": 1,
  "title": "Updated Note Title",
  "content": "Updated content for the note",
  "createdAt": "2023-04-01T12:00:00Z",
  "updatedAt": "2023-04-03T15:45:00Z"
}
```

**Error Responses**:
- **Code**: 401 Unauthorized
  - User ID claim not found
- **Code**: 404 Not Found
  - Note not found
- **Code**: 403 Forbidden
  - Note belongs to another user
- **Code**: 400 Bad Request
  - Invalid note data
- **Code**: 500 Internal Server Error
  - Server error

### Delete Note

Deletes a specific note.

- **URL**: `/notes/{id}`
- **Method**: `DELETE`
- **Auth Required**: Yes (JWT Token)
- **URL Parameters**: `id=[integer]` - ID of the note to delete

**Headers**:
```
Authorization: Bearer your_jwt_token
```

**Success Response**:
- **Code**: 204 No Content

**Error Responses**:
- **Code**: 401 Unauthorized
  - User ID claim not found
- **Code**: 404 Not Found
  - Note not found
- **Code**: 403 Forbidden
  - Note belongs to another user
- **Code**: 500 Internal Server Error
  - Server error

## Error Response Format

All error responses follow this format:

```json
{
  "message": "Description of the error",
  "statusCode": 400
}
```

## Authentication

All protected endpoints require a valid JWT token in the Authorization header:

```
Authorization: Bearer your_jwt_token
```

The token is obtained from the login or register endpoints and should be included in all subsequent requests to authenticated endpoints.

