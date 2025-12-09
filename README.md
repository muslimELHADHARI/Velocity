# Velocity — Bikes & Motors Rental Platform

## Overview

Velocity is a professional and production-ready ASP.NET MVC 8 platform for bicycle and motorcycle rental management. It is built with a modern, mobile-first design and follows best practices for security, code structure, and user experience. The system includes:

- Public vehicle browsing with advanced search and filtering
- Secure account management for Customers and Admins
- Modern reservation and checkout workflows
- Role-based admin dashboard for real-time platform management
- Professional, responsive, and animated UI with light/dark modes

---

## Features

### Public Features

- **Vehicle Listings:** Responsive grid with search, filtering (brand/type), and detailed vehicle pages.
- **Vehicle Details:** Large image preview, specifications, live availability status, and direct booking.
- **Reservations:** Guided reservation process with date/time selection and conflict prevention.
- **Checkout:** Virtual (dummy) payment, pay-on-site option, instant status, and reservation summary.

### Authentication & Roles

- **Customer Registration/Login:** Secure ASP.NET Identity authentication with "Remember Me" support.
- **Profile Management:** Update personal information and change password from the profile page.
- **Roles:** Admin and Customer roles, with role seeding on startup.
- **Admin Controls:** Restrict all sensitive features and dashboard to admins.

### Admin Dashboard

- **Statistics & Insights:** Real-time user, vehicle, reservation, and revenue (virtual payments) statistics.
- **User Management:** List, edit roles, and (de)activate user accounts.
- **Reservation Oversight:** Full listing with payment status, period, and customer details.
- **Vehicle Management:** Create, edit, delete, and update bikes/motors with dynamic availability.
- **Modern UI:** Advanced admin workflows with dashboard tiles, responsive tables, modern icons.

### Design and User Experience

- **Modern Theme:** Orange/yellow animated gradient theme, light/dark mode toggle, professional font stack (Poppins/Inter).
- **Responsive Layout:** Mobile-first experience, card layouts, full-width sections, adaptive navigation.
- **Smooth Animations:** Micro-interactions, button gradients, hover/active effects, and animated form labels.
- **Accessibility:** Keyboard navigable, focus indicators, sufficient color contrast, large interactive areas.

### Security

- **Authentication & Authorization:** Attribute-based access control across all sensitive routes.
- **Efficient Validation:** Server and client-side input validation, reservation slot conflict checking.
- **Defensive Defaults:** Secure password policies, anti-forgery, and proper error handling.
- **Environment Security:** Safe default configuration, environment-specific connection strings, sensitive data protection.

---

## Technologies

- **Backend:** ASP.NET Core MVC 8, .NET 8 (LTS), C#
- **Database:** SQL Server (code-first, migrations via Entity Framework Core)
- **Authentication:** ASP.NET Identity (custom user with roles)
- **Frontend:** Bootstrap 5, custom SCSS/CSS, Bootstrap Icons, Poppins & Inter Google Fonts
- **Tooling:** EF Core Migrations, Visual Studio/VS Code/Cursor

---

## Architecture

```
/Controllers      // MVC Controllers (Home, Vehicles, Reservations, Checkout, Admin, Account)
/Models           // Entity and domain models (Vehicle, Reservation, ApplicationUser, enums)
/ViewModels       // View-specific, server-validated data models
/Services         // Business logic, encapsulated use cases
/Repositories     // Data access layer, abstraction over EF DbContext
/Data             // DbContext, DbSeeder (roles, admin, sample data)
/Views            // Razor Views by controller
/wwwroot          // Static assets (css, js, images, fonts)
```

---

## Installation & Setup

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server instance running on `127.0.0.1,1435` (or update the connection string)

### Steps

1. **Clone the repository**
    ```sh
    git clone <repo-url>
    cd Velocity
    ```

2. **Configure the database connection**
    - By default, uses `appsettings.json`:
      ```
      "ConnectionStrings": {
        "DefaultConnection": "Server=127.0.0.1,1435;Database=VelocityDb;User Id=SA;Password=StrongPassword123!;TrustServerCertificate=True;"
      }
      ```
    - Update this connection string as needed.

3. **Apply migrations and seed data**
    ```sh
    dotnet ef database update
    ```

    This will also seed:
    - An administrator: **admin@velocity.tn** / **Admin#12345**
    - Sample vehicles (bicycles/motorcycles with Tunisian context)

4. **Run the application**
    ```sh
    dotnet run
    ```
    The web app will be available at `https://localhost:5001` (or configured port).

---

## Usage

### Public Pages

- **Browse Fleet:** `/Vehicles`
- **Vehicle Detail:** `/Vehicles/Details/{id}`
- **Register/Login:** `/Account/Register`, `/Account/Login`
- **Reservation:** Available only to logged in users

### Admin

- **Dashboard:** `/Admin`
- **User Management:** `/Admin/Users`
- **Reservations:** `/Admin/Reservations`
- **Vehicle CRUD:** `/Vehicles` (add/edit/delete available to admins)

### Profile

- **Profile & Security:** `/Account/Index` (update email, phone, password, see status)

---

## Customization

- **Images:** Add vehicle images to `/wwwroot/images`. Use `/images/your-image.jpg` as paths.
- **Theme:** Edit `/wwwroot/css/site.css` for advanced color, gradient, animation adjustments.
- **Seed Data:** Modify `/Data/DbSeeder.cs` for default users/vehicles/brands.
- **Connection Strings, Ports:** Edit `appsettings.json`, `launchSettings.json`.

---

## Contributions

- Fork and open pull requests with clear, isolated features or bugfixes.
- Follow MVC best practices, keep code organized by layers.
- Maintain code comments, run linting and tests before submitting.
- Open issues for feature requests or bugs.

---

## License

This project is provided under the MIT License. See `LICENSE` for details.

---

**Velocity** — Bikes & Motors Rental Platform (Tunisian Localization). Secure, modern, production-ready, and easily extensible for your business or demo needs.
