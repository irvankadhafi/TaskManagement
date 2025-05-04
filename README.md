# Task Management System

A practical **Task Management System** built with **.NET 8** that demonstrates clean separation of concerns, SOLID design, and flexible infrastructure. The service exposes a RESTful API for managing tasks and can run either in‑memory or on PostgreSQL with a single switch.

---

## Table of Contents

1. [Overview](#overview)
2. [Architecture](#architecture)
3. [Features](#features)
4. [Prerequisites](#prerequisites)
5. [Getting Started](#getting-started)

    1. [Clone the Repository](#clone-the-repository)
    2. [Run Locally](#run-locally)
    3. [Run with Docker](#run-with-docker)
6. [API Reference](#api-reference)
7. [Testing](#testing)
8. [Project Structure](#project-structure)

---

## Overview

This service lets you **create, assign, update, and track tasks**. It is intentionally lightweight yet production‑ready:

* **Layered Clean Architecture** – Presentation ➜ Application ➜ Domain ➜ Infrastructure.
* **Dependency Injection** – services and repositories are registered in `Program.cs`.
* **Repository Pattern** – swap between **In‑Memory** and **PostgreSQL** without changing higher layers.
* **Extensive Unit Tests** – core use‑cases are covered with xUnit + Moq + FluentAssertions.

---

## Architecture

```text
┌──────────────────────────┐
│   Presentation Layer     │  → ASP.NET Core Web API (Delivery)
└───────────────▲──────────┘
                │ Calls (DTO)
┌───────────────┴──────────┐
│   Application Layer      │  → Use‑Case Interactors (TaskService), Ports, DTOs
└───────────────▲──────────┘
                │ Depends on Abstractions
┌───────────────┴──────────┐
│     Domain Layer         │  → Entities (TaskItem), Enums, Repository Interfaces
└───────────────▲──────────┘
                │ Implemented by
┌───────────────┴──────────┐
│ Infrastructure Layer     │  → InMemoryTaskRepository | TaskRepository(EF Core)
└──────────────────────────┘
```

### Dependency Flow

High‑level policies (Domain/Application) are completely isolated from low‑level concerns (Infrastructure, Web API). Swapping a database, adding RabbitMQ, or migrating to gRPC requires changes only in the outer layers.

---

## Features

| Capability       | Description                                                           |
| ---------------- | --------------------------------------------------------------------- |
| **Create Task**  |  `POST /api/tasks` – title, description, due date, priority, assignee |
| **Update Task**  |  `PUT /api/tasks/{id}` – change status or priority                    |
| **Delete Task**  |  `DELETE /api/tasks/{id}`                                             |
| **List Tasks**   |  `GET /api/tasks`                                                     |
| **List by User** |  `GET /api/tasks/user/{userId}`                                       |
| **Validation**   |  Rejects tasks with due‑date in the past                              |
| **Logging**      |  Key operations are logged via built‑in logging provider              |

---

## Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/download)
* (Optional) [Docker Desktop](https://www.docker.com/) for containerised run

---

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/irvankadhafi/TaskManagement.git
cd task‑management
```

### Run Locally (In‑Memory Database)

```bash
cd src/TaskManagement.API
# restore packages & run
dotnet restore
DOTNET_ENVIRONMENT=Development dotnet run
```

The API listens on `https://localhost:5001` and Swagger UI is available at `/swagger`.

### Run with Docker (PostgreSQL)

```bash
docker‑compose up --build
```

* **API** → [http://localhost:5000](http://localhost:5000)
* **PostgreSQL** exposed on 5432 (credentials in `docker‑compose.yml`).

---

## API Reference

Open the generated Swagger UI to explore and test every endpoint interactively.

---

## Testing

Unit tests live under `tests/TaskManagement.Application.Tests`.

```bash
dotnet test tests/TaskManagement.Application.Tests
```

The suite covers:

* Creating a task (happy & invalid paths)
* Updating status/priority
* Listing tasks (all / by user)
* Deleting tasks

All use‑cases are isolated with mocks, making the tests fast and deterministic.

---

## Project Structure

```text
TaskManagement/
├─ src/
│  ├─ TaskManagement.Domain/         # Entities & enums
│  ├─ TaskManagement.Application/    # DTOs, ports
│  ├─ TaskManagement.UseCases/       # TaskService (interactor)
│  ├─ TaskManagement.Infrastructure/ # In‑memory repository
│  ├─ TaskManagement.Persistence/    # EF Core, DbContext, PG repository
│  └─ TaskManagement.API/            # ASP.NET Core Web API
└─ tests/
   └─ TaskManagement.Application.Tests/ # Unit tests
```

---