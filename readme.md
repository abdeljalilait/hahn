# Tickets API

## Overview

This repository contains the Tickets API built with .NET Core. The API is designed to work with a PostgreSQL database, which is run in a Docker container using Docker Compose.

## Prerequisites

Before you begin, ensure you have the following installed:

- [.NET Core SDK](https://dotnet.microsoft.com/download/dotnet-core)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)
- [Node.js](https://nodejs.org/en/download/) (for the front-end)

## Getting Started

1. Clone the repository to your local machine:

    ```bash
    git clone https://repo-url.git
    cd tickets-api
    ```

2. Start the PostgreSQL database with Docker Compose:

    ```bash
    docker-compose up -d
    ```

3. Apply the database migrations:

    ```bash
    dotnet ef database update
    ```

4. Run the API:

    ```bash
    dotnet run
    ```

   The API should now be running and accessible at [http://localhost:5289](http://localhost:5289).

5. Run the Front-End:

    Navigate to the `tickets-front` directory and run the following commands to install dependencies and start the development server:

    ```bash
    cd ../tickets-front
    npm install
    npm run dev
    ```

   The front-end application should now be running and accessible at [http://localhost:5173](http://localhost:5173) (or whichever port is configured).
