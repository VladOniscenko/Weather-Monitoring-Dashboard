# Weather Monitoring Dashboard

A professional Full-Stack monitoring solution that tracks and visualizes real-time weather data. Built with a .NET 8 Clean Architecture backend and a React frontend.

## 🚀 Key Features

- **Automated Data Accumulator**: Background worker syncs with OpenWeatherMap API every 10 minutes.
- **Secure Identity System**: JWT authentication with RBAC.
- **Interactive Analytics**: Charts for temperature, humidity, and wind (Recharts).
- **Station Management**: Admin interface for global stations.
- **3-Layer Architecture**: Domain, Infrastructure, API.

---

## 🏗️ Tech Stack

### Backend (.NET 8)
- ASP.NET Core Web API  
- Entity Framework Core (PostgreSQL)  
- JWT Authentication & BCrypt.Net  
- Background Services (IHostedService)

### Frontend (React)
- Vite & Tailwind CSS  
- Recharts  
- Axios  


---

## 🛠️ Getting Started

### 1. Configuration

Create a `.env` file in the root directory:

    POSTGRES_USER=postgres
    POSTGRES_PASSWORD=your_secure_password
    POSTGRES_DB=weatherdb
    JWT_SECRET=your_32_character_secret_key
    OPENWEATHER_API_KEY=your_api_key

### 2. Execution

Run:

    docker-compose up --build

Endpoints:
- API: http://localhost:5001  
- Frontend: http://localhost:3000  

---

## 📊 Data Design

One-to-Many relationship between Weather Stations and Weather Readings.

---

## 🔒 Security

- BCrypt password hashing  
- JWT Bearer authentication  
- Global exception middleware  

---

Developed by Vlad
