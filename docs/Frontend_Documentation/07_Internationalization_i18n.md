# Internationalization (i18n)

## Supported Languages
The application is designed to support multiple locales.
- **Default**: English (US) `en-US`
- **Planned**: Portuguese (Brazil) `pt-BR`

## Angular i18n Setup
We use the native **Angular i18n** module for static text translation.
- **Marking Text**: Elements are marked with the `i18n` attribute.
  ```html
  <h1 i18n="@@homePageTitle">Welcome to Aura</h1>
  ```
- **Translation Files**:
    - `src/locale/messages.xlf` (Source)
    - `src/locale/messages.pt.xlf` (Portuguese translation)

## Formatting
Angular pipes are used for locale-sensitive data:
- **Date**: `{{ appointment.date | date:'shortDate' }}` -> `12/25/2025` (US) or `25/12/2025` (BR).
- **Currency**: `{{ service.price | currency }}` -> `$100.00` or `R$ 100,00`.
- **Numbers**: `{{ count | number }}`.

## Adding Translations
1.  Run `ng extract-i18n` to update the source `.xlf` file with new marked strings.
2.  Copy new `trans-unit` blocks to the target language files.
3.  Add translations in the `<target>` tag.

## RTL Support
Currently, RTL (Right-to-Left) is not active. If support for languages like Arabic or Hebrew is required in the future, Tailwind's `rtl:` modifiers and `dir="rtl"` HTML attribute will be utilized.
