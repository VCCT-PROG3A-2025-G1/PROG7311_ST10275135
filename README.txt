 Agri-Energy Connect Platform
Updates 
Updated models (Farmer, Product, ApplicationUser) to support proper data types.
Login Page Fixes
Fixed redirection after login to ensure users land on the correct dashboard.
Added support for role detection (Farmer or Employee) in AccountController.
Products Page Fixes
Farmers can their products.
Employees can view all farmers and filter products by:
Farmer
Category
Production date range

Overview
This is a web-based prototype application for the Agri-Energy Connect Platform. It allows Farmers and Employees to manage agricultural product data using a secure login system.

The app is built using:
- ASP.NET Core MVC (.NET 8)
- Razor Pages (Frontend)
- Entity Framework Core
- SQLite (pre-populated and included)

How to Run the App

 Requirements
- Visual Studio 2022
- .NET 8 SDK
- Windows 10 or 11

Steps
1. Clone the project or download the ZIP.
2. Open the solution file in Visual Studio.
3. Check the connection string in `appsettings.json`:
4. Open Package Manager Console and run:
5. Press Ctrl + F5 to run the app.
6. Login with one of the default accounts below.

Default Login Details

Employee:
- Email: employee1@agri.com
- Password: P@ssword123

Farmer:
- Email: farmer1@agri.com
- Password: P@ssword123

Roles and Features

Employee
- Login
- Add Farmers
- View and filter products

Farmer
- Login
- Add Products
- View own products

