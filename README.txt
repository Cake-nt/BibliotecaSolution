Sistema de Gestión de Biblioteca

Sistema completo de gestión bibliotecaria desarrollado en .NET con aplicación Windows Forms y Web ASP.NET Core MVC.

 Características:

 Funcionalidades Implementadas:

Gestión completa de Libros (CRUD, búsquedas, disponibilidad)

Gestión de Socios (CRUD, validaciones)

Sistema de Préstamos (préstamos, devoluciones, vencimientos)

Búsquedas y Filtros avanzados

Validaciones de negocio integradas

 Arquitectura:

3 Capas (Presentación, Negocio, Datos)

Patrón Repository con Entity Framework

Separación de responsabilidades

Inyección de dependencias

 Aplicaciones:

Windows Forms - Aplicación desktop completa

ASP.NET Core MVC - Aplicación web responsive

 Tecnologías Utilizadas:

.NET 10.0

Entity Framework Core

SQL Server (LocalDB)

ASP.NET Core MVC

Windows Forms

Bootstrap 5 (Web)

 Estructura del Proyecto:

BibliotecaSolution/
├──  Biblioteca.Core/          # Modelos de dominio
├──  Biblioteca.Data/          # Acceso a datos (EF Core + Repositories)  
├──  Biblioteca.Business/      # Lógica de negocio (Services)
├──  Biblioteca.WindowsForms1/ # App Desktop
└──  Biblioteca.Web/           # App Web MVC

Instalación y Configuración:

Prerrequisitos

Visual Studio 2022
.NET 10.0 SDK
SQL Server LocalDB

Pasos de instalación:

Clonar o descargar el proyecto

Configurar la base de datos

La base de datos se crea automáticamente al ejecutar la aplicación

Se pobla con datos de prueba iniciales

Compilar la solución

Ejecutar las aplicaciones

 Uso del Sistema:
Aplicación Windows Forms
Establecer como Startup Project: Biblioteca.WindowsForms1

Ejecutar (F5)

Navegar por el menú principal

Gestionar libros, socios y préstamos

Aplicación Web
Establecer como Startup Project: Biblioteca.Web

Ejecutar (F5)

Acceder en el navegador: https://localhost:7000

Utilizar todas las funcionalidades web

 Entidades del Sistema:

Libro
Título, Autor, ISBN, Género

Año de publicación, Editorial

Ejemplares totales y disponibles

Categorización

Socio
Código único, Nombre, Apellido

Email, Teléfono, Dirección

Fecha de registro, Estado

Préstamo
Código único, Fechas de préstamo y devolución

Estado (Activo, Devuelto, Vencido)

Relaciones con Libro y Socio

 Configuración de Base de Datos:

Connection String
json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BibliotecaDB;Trusted_Connection=true;"
  }
}
Migraciones
Las migraciones de Entity Framework se ejecutan automáticamente.

 Características de la Interfaz:
Windows Forms
Formularios modales para CRUD

DataGridViews con selección completa

Validaciones en tiempo real

Búsquedas instantáneas

Web ASP.NET Core MVC
Layout responsive con Bootstrap

Vistas fuertemente tipadas

Navegación intuitiva

Validaciones del lado cliente y servidor

 Funcionalidades por Módulo:
Módulo de Libros
 Crear, editar, eliminar libros

 Búsqueda por título, autor, género, ISBN

 Control de disponibilidad

 Categorización

Módulo de Socios
 Registro y gestión de socios

 Validación de datos únicos

 Control de estado (activo/inactivo)

Módulo de Préstamos
 Registro de nuevos préstamos

 Devolución de libros

 Control de vencimientos

 Filtros por estado

 Solución de Problemas Comunes:

Error: "No se puede conectar a la base de datos"
Verificar que SQL Server LocalDB esté instalado

Ejecutar: SqlLocalDB.exe start MSSQLLocalDB

Error: "Dependencias faltantes"
Restaurar paquetes NuGet: dotnet restore

Verificar que .NET 10.0 esté instalado

La aplicación web no carga el diseño
Verificar que _Layout.cshtml exista en Views/Shared/

Confirmar que Bootstrap se esté cargando

 Manual Rápido:

Crear un nuevo libro
Ir a Libros → Nuevo Libro

Completar los campos obligatorios

Verificar disponibilidad

Guardar

Registrar un préstamo
Ir a Préstamos → Nuevo Préstamo

Seleccionar libro disponible

Seleccionar socio activo

Establecer fecha de devolución

Confirmar

Buscar elementos
Usar el campo de búsqueda en cada módulo

Los filtros se aplican en tiempo real

 Flujo de Trabajo Típico:

Registrar socios en el sistema

Agregar libros al catálogo

Realizar préstamos a socios activos

Registrar devoluciones cuando corresponda

Consultar reportes de actividad

 Soporte:

Para preguntas:

Verificar la sección de solución de problemas

Revisar los logs de la aplicación

Contactar al equipo de desarrollo

 Licencia:
Este proyecto es para fines educativos y demostrativos.