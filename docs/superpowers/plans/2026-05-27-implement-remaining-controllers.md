# Implement Remaining Controllers Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Implement TabletConfigs, ItemIngredients, and PrintLogs controllers and their associated DTOs.

**Architecture:** ASP.NET Core Web API with Entity Framework Core and DTO mapping. Each controller uses the `CafeDbContext` and returns `ApiResponse<T>`.

**Tech Stack:** .NET 8, EF Core, C#.

---

### Task 1: Update DTOs

**Files:**
- Modify: `DoAnFlutterAPI/DTOs/FloorDtos.cs`
- Modify: `DoAnFlutterAPI/DTOs/MenuDtos.cs`
- Modify: `DoAnFlutterAPI/DTOs/OrderDtos.cs`

- [ ] **Step 1: Update FloorDtos.cs**
  Add `TabletConfigDto` and `TabletConfigCreateDto`.

- [ ] **Step 2: Update MenuDtos.cs**
  Add `ItemIngredientDto` and `ItemIngredientCreateDto`.

- [ ] **Step 3: Update OrderDtos.cs**
  Add `PrintLogDto`.

### Task 2: Implement TabletConfigsController

**Files:**
- Create: `DoAnFlutterAPI/Controllers/TabletConfigsController.cs`

- [ ] **Step 1: Create TabletConfigsController.cs**
  Implement GET all, GET by deviceId, and POST setup (UPSERT logic).

### Task 3: Implement ItemIngredientsController

**Files:**
- Create: `DoAnFlutterAPI/Controllers/ItemIngredientsController.cs`

- [ ] **Step 1: Create ItemIngredientsController.cs**
  Implement GET by menuItemId, POST add, and DELETE remove.

### Task 4: Implement PrintLogsController

**Files:**
- Create: `DoAnFlutterAPI/Controllers/PrintLogsController.cs`

- [ ] **Step 1: Create PrintLogsController.cs**
  Implement GET by orderId and POST record attempt.

### Task 5: Verification

- [ ] **Step 1: Build the project**
  Run `dotnet build` to ensure everything compiles correctly.
