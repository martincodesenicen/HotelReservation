# Hotel Reservation API

API REST para la gestión de hoteles, habitaciones y reservas desarrollada con .NET 8/.NET 9 siguiendo principios de Clean Architecture.

<img width="1024" height="1024" alt="image" src="https://github.com/user-attachments/assets/4d097cf6-49e4-4243-99a1-578436cb4314" />

## Funcionalidades

* Autenticación mediante JWT.
* Roles de usuario: `Customer`, `HotelOwner` y `Administrator`.
* Gestión de hoteles por parte de propietarios.
* Gestión de habitaciones asociadas a cada hotel.
* Creación y administración de reservas.
* Validación de disponibilidad para evitar reservas superpuestas.
* Manejo global de excepciones.

## Arquitectura

El proyecto está organizado siguiendo Clean Architecture y se divide en cuatro capas:

### Domain

Contiene las entidades, reglas de negocio y lógica de dominio. No depende de otras capas ni de librerías externas.

### Application

Incluye los casos de uso, DTOs, validaciones y contratos utilizados por la aplicación.

### Infrastructure

Implementa el acceso a datos mediante Entity Framework Core y SQL Server, además de servicios externos como el hash de contraseñas con BCrypt.

### Api

Expone los endpoints REST, configura la autenticación JWT y la documentación con Swagger.

## Tecnologías utilizadas

* C# 12
* .NET 10
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* JWT Authentication
* BCrypt.Net
* FluentValidation

## API Endpoints

<img width="1081" height="601" alt="image" src="https://github.com/user-attachments/assets/0f08e7a9-5e72-4481-976a-a5e892cac139" />

Los endpoints protegidos requieren un token JWT en el header `Authorization`:

```text
Authorization: Bearer <token>
```

### Auth (`/api/auth`)

| Método | Endpoint             | Acceso  | Descripción                      |
| ------ | -------------------- | ------- | -------------------------------- |
| POST   | `/api/auth/register` | Público | Registrar un usuario.            |
| POST   | `/api/auth/login`    | Público | Iniciar sesión y obtener un JWT. |

### Hotels (`/api/hotels`)

| Método | Endpoint      | Acceso            | Descripción                |
| ------ | ------------- | ----------------- | -------------------------- |
| GET    | `/api/hotels` | Público           | Obtener todos los hoteles. |
| POST   | `/api/hotels` | HotelOwner, Admin | Crear un hotel.            |

### Rooms (`/api/hotels/{hotelId}/rooms`)

| Método | Endpoint                      | Acceso            | Descripción                           |
| ------ | ----------------------------- | ----------------- | ------------------------------------- |
| GET    | `/api/hotels/{hotelId}/rooms` | Público           | Obtener las habitaciones de un hotel. |
| POST   | `/api/hotels/{hotelId}/rooms` | HotelOwner, Admin | Crear una habitación para un hotel.   |

### Reservations (`/api/reservations`)

| Método | Endpoint                       | Acceso      | Descripción                                   |
| ------ | ------------------------------ | ----------- | --------------------------------------------- |
| POST   | `/api/reservations`            | Autenticado | Crear una reserva.                            |
| GET    | `/api/reservations/my-history` | Autenticado | Obtener el historial de reservas del usuario. |

## Configuración

### Clonar el repositorio

```bash
git clone https://github.com/martincodesenicen/HotelReservation.git
cd HotelReservation
```

### Configurar la conexión a la base de datos

Editar `HotelReservation.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=HotelReservationDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### Aplicar migraciones

```bash
dotnet ef database update --project HotelReservation.Infrastructure --startup-project HotelReservation.Api
```

### Ejecutar el proyecto

```bash
dotnet run --project HotelReservation.Api
```

Swagger estará disponible en:

```text
https://localhost:XXXX/swagger
```

## Detalles de implementación

### Control de acceso sobre recursos

Los propietarios únicamente pueden administrar hoteles y habitaciones asociadas a su cuenta mediante validaciones basadas en el usuario autenticado.

### Validación de reservas

La disponibilidad de una habitación se verifica comprobando la superposición de fechas:

```text
ExistingCheckIn < NewCheckOut
&&
ExistingCheckOut > NewCheckIn
```

### Acceso a datos desacoplado

La capa de aplicación trabaja sobre abstracciones para evitar dependencias directas con Entity Framework Core y facilitar las pruebas.
