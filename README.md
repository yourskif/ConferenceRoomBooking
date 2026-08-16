# Conference Room Booking API

## Project Overview

Conference Room Booking API is a backend application for managing conference rooms, bookings, and rental price calculation.

The system allows users to:

- create conference rooms;
- update conference room information;
- delete conference rooms;
- view the list of conference rooms;
- create bookings;
- validate booking conflicts;
- calculate rental price based on booking time and selected extra services;
- access business reports through reporting endpoints.

The project is implemented as a REST API and documented with Swagger.

---

## Business Context

The company rents conference rooms to business clients. Clients need an API that allows them to find an available room, book it for a specific time, and receive the final rental price.

The final price depends on:

- the room base hourly rate;
- the booking time period;
- selected extra services;
- pricing rules for different time ranges.

---

## Initial Data

### Conference Rooms

| ID | Name | Capacity | Base Hourly Rate |
|---:|------|---------:|-----------------:|
| 1 | Hall A | 50 | 2000 |
| 2 | Hall B | 100 | 3500 |
| 3 | Hall C | 30 | 1500 |

### Extra Services

| ID | Service | Price |
|---:|---------|------:|
| 1 | Projector | 500 |
| 2 | Wi-Fi | 300 |
| 3 | Sound | 700 |

In the current seed data, all three extra services are available for all three conference rooms.

---

## Rental Price Calculation Rules

Room rental price depends on the booking time:

| Period | Time | Rule |
|--------|------|------|
| Morning hours | 06:00–09:00 | 10% discount |
| Standard hours | 09:00–18:00 | base price |
| Peak hours | 12:00–14:00 | 15% surcharge |
| Evening hours | 18:00–23:00 | 20% discount |

The final booking price is calculated as:

```text
totalPrice = roomPrice + servicesPrice
```

where:

- `roomPrice` is the room rental price for the selected time period;
- `servicesPrice` is the total price of selected extra services;
- `totalPrice` is the final booking price.

---

## Main API Endpoints

### Conference Rooms

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/conference-rooms` | Get all conference rooms |
| GET | `/api/conference-rooms/{id}` | Get a conference room by ID |
| POST | `/api/conference-rooms` | Create a conference room |
| PUT | `/api/conference-rooms/{id}` | Update a conference room |
| DELETE | `/api/conference-rooms/{id}` | Delete a conference room |

### Bookings

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/bookings` | Get all bookings |
| GET | `/api/bookings/{id}` | Get a booking by ID |
| POST | `/api/bookings` | Create a booking |

### Reports

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/reports/...` | Get business reports and analytics |

The exact reporting endpoints can be viewed in Swagger.

---

## Booking Request Example

```json
{
  "conferenceRoomId": 1,
  "startTime": "2026-10-01T10:00:00",
  "endTime": "2026-10-01T12:00:00",
  "serviceIds": [1, 2]
}
```

Example response:

```json
{
  "id": 4,
  "conferenceRoomId": 1,
  "startTime": "2026-10-01T10:00:00",
  "endTime": "2026-10-01T12:00:00",
  "roomPrice": 4000.0,
  "servicesPrice": 800.0,
  "totalPrice": 4800.0,
  "services": [
    {
      "extraServiceId": 1,
      "serviceName": "Projector",
      "price": 500.0
    },
    {
      "extraServiceId": 2,
      "serviceName": "Wi-Fi",
      "price": 300.0
    }
  ]
}
```

---

## Booking Validations

When creating a booking, the API validates that:

- `startTime` is earlier than `endTime`;
- `startTime` is in the future;
- the conference room exists and is not deleted;
- all selected extra services exist;
- selected extra services are available for the selected conference room;
- booking time does not overlap with existing bookings for the same conference room.

---

## Smoke Test Results for Bookings API

During manual testing, the main positive and negative scenarios for `/api/bookings` were verified.

| # | Scenario | Expected Result | Status |
|---:|----------|-----------------|--------|
| 1 | `GET /api/bookings` | `200 OK` and bookings list | ✅ Passed |
| 2 | `GET /api/bookings/1` | `200 OK` and booking with ID 1 | ✅ Passed |
| 3 | `GET /api/bookings/999` | `404 Not Found` | ✅ Passed |
| 4 | `POST /api/bookings` with valid body | booking with ID 4 is created | ✅ Passed |
| 5 | `GET /api/bookings/4` | `200 OK` and created booking | ✅ Passed |
| 6 | `DELETE /api/bookings/4` | `405 Method Not Allowed` | ✅ Passed |
| 7 | Repeated `POST` with overlapping time | `409 Conflict` | ✅ Passed |
| 8 | `POST` with `startTime >= endTime` | `400 Bad Request` | ✅ Passed |
| 9 | `POST` with `startTime` in the past | `400 Bad Request` | ✅ Passed |
| 10 | `POST` with unknown `conferenceRoomId` | `404 Not Found` | ✅ Passed |
| 11 | `POST` with unknown `serviceId` | `400 Bad Request` | ✅ Passed |
| 12 | Existing service unavailable for selected room | cannot be tested with current seed data because all services are available for all rooms | Not applicable |

---

## PowerShell Smoke Test Examples

### Base URL

```powershell
$base = "https://localhost:7244"
```

### Get all bookings

```powershell
Invoke-RestMethod "$base/api/bookings"
```

### Get booking by ID

```powershell
Invoke-RestMethod "$base/api/bookings/1"
```

### Create booking

```powershell
$body = @{
    conferenceRoomId = 1
    startTime = "2026-10-01T10:00:00"
    endTime = "2026-10-01T12:00:00"
    serviceIds = @(1, 2)
} | ConvertTo-Json

Invoke-RestMethod "$base/api/bookings" -Method Post -ContentType "application/json" -Body $body
```

### Check overlap conflict

```powershell
Invoke-RestMethod "$base/api/bookings" -Method Post -ContentType "application/json" -Body $body
```

Expected result:

```text
409 Conflict
```

### Check invalid time

```powershell
$invalidTimeBody = @{
    conferenceRoomId = 1
    startTime = "2026-10-02T12:00:00"
    endTime = "2026-10-02T10:00:00"
    serviceIds = @(1, 2)
} | ConvertTo-Json

try {
  Invoke-RestMethod "$base/api/bookings" -Method Post -ContentType "application/json" -Body $invalidTimeBody
} catch {
  $_.Exception.Response.StatusCode.value__
  $_.Exception.Response.StatusDescription
}
```

Expected result:

```text
400
Bad Request
```

---

## How to Run the Project Locally

### 1. Restore dependencies

```powershell
dotnet restore
```

### 2. Build solution

```powershell
dotnet build
```

### 3. Run API

```powershell
dotnet run --project .\src\ConferenceRoomBooking.Api
```

### 4. Open Swagger

After the API starts, Swagger is available at the address shown in the application console. During local smoke testing, the following base URL was used:

```text
https://localhost:7244
```

---

## Technical Decisions

- The REST API is separated by responsibility between controllers, domain entities, persistence layer, and application services.
- Price calculation is handled by a separate service layer.
- Entity Framework Core is used for data access.
- `AsNoTracking` is used for read-only list queries to avoid unnecessary tracking.
- Response DTOs are used for API responses.
- Request DTOs are used for create and update operations.
- Conference room deletion is implemented as soft delete using `IsDeleted`.
- API documentation is provided with Swagger.

---

## Current Status

At the time of the latest manual verification, the Bookings API successfully passed the smoke test for the main create, read, and validation scenarios.

This README can be used as short repository documentation and as a summary of completed work for project review.
