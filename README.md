# MarketCatalogue – E-Commerce Demo Platform

A full-stack e-commerce platform built in ASP.NET Core following **Clean Architecture** principles. The app simulates interactions between **Market Representatives** (managing products and shops) and **Purchasers** (browsing, purchasing, and managing carts/orders).

---

## Installation Instructions
[Instructions.md](./Instructions.md)

## Features

### Purchaser Features
- View **all shops** (displayed on a map using geolocation)
- View available products
- Add products to cart
- Edit/remove cart items
- Purchase cart
- Cancel orders
- View order history and status

### Market Representative Features
- CRUD operations for **Products**
- CRUD operations for **Shops**

---

## Technical Overview

### Architecture – Clean Architecture Inspired
- **Domain**:
  - Core business models: `Entities`, `ValueObjects`, `DTOs`, `Interfaces`
  - **No dependencies**
- **Application**:
  - Business logic: `Services`, `Validators`, `Exceptions`
- **Infrastructure**:
  - `DbContext`, external API integrations (e.g. geolocation, email)
- **Presentation (Web UI)**:
  - ASP.NET Core MVC app
  - Uses cookie-based authentication with Identity

### Authentication
- ASP.NET Identity with cookie-based auth
- Roles: **Purchaser** and **MarketRepresentative**
- Auth lives in a dedicated `Authentication` project

---

## Email Support
- **SMTP (smtp4dev)** is used to send:
  - Email confirmation
  - Order confirmation
  - Order cancellation

---

## Location Features
- **Leaflet** + **[Geocode.maps.co](https://geocode.maps.co/)** used to:
  - Display shop addresses on a map

---

## Background Jobs
- **Hangfire (In-Memory)** used to:
  - Automatically update order statuses (e.g. InProgress → Completed)

---

## Performance
- **Caching** applied to the Shops main page to improve performance

---

## 📦 Tech Stack

| Area             | Tech                             |
|------------------|----------------------------------|
| Backend          | ASP.NET Core (MVC)               |
| Database         | Entity Framework Core (SSMS)     |
| Auth             | ASP.NET Identity (Cookie-based)  |
| Background Jobs  | Hangfire (In-Memory)             |
| Mapping          | AutoMapper                       |
| Maps             | Leaflet + Geocode.maps.co        |
| Emails           | smtp4dev                         |
| UI               | Bootstrap                        |
| Caching          | IMemoryCache                     |

---

