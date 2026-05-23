# Project & Task Management API

Welcome to the **Project & Task Management API**. This is a robust, scalable, and secure backend system built using modern enterprise architecture patterns in .NET Core.

---

## 🚨 CRITICAL NOTES (READ BEFORE TESTING)

### 1. Security & Authorization Mechanism
This system implements an advanced **Permission-Based Authorization** coupled with a **Dynamic RBAC (Role-Based Access Control) System**. 
* **How it works:** Policies are dynamically built at runtime based on granular permissions assigned to roles, rather than using hardcoded role checks.

### 2. Testing the Endpoints (Authentication Required)
* ⚠️ **Regular Users:** Newly registered or standard users **DO NOT** have permissions to perform actions (Create, Edit, Delete, etc.) by default. If you try to access the endpoints with a regular user token, you will receive a `403 Forbidden` response formatted via our custom middleware.
* 🔑 **To properly test the API actions**, you **MUST** log in using the pre-seeded Super Admin credentials below, which possess full granular permissions:

| Field | Seeded Admin Credentials |
| :--- | :--- |
| **Email** | `Admin@Admin.com` |
| **Password** | `P00000` |

---

## 🛠️ Project Deliverables & Scope Commitment

* **Task Compliance:** I have fully adhered to all the core software engineering requirements specified in the task challenge.
* **Postman Collection Notice:** Please note that a **Postman Collection was not available or provided** as part of this submission.
* **Swagger Documentation Alternative:** To ensure a seamless testing experience, **Swagger UI has been fully configured and integrated**. You can run the project and it will serve as your primary interactive testing ground.

---

## 🏗️ Architectural Patterns & Tech Stack

This project is built following **Clean Architecture** principles and incorporates the following patterns and tools:

### 1. CQRS (Command Query Responsibility Segregation)
Separation of concerns between Read and Write operations using **MediatR**. This ensures high performance, maintainability, and clean decoupling of business logic.

### 2. Database & Persistence
* **SQL Server:** Used as the primary relational enterprise database.
* **Entity Framework Core:** Utilized as the ORM with explicit configurations.
* **Data Seeding:** Built-in automatic seeding for the Super Admin user, assigning them full roles and operational permissions upon database initialization.

### 3. Advanced MediatR Pipeline Behaviors
* **Validation Pipeline:** Automatically intercepting incoming requests to validate them before hitting the handlers.
* **Transaction Pipeline:** Wrapping write operations (Commands) automatically inside an isolated database transaction block. If any step fails, changes are completely rolled back to maintain data integrity.

### 4. Validation & Error Handling
* **FluentValidation:** Used for elegant, strongly-typed request validation (ensuring clean data entry inputs).
* **GlobalExceptionHandlerMiddleware:** A centralized custom middleware that catches any unhandled exceptions project-wide and formats them into a clean, uniform JSON response object (`ApiResponse`).
* **Forbidden Handler (`IAuthorizationMiddlewareResultHandler`):** Intercepts authorization failures to return a clean customized JSON error body on `403 Forbidden` states instead of generic blank pages.

### 5. API Optimization
* **Server-Side Pagination:** Implemented on listing endpoints to handle large datasets efficiently, preventing memory overhead and reducing response payload sizes.

---

## 🚀 How to Run and Test

1. **Update Connection String:** Check `appsettings.json` and ensure the `DefaultConnection` points to your local SQL Server instance.
2. **Run Migrations & Seed:** The system is configured to apply migrations and seed the `Admin@Admin.com` user automatically on startup.
3. **Launch:** Run the project via CLI `dotnet run` or click the Start/Run button in Visual Studio.
4. **Explore Swagger:** Upon running the application, **it will automatically redirect and open the Swagger UI page in your default browser**. If it doesn't open automatically for any reason, you can navigate manually to: `https://localhost:{port}/swagger/index.html`.
5. **Authorize:** 
   * Call the login endpoint with the Admin credentials listed in the critical notes section above.
   * Copy the generated `accessToken` from the response.
   * Click the **Authorize** button at the top of the Swagger page, paste the token, and unlock all endpoints for testing.
