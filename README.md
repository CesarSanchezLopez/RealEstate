# Real Estate API

Este proyecto es una API desarrollada en **.NET 7** siguiendo principios de **Clean Architecture**.  
Permite gestionar propiedades inmobiliarias con sus imágenes, precios y trazabilidad de cambios.

## Funcionalidades principales

- **Create Property Building** → Crear nuevas propiedades.
- **Add Image from Property** → Agregar imágenes a las propiedades.
- **Change Price** → Cambiar el precio de una propiedad con registro histórico (Property Trace).
- **Update Property** → Actualizar la información de una propiedad existente.
- **List Property with Filters** → Consultar propiedades aplicando filtros (nombre y rango de precios).

## 📦 Tecnologías

- ⚙️ **Backend**: ASP.NET Core (.NET 7) + Entity Framework Core
- 🗃️ **Base de Datos**: SQL Server

## ✅ Requisitos

### Backend (.NET Core API)

- SQL Server instalado o disponible online


### Pasos
 
 
### 1. Clona el repositorio

	git clone https://github.com/TU_USUARIO/ManageVM.git
	cd ManageVM

### 2.  Configura la base de datos

Configura la cadena de conexión al archivo:

/Backend/Api/appsettings.Development.json

{
  "ConnectionStrings": {
  "RealEstateConnection": "Server=.\\SQLEXPRESS;Database=RealEstateDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}

### 3. Ejecuta las migraciones

cd Backend/

dotnet ef migrations add InitialCreate -p Infrastructure/RealEstate.Infrastructure.csproj -s Api/RealEstate.Api.csproj
dotnet ef database update -p Infrastructure/RealEstate.Infrastructure.csproj -s Api/RealEstate.Api.csproj


### 4. Corre el Backend

dotnet run --project Backend/Api/RealEstate.Api



### (Opcional) Frontend (Angular) de pruebas

- Node.js v16+
- Angular CLI:
  npm install -g @angular/cli
  
