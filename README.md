# 📌 Project Summary

This project is part of the Ambev test. I aimed to be as concise as possible with concepts to streamline the process. Unfortunately, I didn't have enough time to implement everything I wanted, but below, I will list all the future features I consider necessary for a high-availability system, based on my knowledge and past experiences.

I structured the commits by separating the features according to their development progress.

# ✅ Features Implemented in the Project

Implemented CRUD for the Order and OrderItem entities, including:

Basic filtering and pagination

Insertion, updating, and cancellations

Fixed PostgreSQL connection in Docker Compose so that all services start properly and connect without additional configuration.

Fixed authentication endpoints that had mapping issues.

Implemented unit tests for key functionalities.

# 🚀 How to Run the Project

Start the project using Docker Compose mode so that the database starts via Docker.

Run the migrations to create the required tables in the database.

Once completed, the system is ready for testing.

Run EF Core migrations
dotnet ef database update

If the migration does not work
Execute the script.sql file in the database to apply the migrations.

# 🔮 Future Enhancements for Scalability & Microservices

Implement a message broker (e.g., RabbitMQ) to distribute events across necessary systems.

Improve JWT authentication using a tool that simplifies user and client management, such as Keycloak.

Develop a frontend and implement a Backend for Frontend (BFF) pattern.

Improve logging and monitoring, integrating tools like Serilog, Sentry, and others for better visibility.

# 🛠️ Technologies Used

.NET 7

Entity Framework Core

PostgreSQL

Docker & Docker Compose

MediatR

AutoMapper

FluentValidation

xUnit, Moq, FluentAssertions (for unit testing)

# 📄 License

This project is for evaluation purposes and does not include a specific license.

# 🔗 Author

Developed by Lucas Silva
