# Services & Dependency Injection

## DI Structure
Angular's Dependency Injection (DI) system is used to provide services to components and other services. We prefer **`providedIn: 'root'`** for most services to ensure they are tree-shakable singletons.

## Core Services

### 1. `AuthService`
Handles user authentication logic.
- **Methods**: `login()`, `register()`, `logout()`, `refreshToken()`.
- **State**: Exposes `currentUser$` and `isAuthenticated$` observables.

### 2. `ApiService`
A generic wrapper around `HttpClient` to standardize API calls.
- **Methods**: `get<T>()`, `post<T>()`, `put<T>()`, `delete<T>()`.
- **Responsibility**: Appends base URL, handles generic serialization.

### 3. `AppointmentService`
Business logic for appointments.
- **Methods**: `getSlots()`, `bookAppointment()`, `cancelAppointment()`.

### 4. `NotificationService`
Manages global toast notifications.
- **Methods**: `success()`, `error()`, `info()`.

## Scoped Services
For feature-specific state that should be destroyed when the feature module is unloaded (e.g., a complex multi-step wizard), services are provided in the component's `providers` array or the feature route configuration.

## Interceptors
HTTP Interceptors are registered in `app.config.ts`.
1.  **AuthInterceptor**: Appends `Authorization: Bearer <token>` to requests.
2.  **ErrorInterceptor**: Catches 401/403 errors (redirects to login) and formats other errors.
3.  **LoadingInterceptor**: Toggles a global loading spinner for long-running requests.
