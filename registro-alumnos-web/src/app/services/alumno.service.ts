import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Alumno, AlumnoNuevo } from '../models/alumno';

// El servicio es el único lugar que sabe cómo hablar con la API.
// Los componentes solo le piden cosas: "dame los alumnos", "guarda este".
@Injectable({ providedIn: 'root' })
export class AlumnoService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5000/api/alumnos';

  // READ  ->  GET /api/alumnos
  obtenerTodos(): Observable<Alumno[]> {
    return this.http.get<Alumno[]>(this.apiUrl);
  }

  // CREATE  ->  POST /api/alumnos
  crear(alumno: AlumnoNuevo): Observable<Alumno> {
    return this.http.post<Alumno>(this.apiUrl, alumno);
  }

  // UPDATE  ->  PUT /api/alumnos/5
  actualizar(id: number, alumno: AlumnoNuevo): Observable<Alumno> {
    return this.http.put<Alumno>(`${this.apiUrl}/${id}`, alumno);
  }

  // DELETE  ->  DELETE /api/alumnos/5
  eliminar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
