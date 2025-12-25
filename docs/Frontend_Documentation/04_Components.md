# Components

This section details the core reusable components found in `src/app/shared/components`.

## Reusable Components

### 1. `ButtonComponent`
A configurable button wrapper supporting variants, sizes, and loading states.
- **Inputs**:
    - `variant`: 'primary' | 'secondary' | 'outline' | 'danger'
    - `size`: 'sm' | 'md' | 'lg'
    - `loading`: boolean (shows spinner)
    - `disabled`: boolean
- **Outputs**: `onClick` (EventEmitter)

### 2. `CardComponent`
A layout container with consistent padding, shadow, and border radius.
- **Inputs**: `title` (optional string), `footer` (optional template)
- **Usage**: Wraps content in dashboard widgets or list items.

### 3. `StatusBadgeComponent`
Displays entity status (e.g., Appointment Status) with appropriate color coding.
- **Inputs**: `status`: 'Pending' | 'Confirmed' | 'Cancelled' | 'Completed'

### 4. `DataTableComponent`
A generic table component with sorting, pagination, and filtering.
- **Inputs**:
    - `data`: Array<T>
    - `columns`: ColumnDefinition[]
    - `total`: number
- **Outputs**: `onSort`, `onPageChange`

## Lifecycle Hooks
- `ngOnInit`: Initialize data, subscribe to observables.
- `ngOnChanges`: React to input updates (re-calculate derived state).
- `ngOnDestroy`: Unsubscribe from observables (using `Subject.next()` or `takeUntilDestroyed`).

## Example Usage
```html
<app-card title="Appointment Details">
  <div class="info-grid">
    <span>Date: {{ appointment.date | date }}</span>
    <app-status-badge [status]="appointment.status"></app-status-badge>
  </div>
  
  <div class="actions" footer>
    <app-button 
      variant="danger" 
      [loading]="isCancelling"
      (onClick)="cancelAppointment()">
      Cancel
    </app-button>
  </div>
</app-card>
```
