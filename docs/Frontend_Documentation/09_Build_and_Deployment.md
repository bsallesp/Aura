# Build & Deployment

## Build Process
The application is built using the Angular CLI.
- **Development**: `ng serve` (In-memory, hot-reload).
- **Production**: `ng build --configuration production`
    - AOT (Ahead-of-Time) Compilation.
    - Minification and Uglification.
    - Hashing file names for cache busting.

## Environment Configuration
Managed via `src/environments/`:
- `environment.ts`: Development settings (localhost API).
- `environment.prod.ts`: Production settings (live API).
- **Variables**: `apiUrl`, `stripeKey`, `production` flag.

## CI/CD Pipeline
We use **GitHub Actions** (or Azure DevOps) for continuous integration and deployment.
1.  **Checkout Code**.
2.  **Install Dependencies**: `npm ci`.
3.  **Lint & Test**: `ng lint`, `ng test --watch=false`.
4.  **Build**: `ng build`.
5.  **Deploy**:
    - **Azure Static Web Apps** (recommended) or **AWS S3 + CloudFront**.
    - Artifacts from `dist/` are uploaded to the hosting provider.

## Deployment Instructions
1.  Ensure all environment variables are set in the CI secrets.
2.  Push to `main` branch triggers the production deployment workflow.
3.  Verify the deployment URL (e.g., `https://aura-app.com`).
