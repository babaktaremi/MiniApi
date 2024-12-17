# MiniApi

MiniApi is a boilerplate project designed to simplify the development of .NET applications by combining several powerful tools and frameworks. This project is particularly suited for small to medium-sized projects or projects that require rapid development.

## Features

- **PostgreSQL**: A powerful, open-source object-relational database system.
- **Mediator Library**: Helps in implementing the Mediator pattern for handling requests and responses.
- **Minimal APIs**: Simplifies the creation of HTTP APIs with minimal setup.
- **EF Core**: Entity Framework Core for database access.
- **Dapper**: A simple object mapper for .NET.
- **Aspire App Host**: A hosting framework for .NET applications.

## Project Structure

The project is organized into several folders and files:

- `MiniApi/`: Contains the main application code.
  - `Features/`: Contains feature-specific code, organized by domain (e.g., `Order`, `Product`).
  - `Shared/`: Contains shared code, such as database entities and extensions.
  - `Migrations/`: Contains database migration files.
  - `Properties/`: Contains project properties and settings.
  - `Program.cs`: The entry point of the application.
  - `MiniApi.csproj`: The project file for the main application.
- `MiniApi.AppHost/`: Contains the hosting configuration for the application.
  - `Program.cs`: The entry point for the hosting application.
  - `MiniApi.AppHost.csproj`: The project file for the hosting application.
- `.gitignore`: Specifies files and directories to be ignored by Git.
- `MiniApi.sln`: The solution file for the project.
- `README.md`: This file.

## Getting Started

1. **Clone the repository**:

   ```sh
   git clone https://github.com/babaktaremi/MiniApi.git
   cd MiniApi
   ```

2. **Build the project**:

   ```sh
   dotnet build
   ```

3. **Run the project**:

   ```sh
   dotnet run --project MiniApiCollecting workspace information
   ```

4. **Access the API**:
   Open your browser and navigate to `http://localhost:5078` to access the API.

## Contributing

If you find any issues or have suggestions for improvements, please open an issue on the [GitHub repository](https://github.com/babaktaremi/MiniApi). If you like the project, consider giving it a star!

## License

This project is licensed under the MIT License. See the LICENSE file for more details.

---

Join the community: [@DotNetIsFun](https://t.me/DotNetIsFun)
