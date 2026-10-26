// Misma "forma" que la clase Alumno.cs del backend.
// El JSON que manda la API llega con estos mismos campos.
export interface Alumno {
  id: number;
  nombre: string;
  grupo: string;
  promedio: number;
}

// Para crear un alumno todavía no hay Id: lo asigna la base de datos.
export type AlumnoNuevo = Omit<Alumno, 'id'>;
