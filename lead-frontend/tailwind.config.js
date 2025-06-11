// tailwind.config.js
/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{js,jsx,ts,tsx}", // Garante que o Tailwind escaneie seus arquivos React
    "./public/index.html",
  ],
  theme: {
    extend: {},
  },
  plugins: [], // Plugins do Tailwind, como o Formas, se for usar
}