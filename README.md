# Cafe Employee Management

This repository contains the source code of two projects of the Cafe Employee Management application.

1. Backend
2. Frontend


To get started, begin by cloning the repository.

```
git clone https://github.com/isurukdniss/Cafe-Employee-Management.git
```

## Backend

The application backend can be found in `/Backend/CafeEmployeeManagement` directory.

The following technologies are used to build the backend,

1. .Net 9
2. Clean Architecture with CQRS and MediatR
3. Autofac for Dependency Injection
4. Automapper for mappings
5. Entity Framework as the ORM
6. PostgreSQL as the Relational Database
7. Fluent Validation for the validations
8. SeriLog for Logging


### Steps to run the program

#### 1. Restore nuget packages
```
dotnet restore
```

#### 2. Run the application using https profile

```
dotnet run --launch-profile https
```

The Application will start running on `https://localhost:7199` url.

### Authentication

All Cafe and Employee endpoints need a JWT bearer token. Log in (or register) to get one, then send it in the `Authorization` header. Any logged-in user can read; only users with the `Admin` role can create, update or delete. In Development, an admin account `admin@cafe.local` / `Admin@12345` is created on startup.

See [Authentication.md](Backend/CafeEmployeeManagement/Authentication.md) for the full details.

### Endpoints

| HTTP Method | Endpoint | Description | Access |
| ------------|----------|-------------|--------|
| POST | /Auth/register | Register a new user (gets the `User` role) and return a token | Anyone |
| POST | /Auth/login | Log in and return a token | Anyone |
| POST | /Auth/assign-role | Assign a role to a user | Admin |
| GET | /Cafe?location="location"| Get all cafes. Supports search by location | Logged in |
| GET | /Cafe/{id}| Get cafe by Id | Logged in |
| POST | /Cafe| Create new Cafe. The input type is FormData | Admin |
| PUT | /Cafe/{id} | Update Cafe by Id. The input type is FormData  | Admin |
| DELETE | /Cafe/{id}| Delete Cafe by Id | Admin |
| GET | /Employee?cafe="cafe_name"| Get all employees. Supports search by cafe name | Logged in |
| GET | /Employee/{id} | Get Employee by Id | Logged in |
| POST | /Employee| Create new Employee | Admin |
| PUT | /Employee/{id} | Update Employee by Id | Admin |
| DELETE | /Employee/{id}| Delete Employee by Id | Admin |

Example usage:
```
curl --location 'https://localhost:7199/api/Auth/login' --header 'Content-Type: application/json' --data '{"email":"admin@cafe.local","password":"Admin@12345"}'

curl --location 'https://localhost:7199/api/Employee' --header 'Authorization: Bearer <token from the login response>'
```
-------

### Open API Documentation
Navigate to the below URL to see the OpenAPI documentation
```
https://localhost:7199/openapi/v1.json
```

>Note: The Swagger / Swagger UI is not available in the project as the .Net 9 has switched to the OpenApi. Please use the above URL for API documentation.

----

## Frontend
The application frontend can be found in `/Frontend/cafe-employee-management-web`

The following technologies are used to build the backend.

1. React
2. Material-ui
3. AgGrid for the grids
4. react-hook-form for the Forms

### Steps to Run the Application
#### 1. Restore node modules
```
npm install
```

### 2. Run the Application
```
npm run dev
```

