EasyWay Ride Hailing System API

Reuploaded the project because of some mistakes

Overview
The EasyWay Ride Hailing System API is the backend infrastructure for a ride-hailing platform that connects passengers with drivers. It manages all core functionalities of the system, including ride requests, driver availability, pricing calculations, and real-time updates. The API is built using .NET, with MSSQL as the primary database and Redis as a caching mechanism to ensure high performance and scalability.

Key Features
1. Ride Management
Ride Requests: Handles incoming ride requests from passengers, including destination selection and fare calculation based on distance and car type.
Driver Availability: Manages driver statuses (online/offline) and assigns the nearest available driver to a ride request.

3. Real-Time Communication
Firebase Messaging: Integrates with Firebase to send notifications to drivers and passengers about ride status, updates, and alerts.

5. Data Management
MSSQL Database: Stores and manages all persistent data related to passengers, drivers, rides, and transactions.
Redis Caching: Implements Redis for caching frequently accessed data, reducing latency and improving response times.

Technology Stack
Backend Framework: .NET
Database: MSSQL
Caching: Redis
Messaging: Firebase Cloud Messaging
