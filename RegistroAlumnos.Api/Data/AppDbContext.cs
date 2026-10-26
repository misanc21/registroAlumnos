using Microsoft.EntityFrameworkCore;
using RegistroAlumnos.Api.Models;

namespace RegistroAlumnos.Api.Data;

// El DbContext es el "puente" entre nuestro código C# y la base de datos.
// Gracias a él escribimos C# y Entity Framework lo traduce a SQL.
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // Cada DbSet representa una tabla. Esta es la tabla Alumnos.
    public DbSet<Alumno> Alumnos => Set<Alumno>();
}
