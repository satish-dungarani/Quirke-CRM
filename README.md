# MSc Computer Science - Project
- **Name**: Satishkumar Raghavbhai Dungarani
- **Student Id**: 30073102

## Project Overview
This project is a **Customer Relationship Management (CRM) Dashboard** designed for **Quirke & Co Hairdressers** as part of the MSc Computer Science program at the University of South Wales. The system manages client compliance forms, customer data, employee management, and inventory handling. It was developed using **ASP.NET Core MVC** with **Entity Framework Code-First** approach and **Microsoft SQL Server** as the database.

The project repository can be accessed at: [Quirke-CRM GitHub Repository](https://github.com/satish-dungarani/Quirke-CRM.git)

## Software Requirements
To build and run this project locally, the following software is required:

- **IDE**: Visual Studio 2022 (or later)
- **.NET SDK**: .NET 6.0 or above
- **Database**: Microsoft SQL Server (LocalDB or SQL Server Express)
- **Browser**: Chrome, Firefox, or Edge
- **Optional**: IIS (Internet Information Services) or Apache for local hosting (if not using Visual Studio's built-in web server)

## Build Instructions
Follow these steps to set up and run the project locally:

### 1. Clone the Repository
To clone the repository to your local machine, use the following command:
```bash
git clone https://github.com/satish-dungarani/Quirke-CRM.git
```
### 2. Open the Project
- Open **Visual Studio 2022**.
- Select **"Open a project or solution"** and navigate to the folder where the repository is cloned.
- Open the `Quirke.CRM.sln` file.

### 3. Restore NuGet Packages
- Visual Studio will prompt you to restore NuGet packages. Click **"Restore"**.
- Alternatively, you can manually restore the packages via **Tools > NuGet Package Manager > Restore NuGet Packages**.

### 4. Set up the Database
The project uses Entity Framework Code-First migrations to create the database.

#### Step-by-Step:
1. Ensure you have **Microsoft SQL Server** installed (LocalDB or SQL Server Express for local development).
2. Modify the connection string in `appsettings.json` to match your SQL Server setup. Example:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=QuirkeCRM;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```
3. Open the Package Manager Console in Visual Studio (Tools > NuGet Package Manager > Package Manager Console).
4. Run the following command to apply migrations and create the database:
    ```bash
    Update-Database
    ```
### 5. Run the Application
To run the application:

- Press **F5** in Visual Studio.
- Alternatively, click the **Start** button to launch the application.
- The ASP.NET Core application will start on your local machine.

### 6. User Registration, Login, and Accessing the Application

To start using the application, follow these steps:

1. **User Registration**:
   - Navigate to the registration page by visiting `https://localhost:7175/account/signup` (or replace `localhost` with your domain in a hosted environment).
   - Fill in the required details (e.g., username, email, password) and complete the registration process.

2. **Email Confirmation** (if enabled):
   - If the system has email confirmation enabled, check your email inbox for a confirmation link and click on it to verify your account.

3. **Login**:
   - After registration, go to the login page by visiting `https://localhost:7175/account/login`.
   - Enter your credentials and log into the system.

4. **Accessing the Application**:
   - Once logged in, you can navigate through the application to access features such as Client Compliance Forms, Customer Records, Employee Management, and Inventory Management.

5. **Submitting Customer Compliance Form**:
   - The customer submits a compliance form by scanning the **QR code** provided by the salon.
   - The QR code will redirect the customer to the compliance form link: `https://localhost:7175/quirke` (or replace `localhost` with your domain).

6. **Managing Other Features**:
   - After login, access all the application features including managing **Customer Records**, handling **Employee Management**, and **Inventory Management** through the system's dashboard.


### Technologies Used
- **Frontend**: HTML, CSS, Bootstrap, jQuery
- **Backend**: ASP.NET Core MVC
- **Database**: Microsoft SQL Server
- **ORM**: Entity Framework Core
