# Cafe API Controllers Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Implement a full set of RESTful controllers for the Cafe management system.

**Architecture:** ASP.NET Core Web API with EF Core. Use DTOs for data transfer to avoid circular references and simplify the API surface. Integrated logic for complex operations like ordering and kitchen status updates.

**Tech Stack:** .NET 8.0, EF Core, SQL Server.

---

### Task 1: Project Setup & Config Fix

**Files:**
- Modify: `DoAnFlutterAPI/appsettings.json`
- Modify: `DoAnFlutterAPI/Program.cs`
- Create: `DoAnFlutterAPI/DTOs/BaseResponse.cs`

- [ ] **Step 1: Fix connection string mismatch**
Update `appsettings.json` to use "CafeDB" as the key.
```json
"ConnectionStrings": {
  "CafeDB": "Server=TRUNGPC;Database=CafeDB;User ID=sa;Password=123;Trust Server Certificate=True"
}
```

- [ ] **Step 2: Remove hardcoded connection string from DbContext**
Modify `DoAnFlutterAPI/Entities/CafeDbContext.cs` to remove the `OnConfiguring` override or make it safe.

- [ ] **Step 3: Create a common response DTO**
```csharp
namespace DoAnFlutterAPI.DTOs;
public class ApiResponse<T>
{
    public bool Success { get; set; } = true;
    public string Message { get; set; } = "";
    public T? Data { get; set; }
}
```

### Task 2: Menu Management (Categories & MenuItems)

**Files:**
- Create: `DoAnFlutterAPI/DTOs/MenuDtos.cs`
- Create: `DoAnFlutterAPI/Controllers/CategoriesController.cs`
- Create: `DoAnFlutterAPI/Controllers/MenuItemsController.cs`

- [ ] **Step 1: Define Menu DTOs**
Include `CategoryDto` and `MenuItemDto`.

- [ ] **Step 2: Implement CategoriesController**
Standard CRUD. Soft delete logic.

- [ ] **Step 3: Implement MenuItemsController**
CRUD. Endpoint for items by category.

### Task 3: Floor & Session Management

**Files:**
- Create: `DoAnFlutterAPI/DTOs/FloorDtos.cs`
- Create: `DoAnFlutterAPI/Controllers/TablesController.cs`
- Create: `DoAnFlutterAPI/Controllers/TableSessionsController.cs`

- [ ] **Step 1: Define Floor DTOs**
Include `TableDto` and `SessionDto`.

- [ ] **Step 2: Implement TablesController**
Include an endpoint that returns table status from `VwTableStatus`.

- [ ] **Step 3: Implement TableSessionsController**
Logic for opening and closing sessions.

### Task 4: Ordering & Kitchen

**Files:**
- Create: `DoAnFlutterAPI/DTOs/OrderDtos.cs`
- Create: `DoAnFlutterAPI/Controllers/OrdersController.cs`
- Create: `DoAnFlutterAPI/Controllers/KitchenController.cs`

- [ ] **Step 1: Define Order DTOs**
`OrderCreateDto` (with items), `OrderResponseDto`, `KitchenQueueDto`.

- [ ] **Step 2: Implement OrdersController.PostOrder**
Integrated logic: Create `Order`, then `OrderItems`, then `KitchenQueue` entries. Use a transaction.

- [ ] **Step 3: Implement KitchenController**
Get queue from `VwKitchenQueue`. Update status (updates `KitchenQueue` and `OrderItem`).

### Task 5: Inventory, Payments & Reports

**Files:**
- Create: `DoAnFlutterAPI/DTOs/InventoryDtos.cs`
- Create: `DoAnFlutterAPI/Controllers/IngredientsController.cs`
- Create: `DoAnFlutterAPI/Controllers/StockTransactionsController.cs`
- Create: `DoAnFlutterAPI/Controllers/PaymentsController.cs`
- Create: `DoAnFlutterAPI/Controllers/ReportsController.cs`

- [ ] **Step 1: Implement IngredientsController**
Include low-stock view endpoint.

- [ ] **Step 2: Implement StockTransactionsController**
Log stock changes and update `Ingredient.CurrentStock`.

- [ ] **Step 3: Implement PaymentsController**
Record payment and update `Order.Status` / `TableSession.Status` if appropriate.

- [ ] **Step 4: Implement ReportsController**
Endpoints for revenue views.

### Task 6: Admin & Verification

**Files:**
- Create: `DoAnFlutterAPI/Controllers/AdminsController.cs`
- Test: `DoAnFlutterAPI/DoAnFlutterAPI.http`

- [ ] **Step 1: Implement AdminsController**
Simple login and CRUD.

- [ ] **Step 2: Update .http file for testing**
Add requests for all new endpoints.

- [ ] **Step 3: Final Verification**
Build and run. Verify Swagger output.
