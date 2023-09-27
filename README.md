# RedisClient Blazor Application

This is a simple Blazor application that interacts with a Redis cache to fetch and display game data.

## Prerequisites

Before running this application, ensure you have the following installed on your system:

- .NET 5 SDK
- Docker (for running Redis locally)

## Getting Started

### Clone the Repository
    
    git clone https://github.com/your-username/redis-client-blazor.git
    cd redis-client-blazor
    

### Run the Application

To run the application locally, open a terminal in the project's root directory and execute the following commands:

    
    dotnet restore
    dotnet build
    dotnet run
    

This will start the application, and you can access it in your web browser at https://localhost:5001.

### Docker: Run Redis Locally

If you don't have Redis installed locally, you can run it in a Docker container. Use the following Docker command:

    
    docker run --name redis-container -p 6379:6379 -d redis
    
This command pulls the official Redis image from Docker Hub and starts a Redis instance on port 6379. You can adjust the port if needed.

### Application Usage

![image](https://github.com/abiandreb/RedisClient/assets/26677911/39cccdc1-6329-4918-a27f-b43d30bccc5a)

- The application fetches game data either from a Redis cache or an API.
- Click the "Fetch Data" button to load data from the API and cache it in Redis.
- Click the "Clear" button to clear the cached data.

### License

This project is licensed under the MIT License - see the LICENSE file for details.
