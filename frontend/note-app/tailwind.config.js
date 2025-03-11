import defaultTheme from "tailwindcss/defaultTheme";

/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./src/**/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
        fontFamily: {
            primary: "Poppins",
            sans: ["Poppins", ...defaultTheme.fontFamily.sans],
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
            "black-15": "rgba(0, 0, 0, 0.15)",
            "black-30": "rgba(0, 0, 0, 0.3)",
            "white-30": "rgba(255, 255, 255, 0.3)",
            success: "#10B981",
            warning: "#F59E0B",
            error: "#EF4444",
        },
    },
},
  plugins: [],
}
