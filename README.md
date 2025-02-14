# MessageBroker

## Overview

MessageBroker is a .NET-based messaging system that facilitates message production and consumption using a broker system. It consists of multiple components, including an API, producer, and consumer libraries.

## Project Structure

```
MessageBroker/
│── MessageBrokerAPI/      # ASP.NET Core API
│   ├── Controllers/       # API Controllers (Message handling)
│   ├── Program.cs         # API entry point
│   ├── Startup.cs         # Configurations
│
│── MessageBrokerLib/      # Core messaging library
│
│── ProducerLib/           # Message producer logic
│
│── ConsumerLib/           # Message consumer logic
│
│── MessageBroker.sln      # Solution file
```

## Prerequisites

- .NET 8.0 SDK
- A message broker (e.g., RabbitMQ, Kafka, or a custom implementation)
- Postman or any API testing tool (optional, for testing endpoints)

## Installation & Setup

1. Clone the repository:
   ```sh
   git clone <repo_url>
   cd MessageBroker
   ```
2. Restore dependencies:
   ```sh
   dotnet restore
   ```
3. Build the solution:
   ```sh
   dotnet build
   ```

## Running the API

To start the API:

```sh
cd MessageBrokerAPI
dotnet run
```

The API will be available at `http://localhost:5000` (or another assigned port).

## API Endpoints

### Message Broker Controller

- `POST /api/messages/send` - Send a message
- `GET /api/messages/receive` - Retrieve messages

### Plugins Controller

- `GET /api/plugins` - List available plugins

## Running the Producer

To start the producer:

```sh
cd ProducerLib
dotnet run
```

## Running the Consumer

To start the consumer:

```sh
cd ConsumerLib
dotnet run
```

## Testing

Use Postman or cURL to test API endpoints:

```sh
curl -X POST http://localhost:5000/api/messages/send -d '{"message":"Hello"}' -H "Content-Type: application/json"
```

## How It Works

- **MessageBrokerAPI** handles HTTP requests for message production and consumption.
- **ProducerLib** sends messages to the broker.
- **ConsumerLib** retrieves messages from the broker.
- **MessageBrokerLib** provides core functionality for messaging.


