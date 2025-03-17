#THIS IS NOT THE FINAL VERSION - STILL IN DEVELOPMENT

# Dot Net API Project

This project is a .NET API built using Entity Framework, AutoMapper, and SQLite. It provides endpoints for managing `Rangos` and `Ingredientes`.

## Prerequisites

- .NET 8.0 SDK
- Visual Studio Code (recommended)
- SQLite Database

## Installation

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd dot-net-api
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Build the project:**
   ```bash
   dotnet build
   ```

4. **Run the project with hot reload:**
   ```bash
   dotnet watch run
   ```

## Database Setup

The database is configured using Entity Framework with SQLite. To apply migrations and update the database:

```bash
   dotnet ef database update
```

The database file (`Rango.db`) will be created automatically in the root folder.

## Endpoints

### Home
**GET** `/`
- Returns `.NET ONLINE` to confirm the API is running.

### Get All Rangos
**GET** `/rangos`
- Retrieves all `Rangos` from the database.
- Supports filtering with a query string parameter `rangoNome`.

**Example:**
```
GET /rangos?rangoNome=pasta
```

### Get a Specific Rango
**GET** `/rango/{id}`
- Retrieves a specific `Rango` by ID.

**Example:**
```
GET /rango/2
```

### Get Rango Ingredients
**GET** `/rangos/{rangoId:int}/ingredientes`
- Retrieves all ingredients associated with a specific `Rango`.

**Example:**
```
GET /rangos/2/ingredientes
```

### Database Test
**GET** `/db`
- Returns all entries from the database for verification.

## Troubleshooting

### Common Errors
1. **`Ambiguous method` error when registering AutoMapper:**
   - Ensure only one `AddAutoMapper()` method is defined in your `Program.cs`.

2. **`System.MissingMethodException` or Null Values in DTOs:**
   - Ensure your DTO properties match the database model properties exactly.

3. **Hot Reload Fails:**
   - Restart the server using `Ctrl + R` or stop and start the app again with `dotnet watch run`.


## Contact
For any questions or issues, feel free to open an issue or contact the project owner.

