import defaultTheme from "tailwindcss/defaultTheme";

/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./src/**/**/*.{vue,js,ts,jsx,tsx}",
  ],
  darkMode: 'class',
  theme: {
    extend: {
      fontFamily: {
        primary: "Inter",
        sans: ["Inter", ...defaultTheme.fontFamily.sans],
      },
      fontSize: {
        // desktop or laptop font sizes
        "d-p-s": ["1rem", "1.2"], //16px
        "d-p": ["1.12rem", "1.2"], //18px
        "d-h4": ["1.12rem", "1.2"], //18px
        "d-h3": ["1.25rem", "1.5"], //20px
        "d-h2": ["1.5rem", "1.5"], //24px
        "d-h1": ["1.5rem", "1.5"], //24px

        // mobile and tablet font sizes//7px
        "m-p-s": ["0.625rem", "1.5"], //10px
        "m-p": ["0.81rem", "1.5"], //13
        "m-h4": ["0.81rem", "1.5"], //13px
        "m-h3": ["1.12rem", "1.5"], //18px
        "m-h2": ["1.25rem", "1.5"], //20px
        "m-h1": ["1.25rem", "1.5"], //20px
      },
      screens: {
        sm: "600px",
      },
      colors: {
        "primary-dark": "#004B73",
        "primary-light": "#0081C2",
        primary: "#065886",
        "primary-70": "#004B73",
        "primary-30": "#0081C2",
        secondary: "#252563",
        accent: "#627251",
        neutral: "#20201F",

        // Indigo colors
        indigo: {
          50: '#EEF2FF',
          100: '#E0E7FF',
          200: '#C7D2FE',
          300: '#A5B4FC',
          400: '#818CF8',
        },
        transitionProperty: {
          'default': 'color, background-color, border-color, text-decoration-color, fill, stroke, opacity, box-shadow, transform, filter, backdrop-filter',
          'colors': 'color, background-color, border-color, text-decoration-color, fill, stroke',
          'opacity': 'opacity',
          'shadow': 'box-shadow',
          'transform': 'transform',
        },
        transitionTimingFunction: {
          'default': 'cubic-bezier(0.4, 0, 0.2, 1)',
          'linear': 'linear',
          'in': 'cubic-bezier(0.4, 0, 1, 1)',
          'out': 'cubic-bezier(0, 0, 0.2, 1)',
          'in-out': 'cubic-bezier(0.4, 0, 0.2, 1)',
        },
        transitionDuration: {
          'default': '150ms',
          '75': '75ms',
          '100': '100ms',
          '150': '150ms',
          '200': '200ms',
          '300': '300ms',
          '500': '500ms',
          '700': '700ms',
          '1000': '1000ms',
        },
      },
    },
  },
  plugins: [],
}
