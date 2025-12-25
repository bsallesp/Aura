# Performance & Optimization

## Lazy Loading
To reduce the Initial Bundle Size, the application is split into feature modules. Routes are loaded only when the user navigates to them.
- `loadChildren` syntax in `app.routes.ts`.
- Critical CSS is inlined; non-critical styles are deferred.

## Change Detection
- **OnPush**: All components use `ChangeDetectionStrategy.OnPush`. This ensures Angular only checks the view when Inputs change or an event occurs, significantly reducing CPU cycles.
- **Async Pipe**: We use the `| async` pipe in templates to handle observables, which automatically handles subscription and unsubscription, avoiding memory leaks.

## Asset Optimization
- **Images**: Served in WebP format where possible.
- **Fonts**: Self-hosted or preconnected Google Fonts to reduce latency.
- **Bundles**: `budget` checks in `angular.json` warn if bundle sizes exceed thresholds (e.g., 2MB initial, 4MB total).

## Caching
- **API**: HTTP GET requests for static resources (Service Types, Configs) are cached via interceptors or RxJS `shareReplay`.
- **Service Worker**: `@angular/pwa` is configured to cache static assets (HTML, CSS, JS) and provide offline capabilities for the basic shell.

## Metrics
We monitor:
- **LCP (Largest Contentful Paint)**: Goal < 2.5s.
- **FID (First Input Delay)**: Goal < 100ms.
- **CLS (Cumulative Layout Shift)**: Goal < 0.1.
