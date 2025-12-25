# Troubleshooting

## Common Issues

### 1. "CORS Error" in Console
- **Cause**: Frontend (localhost:4200) trying to access Backend (localhost:5000) without allowed origin.
- **Fix**: Update the `.NET` backend CORS policy to allow `http://localhost:4200`.

### 2. Changes not reflecting in Browser
- **Cause**: Browser caching or HMR failure.
- **Fix**: Hard refresh (`Ctrl+F5`) or restart `ng serve`.

### 3. "NullInjectorError: No provider for X"
- **Cause**: A service is being injected but not provided in `root` or the module.
- **Fix**: Ensure `@Injectable({ providedIn: 'root' })` is present or add to `providers` array in `app.config.ts`.

### 4. Styles not applying
- **Cause**: View Encapsulation or Tailwind configuration.
- **Fix**: Check if `ViewEncapsulation.None` is needed or if the class is safe-listed in Tailwind config.

## Debugging Tips
- **Angular DevTools**: Install the Chrome extension to inspect the component tree and change detection cycles.
- **Redux DevTools**: Use to inspect NgRx state changes and time-travel debug.
- **Network Tab**: Always check the "Network" tab in DevTools to verify API payloads and response headers (especially Auth tokens).

## FAQs
**Q: How do I add a new page?**
A: Generate a component (`ng g c features/my-page`), add a route in `app.routes.ts`, and link it in the navigation.

**Q: Where do I update the API URL?**
A: In `src/environments/environment.ts` (for local) and `environment.prod.ts` (for production).
