/** @type {import('tailwindcss').Config} */
// eslint-disable-next-line no-undef
module.exports = {
  darkMode: ["class"],
  content: [
    "./pages/**/*.{ts,tsx}",
    "./components/**/*.{ts,tsx}",
    "./app/**/*.{ts,tsx}",
    "./src/**/*.{ts,tsx}",
  ],
  prefix: "",
  theme: {
    screens: {
      xs: "430px",
      ms: "590px",
      sm: "640px",
      tb: "864px",
      md: "960px",
      lg: "1024px",
      // => @media (min-width: 1024px) { ... }
      xl: "1360px",
      // => @media (min-width: 1280px) { ... }

      "2xl": "1565px",
    },
    container: {
      center: true,
      padding: "2rem",
      screens: {
        "2xl": "1400px",
      },
    },
    extend: {
      colors: {
        border: "hsl(var(--border))",
        input: "hsl(var(--input))",
        ring: "hsl(var(--ring))",
        background: "hsl(var(--background))",
        foreground: "hsl(var(--foreground))",
        primary: {
          DEFAULT: "hsl(var(--primary))",
          foreground: "hsl(var(--primary-foreground))",
        },
        secondary: {
          DEFAULT: "hsl(var(--secondary))",
          foreground: "hsl(var(--secondary-foreground))",
        },
        destructive: {
          DEFAULT: "hsl(var(--destructive))",
          foreground: "hsl(var(--destructive-foreground))",
        },
        muted: {
          DEFAULT: "hsl(var(--muted))",
          foreground: "hsl(var(--muted-foreground))",
        },
        accent: {
          DEFAULT: "hsl(var(--accent))",
          foreground: "hsl(var(--accent-foreground))",
        },
        popover: {
          DEFAULT: "hsl(var(--popover))",
          foreground: "hsl(var(--popover-foreground))",
        },
        card: {
          DEFAULT: "hsl(var(--card))",
          foreground: "hsl(var(--card-foreground))",
        },
        "primary-1": "#588E58",
        "primary-2": "#B3E8B2",
        "primary-3": "#F5FFF5",
        "primary-4": "#356D35",
        "secondary-1": "#F30000",
        "secondary-2": "#952E2B",
        "neutral-black": "#444444",
        "neutral-white": "#FFFFFF",
        "neutral-silver": "#F5F5F5",
        "neutral-silver-1": "#E0E0E0",
        "neutral-silver-2": "#EAEAEA",
        "neutral-silver-3": "#A5A5A5",
      },
      boxShadow: {
        "white-blur": "0 0px 600px rgba(255, 255, 255, 1)",
        "white-shadow": "0 1px 2px 0 rgba(255, 255, 255, 0.05)",
        "custom-shadow": "2px 2px 12px 2px rgba(155, 155, 155, 0.25)",
      },
      borderRadius: {
        lg: "var(--radius)",
        md: "calc(var(--radius) - 2px)",
        sm: "calc(var(--radius) - 4px)",
      },
      keyframes: {
        "accordion-down": {
          from: { height: "0" },
          to: { height: "var(--radix-accordion-content-height)" },
        },
        progress: {
          "0%": { left: "-100%" },
          "100%": { left: "100%" },
        },
        "accordion-up": {
          from: { height: "var(--radix-accordion-content-height)" },
          to: { height: "0" },
        },
      },
      animation: {
        "accordion-down": "accordion-down 0.2s ease-out",
        "accordion-up": "accordion-up 0.2s ease-out",
        "progress-infinite": "progress 2s linear infinite",
        "spin-slow": "spin 10s linear infinite",
      },
      fontFamily: {
        main: ["Inter", "sans-serif"],
      },
    },
  },
  // eslint-disable-next-line no-undef
  plugins: [require("tailwindcss-animate")],
}
