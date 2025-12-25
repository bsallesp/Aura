# Testing

## Strategy
We maintain a high standard of code quality through multiple layers of testing.

## Unit Tests
- **Framework**: **Jasmine** (syntax) and **Karma** (runner).
- **Scope**: Individual components, services, pipes, and utility functions.
- **Coverage Goal**: >80% line coverage for business logic.
- **Mocking**: `SpyObj` is used to mock dependencies (e.g., `ApiService`) to isolate the unit under test.

```typescript
it('should create the app', () => {
  const fixture = TestBed.createComponent(AppComponent);
  const app = fixture.componentInstance;
  expect(app).toBeTruthy();
});
```

## Integration Tests
- **Scope**: Interactions between parent/child components and state management (NgRx).
- **Tools**: Angular `TestBed`.

## End-to-End (E2E) Tests
- **Framework**: **Cypress** (preferred over Protractor for modern Angular).
- **Scope**: Critical user journeys (e.g., "User can login", "User can book an appointment").
- **Environment**: Runs against a staging environment with a seeded database.

## Running Tests
- Unit: `ng test`
- E2E: `ng e2e` (or `npx cypress open`)
- CI: Tests run automatically on PR creation via GitHub Actions.
