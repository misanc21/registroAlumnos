# Registro de Alumnos

CRUD de ejemplo con **Angular** (frontend), **ASP.NET Core / .NET 10** (backend) y **SQLite** (base de datos).

```
RegistroAlumnos/
├── RegistroAlumnos.Api/    ← backend en .NET (puerto 5000)
└── registro-alumnos-web/   ← frontend en Angular (puerto 4200)
```

## Lo que necesitas instalado

- .NET SDK 10: revisa con `dotnet --version`
- Node.js LTS: revisa con `node -v`
- Angular CLI: `npm install -g @angular/cli`, y luego revisa con `ng version`
- Visual Studio Code

## Cómo ejecutarlo

Necesitas **dos terminales abiertas al mismo tiempo**, una para cada proyecto.

### Terminal 1: la API

```
cd RegistroAlumnos.Api
dotnet run
```

Espera a que diga `Now listening on: http://localhost:5000`. Déjala abierta.
La primera vez se crea solo el archivo `alumnos.db` con tres alumnos de ejemplo.

### Terminal 2: Angular

```
cd registro-alumnos-web
npm install
ng serve
```

`npm install` solo se hace la primera vez y tarda unos minutos.
Después abre **http://localhost:4200** en el navegador.

## Dónde está cada cosa

| Archivo | Qué hace |
|---|---|
| `RegistroAlumnos.Api/Models/Alumno.cs` | La forma de un alumno: Id, Nombre, Grupo y Promedio |
| `RegistroAlumnos.Api/Data/AppDbContext.cs` | Conecta C# con la base de datos |
| `RegistroAlumnos.Api/Program.cs` | Configuración y las 5 rutas del CRUD |
| `RegistroAlumnos.Api/RegistroAlumnos.Api.http` | Pruebas de la API sin usar Angular |
| `registro-alumnos-web/src/app/models/alumno.ts` | La misma forma de alumno, pero en TypeScript |
| `registro-alumnos-web/src/app/services/alumno.service.ts` | Llama a la API |
| `registro-alumnos-web/src/app/alumnos/` | La pantalla: tabla y formulario |

## Endpoints

| CRUD | Verbo | Ruta |
|---|---|---|
| Read | GET | `/api/alumnos` |
| Read | GET | `/api/alumnos/{id}` |
| Create | POST | `/api/alumnos` |
| Update | PUT | `/api/alumnos/{id}` |
| Delete | DELETE | `/api/alumnos/{id}` |

## Problemas comunes

- **La página dice "No se pudo conectar con la API".** La terminal 1 no está corriendo. Ejecuta `dotnet run` en `RegistroAlumnos.Api`.
- **`dotnet`, `node` o `ng` "no se reconoce".** Cierra VS Code y vuelve a abrirlo después de instalar.
- **`ng` no se puede ejecutar porque "la ejecución de scripts está deshabilitada".** En PowerShell ejecuta una vez `Set-ExecutionPolicy -Scope CurrentUser RemoteSigned`, o usa `npx ng serve`.
- **Quieres empezar con la base de datos limpia.** Detén la API, borra `alumnos.db` y vuelve a ejecutar `dotnet run`.

## Retos para practicar

1. Agrega un campo `Edad` al alumno, en el backend y en el frontend.
2. Muestra el promedio en rojo cuando sea menor a 6.
3. Agrega un cuadro de búsqueda para filtrar por nombre.
