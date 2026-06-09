# Design Spec - Cafe API Controllers

## 1. Overview
This specification outlines the implementation of a full set of RESTful controllers for the `DoAnFlutterAPI` project. The goal is to provide a complete backend for a cafe management system, supporting menu management, ordering, kitchen operations, inventory, and reporting.

## 2. Architecture
- **Framework**: .NET 8.0 Web API.
- **ORM**: Entity Framework Core with SQL Server.
- **Pattern**: Standard Controller-Service-Repository (simplified for this basic approach using direct Context access in controllers).
- **Data Transfer**: Use DTOs to decouple the database schema from the API response and avoid circular reference issues.

## 3. Controllers and Endpoints

### 3.1 Menu Management
- **CategoriesController**
  - `GET /api/categories`: List all active categories.
  - `GET /api/categories/{id}`: Get category details.
  - `POST /api/categories`: Create new category.
  - `PUT /api/categories/{id}`: Update category.
  - `DELETE /api/categories/{id}`: Soft delete (set `IsActive = false`).

- **MenuItemsController**
  - `GET /api/menuitems`: List all items.
  - `GET /api/menuitems/category/{categoryId}`: Items by category.
  - `GET /api/menuitems/{id}`: Item details.
  - `POST /api/menuitems`: Create item.
  - `PUT /api/menuitems/{id}`: Update item.
  - `DELETE /api/menuitems/{id}`: Soft delete.

### 3.2 Floor & Session Management
- **TablesController**
  - `GET /api/tables`: List all tables and their status (using `VwTableStatus`).
  - `POST /api/tables`: Add new table.
  - `PUT /api/tables/{id}`: Update table info.

- **TableSessionsController**
  - `POST /api/tablesessions/open`: Open a new session for a table.
  - `POST /api/tablesessions/close/{id}`: Close a session.

### 3.3 Ordering
- **OrdersController**
  - `GET /api/orders/session/{sessionId}`: Get all orders for a session.
  - `POST /api/orders`: Place a new order. 
    - **Logic**: Creates `Order`, `OrderItems`, and inserts into `KitchenQueue`.
  - `GET /api/orders/{id}`: Get order details with items.

### 3.4 Kitchen Operations
- **KitchenController**
  - `GET /api/kitchen/queue`: List active items in queue (using `VwKitchenQueue`).
  - `PUT /api/kitchen/status/{queueId}`: Update status (e.g., "waiting" -> "cooking" -> "done").
    - **Logic**: Also updates the status in the corresponding `OrderItem`.

### 3.5 Inventory
- **IngredientsController**
  - `GET /api/ingredients`: List ingredients and stock levels.
  - `GET /api/ingredients/low-stock`: List ingredients below `MinStock` (using `VwLowStockIngredient`).
- **StockTransactionsController**
  - `POST /api/stock/transaction`: Log a stock update (in/out).

### 3.6 Financials & Reporting
- **PaymentsController**
  - `POST /api/payments`: Record a payment for an order.
- **ReportsController**
  - `GET /api/reports/revenue/daily`: Daily revenue list.
  - `GET /api/reports/revenue/monthly`: Current month revenue (using `VwRevenueThisMonth`).

### 3.7 Admin
- **AdminsController**
  - `GET /api/admins`: List admins.
  - `POST /api/admins/login`: Simple credential check.

## 4. Implementation Details
- **Error Handling**: Standard try-catch with problem details response.
- **Validation**: Use Data Annotations on DTOs.
- **CORS**: Enabled for Flutter app access.

## 5. Success Criteria
- All entities have corresponding CRUD endpoints.
- Placing an order successfully populates items and kitchen queue.
- Kitchen status updates reflect in order items.
- Reporting views are accessible via API.
