# API Integration

## Overview
The Angular frontend communicates with the .NET backend via RESTful APIs. All API endpoints are defined in `src/environments/environment.ts` (Base URL) and specific service files.

## Authentication & Authorization
- **JWT**: The backend issues a JSON Web Token (JWT) upon login.
- **Storage**: Tokens are stored in `localStorage` (or `HttpOnly` cookies if configured).
- **Request Flow**: The `AuthInterceptor` attaches the token to the `Authorization` header of every outgoing request to the API domain.

## HTTP Client Usage
We use Angular's `HttpClient` with RxJS observables.

```typescript
getAppointments(): Observable<Appointment[]> {
  return this.http.get<Appointment[]>(`${this.apiUrl}/appointments`).pipe(
    retry(2), // Retry failed requests twice
    catchError(this.handleError) // Centralized error handling
  );
}
```

## Error Handling
- **Global Handling**: `GlobalErrorHandler` implements `ErrorHandler` to log unknown runtime errors to a logging service (e.g., Sentry).
- **API Errors**: The `ErrorInterceptor` transforms backend error responses into a standardized format for the UI to display.

## Caching Strategies
- **HttpCachingInterceptor**: (Optional) Can cache GET requests for static data (e.g., list of services) for a short duration.
- **RxJS `shareReplay`**: Used in services to cache Observables for data that doesn't change often, preventing multiple API calls when multiple components subscribe.
