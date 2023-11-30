
# Time Management System

This web-based time management system allows users to efficiently track and manage their study hours for different modules as well as add semesters.

## Features

- **Semester Management:** Add, edit and delete semesters
- **Study Hours Tracking:** Record and monitor study hours for each module.
- **Graphical Representation:** Visualize study hours using interactive graphs.
- **Module Management:** Add, edit, and delete modules.
- **Study Hours calculation:** Automatically calculates study hors that each module will require

## Technologies Used

- ASP.NET MVC
- Entity Framework Core
- Chart.js
- Bootstrap

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/)
- [Visual Studio](https://visualstudio.microsoft.com/) 

### Installation

1. **Clone the Repository:**

    ```bash
    git clone https://github.com/ST10019757/https://github.com/VCWVL/prog6212-poe-ST10019757.git
    cd ST10019757
    ```

2. **Database Configuration:**

    - Open `appsettings.json` and update the connection string if needed.
    - Run Entity Framework migrations:

        ```bash
        dotnet ef database update
        ```

3. **Run the Application:**

    ```bash
    dotnet run
    ```

4. Open your web browser and go to `http://localhost:5000` to view the application.

## Database Instructions

### Database Migrations

To apply database migrations, use the following commands:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Resetting the Database

To reset the database (remove all data and apply migrations again), use:

```bash
dotnet ef database drop
dotnet ef database update
```



## Contributing

Contributions are welcome! If you find any issues or have suggestions, feel free to let me know.

## License

This project is licensed under the [MIT License](https://memgraph.com/blog/what-is-mit-license).
```
