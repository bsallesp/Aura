/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          DEFAULT: '#0D9488', // Teal-600 approx
          light: '#2DD4BF',   // Teal-400
          dark: '#0F766E',    // Teal-700
        },
        secondary: {
          DEFAULT: '#F3F4F6', // Gray-100
          dark: '#9CA3AF',    // Gray-400
        },
        accent: {
          DEFAULT: '#D97706', // Amber-600 (Gold-ish)
          light: '#FBBF24',   // Amber-400
        },
        success: '#10B981',   // Emerald-500
        warning: '#F59E0B',   // Amber-500
        error: '#F43F5E',     // Rose-500
        info: '#0EA5E9',      // Sky-500
      },
      fontFamily: {
        sans: ['Inter', 'Roboto', 'sans-serif'],
      },
    },
  },
  plugins: [],
}
