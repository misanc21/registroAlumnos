namespace RegistroAlumnos.Api.Models;

// El modelo describe la "forma" de un alumno.
// Entity Framework usa esta clase para crear la tabla Alumnos en la base de datos.
public class Alumno
{
    // Llave primaria: la base de datos le asigna un número automáticamente.
    public int Id { get; set; }

    public string Nombre { get; set; } = "";

    public string Grupo { get; set; } = "";

    // decimal es ideal para calificaciones porque no tiene errores de redondeo.
    public decimal Promedio { get; set; }
}
