# Architecture

## Angular Architecture
The application follows a modular architecture emphasizing separation of concerns and reusability. We utilize **Standalone Components** (Angular 15+) to reduce boilerplate and improve tree-shaking.

### Core Building Blocks
- **Components**: The fundamental UI building blocks. Smart components (containers) handle data fetching and state, while Dumb components (presentational) handle rendering and user interaction.
- **Services**: Singleton classes for business logic, API communication, and state management.
- **Directives**: Custom behaviors attached to DOM elements (e.g., permissions, intersection observers).
- **Pipes**: Pure functions for data transformation (e.g., currency formatting, date localization).

## State Management
We employ a hybrid approach to state management:
- **Local State**: Angular Signals or `BehaviorSubject` within services for component-level state.
- **Global State**: **NgRx** (Store, Effects, Selectors) for complex, shared state such as:
    - User Authentication (Session)
    - Shopping Cart / Booking Flow
    - Notification/Toast System
- **Facade Pattern**: Components interact with state via Facade services to decouple UI from the state management library.

## Routing and Modules
The application uses the Angular Router with **Lazy Loading** to optimize initial load time.
- `app.routes.ts`: Main routing configuration.
- Feature Routes: Each major feature (e.g., `/booking`, `/dashboard`, `/admin`) is lazy-loaded.
- **Guards**: `AuthGuard`, `RoleGuard` (RBAC) protect routes based on user authentication and roles.

## Project Organization
The folder structure follows a feature-first approach:

```
src/
  app/
    core/               # Singleton services, interceptors, guards, global models
    shared/             # Reusable UI components, pipes, directives
    features/           # Feature modules (Lazy loaded)
      auth/
      booking/
      dashboard/
      services/
    layout/             # Main layout components (Header, Sidebar, Footer)
    store/              # Global NgRx state (Actions, Reducers, Effects)
  assets/               # Images, fonts, i18n files
  environments/         # Configuration for Dev, Stage, Prod
```

## Design Patterns & Coding Standards
- **Smart/Dumb Components**: Separation of logic and view.
- **OnPush Strategy**: All components use `ChangeDetectionStrategy.OnPush` for performance.
- **Strict Typing**: TypeScript `strict` mode is enabled. No `any`.
- **DRY (Don't Repeat Yourself)**: Common logic moved to services or utility functions.
