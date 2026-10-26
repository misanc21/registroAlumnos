using Microsoft.EntityFrameworkCore;
using RegistroAlumnos.Api.Data;
using RegistroAlumnos.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------------------------------------------------
// 1. SERVICIOS: aquí "registramos" lo que la aplicación va a necesitar
// ----------------------------------------------------------------------

// Base de datos SQLite. La cadena de conexión está en appsettings.json.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Alumnos")));

// CORS: por seguridad, el navegador bloquea que una página (localhost:4200)
// llame a otro servidor (localhost:5000). Aquí le damos permiso a Angular.
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()));

builder.Services.AddOpenApi();

var app = builder.Build();

// ----------------------------------------------------------------------
// 2. BASE DE DATOS: si el archivo alumnos.db no existe, se crea aquí
// ----------------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    // Datos de ejemplo para que la tabla no aparezca vacía la primera vez.
    if (!db.Alumnos.Any())
    {
        db.Alumnos.AddRange(
            new Alumno { Nombre = "Ana López Ruiz", Grupo = "3°B", Promedio = 8.7m },
            new Alumno { Nombre = "Carlos Méndez Soto", Grupo = "3°A", Promedio = 9.2m },
            new Alumno { Nombre = "Lucía Torres Vega", Grupo = "2°C", Promedio = 7.5m });
        db.SaveChanges();
    }
}

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// ----------------------------------------------------------------------
// 3. ENDPOINTS: las rutas del CRUD. Todas empiezan con /api/alumnos
// ----------------------------------------------------------------------
var alumnos = app.MapGroup("/api/alumnos");

// READ: obtener todos los alumnos
alumnos.MapGet("/", async (AppDbContext db) =>
    await db.Alumnos.OrderBy(a => a.Nombre).ToListAsync());

// READ: obtener un alumno por su Id
alumnos.MapGet("/{id}", async (int id, AppDbContext db) =>
    await db.Alumnos.FindAsync(id) is Alumno alumno
        ? Results.Ok(alumno)
        : Results.NotFound());

// CREATE: guardar un alumno nuevo
alumnos.MapPost("/", async (Alumno alumno, AppDbContext db) =>
{
    var errores = Validar(alumno);
    if (errores.Count > 0) return Results.ValidationProblem(errores);

    db.Alumnos.Add(alumno);
    await db.SaveChangesAsync();   // aquí la base de datos le asigna el Id

    return Results.Created($"/api/alumnos/{alumno.Id}", alumno);
});

// UPDATE: cambiar los datos de un alumno existente
alumnos.MapPut("/{id}", async (int id, Alumno datos, AppDbContext db) =>
{
    var alumno = await db.Alumnos.FindAsync(id);
    if (alumno is null) return Results.NotFound();

    var errores = Validar(datos);
    if (errores.Count > 0) return Results.ValidationProblem(errores);

    alumno.Nombre = datos.Nombre;
    alumno.Grupo = datos.Grupo;
    alumno.Promedio = datos.Promedio;
    await db.SaveChangesAsync();

    return Results.Ok(alumno);
});

// DELETE: borrar un alumno
alumnos.MapDelete("/{id}", async (int id, AppDbContext db) =>
{
    var alumno = await db.Alumnos.FindAsync(id);
    if (alumno is null) return Results.NotFound();

    db.Alumnos.Remove(alumno);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();

// ----------------------------------------------------------------------
// Reglas de validación: la API nunca debe confiar en lo que le mandan
// ----------------------------------------------------------------------
static Dictionary<string, string[]> Validar(Alumno alumno)
{
    var errores = new Dictionary<string, string[]>();

    if (string.IsNullOrWhiteSpace(alumno.Nombre))
        errores["nombre"] = ["El nombre es obligatorio."];

    if (string.IsNullOrWhiteSpace(alumno.Grupo))
        errores["grupo"] = ["El grupo es obligatorio."];

    if (alumno.Promedio < 0 || alumno.Promedio > 10)
        errores["promedio"] = ["El promedio debe estar entre 0 y 10."];

    return errores;
}
