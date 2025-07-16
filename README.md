# ReWow Pet Project

## Overview
ReWow Pet is a full-stack web application for managing pet medical records and services. The project consists of an Angular frontend and an ASP.NET Core 8 backend API, using SQLite as the database engine. The system supports admin authentication with JWT, CRUD for pet owners, pets, vaccinations, and medical services, and a responsive Bootstrap 5 UI.

## Tech Stack
- **Frontend:** Angular 18, Bootstrap 5
- **Backend:** ASP.NET Core 8 Web API
- **Database:** SQLite
- **Authentication:** JWT (JSON Web Token) for admin users

## Features
- Admin login and registration (JWT-based authentication)
- Medical Record management (CRUD for Pet Owners, Pets, Vaccinations, Medical Services)
- Responsive UI with Bootstrap 5
- Protected routes for authenticated users

## Backend Setup
1. **Install dependencies:**
   - .NET 8 SDK
   - SQLite
2. **Configure the database:**
   - The connection string is set in `appsettings.json` as `Data Source=rewow.db`.
3. **Run migrations:**
   ```sh
   dotnet ef database update
   ```
4. **Run the backend:**
   ```sh
   dotnet run --project ReWoWets
   ```
5. **CORS:**
   - CORS is enabled for all origins, headers, and methods by default.
6. **JWT Authentication:**
   - The backend issues JWT tokens on successful login. The secret key is set in `Program.cs`.

## Frontend Setup
1. **Install dependencies:**
   ```sh
   npm install
   ```
2. **Run the frontend:**
   ```sh
   npm start
   ```
3. **API URL:**
   - The frontend expects the backend API at `http://localhost:5019/api` (update in `auth.service.ts` if needed).

## Authentication Flow
- Only admin users can log in and access protected routes (e.g., Medical Record).
- On login, the backend returns a JWT token, which is stored in `localStorage` and sent in the `Authorization` header for all API requests.
- The `/medical-record` route and other sensitive routes are protected by an Angular route guard.

## Main Packages Used
### Frontend (`package.json`)
- `@angular/core`, `@angular/forms`, `@angular/router` (Angular 18)
- `bootstrap` (Bootstrap 5)
- `rxjs`, `zone.js`

### Backend (`Program.cs`)
- `Microsoft.EntityFrameworkCore` (with SQLite provider)
- `Microsoft.AspNetCore.Authentication.JwtBearer` (JWT authentication)
- `Microsoft.IdentityModel.Tokens` (JWT token generation)

## Database
- SQLite is used for persistence. The database file is `rewow.db`.
- Migrations are managed with Entity Framework Core.

## Usage
- Register a new admin or use the seeded admin account.
- Log in as admin to access and manage medical records.
- All API requests from the frontend are authenticated using JWT.

## License
This project is for educational/demo purposes.
