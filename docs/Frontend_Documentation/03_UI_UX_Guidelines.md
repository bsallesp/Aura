# UI/UX Guidelines

## Design System
Aura utilizes a custom design system built on top of **Tailwind CSS**. The design language focuses on cleanliness, trust, and ease of use, suitable for a medical/aesthetic environment.

## Color Palette
- **Primary**: Teal/Aqua variants (calm, medical trust).
- **Secondary**: Soft Greys and Whites (cleanliness).
- **Accent**: Gold/Bronze (elegance, premium feel).
- **Status Colors**:
    - Success: Emerald Green
    - Warning: Amber
    - Error: Rose Red
    - Info: Sky Blue

## Typography
- **Font Family**: `Inter` or `Roboto` for excellent readability.
- **Hierarchy**:
    - `h1`: Page Titles (24px - 32px)
    - `h2`: Section Headers (20px - 24px)
    - `body`: Standard text (14px - 16px)
    - `caption`: Auxiliary text (12px)

## Component Usage
- **Buttons**:
    - Primary: Filled, for main actions (e.g., "Book Now").
    - Secondary: Outlined, for cancellations or secondary options.
    - Ghost: Text-only, for navigation links within cards.
- **Forms**:
    - Floating labels for modern look.
    - Inline validation messages (red text below input).
- **Modals**: Used for quick actions (confirmations, simple edits) to preserve context.

## Accessibility (a11y)
- **Contrast**: All text must meet WCAG AA contrast ratios.
- **Keyboard Navigation**: All interactive elements must be focusable and actionable via keyboard.
- **ARIA Labels**: Used on icon-only buttons and complex custom widgets.
- **Screen Readers**: Semantic HTML (`<main>`, `<nav>`, `<article>`) used throughout.
