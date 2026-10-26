import { Component, OnInit, inject, signal } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Alumno } from '../models/alumno';
import { AlumnoService } from '../services/alumno.service';

@Component({
  selector: 'app-alumnos',
  imports: [FormsModule, DecimalPipe],
  templateUrl: './alumnos.component.html',
  styleUrl: './alumnos.component.css',
})
export class AlumnosComponent implements OnInit {
  private alumnoService = inject(AlumnoService);

  // Los "signals" guardan datos. Cuando cambian, la pantalla se actualiza sola.
  alumnos = signal<Alumno[]>([]);
  error = signal('');

  // Campos del formulario
  nombre = signal('');
  grupo = signal('');
  promedio = signal<number | null>(null);

  // Si es null estamos creando; si tiene un número, estamos editando ese alumno.
  editandoId = signal<number | null>(null);

  // Se ejecuta una vez, cuando el componente aparece en pantalla.
  ngOnInit(): void {
    this.cargarAlumnos();
  }

  cargarAlumnos(): void {
    this.alumnoService.obtenerTodos().subscribe({
      next: (lista) => {
        this.alumnos.set(lista);
        this.error.set('');
      },
      error: () => this.error.set('No se pudo conectar con la API. ¿Está corriendo "dotnet run"?'),
    });
  }

  guardar(): void {
    const datos = {
      nombre: this.nombre().trim(),
      grupo: this.grupo().trim(),
      promedio: this.promedio() ?? 0,
    };

    // Validación rápida en el navegador. La API vuelve a validar por seguridad.
    if (!datos.nombre || !datos.grupo) {
      this.error.set('Escribe el nombre y el grupo.');
      return;
    }
    if (datos.promedio < 0 || datos.promedio > 10) {
      this.error.set('El promedio debe estar entre 0 y 10.');
      return;
    }

    const id = this.editandoId();
    const peticion = id === null
      ? this.alumnoService.crear(datos)            // CREATE
      : this.alumnoService.actualizar(id, datos);  // UPDATE

    peticion.subscribe({
      next: () => {
        this.limpiarFormulario();
        this.cargarAlumnos();
      },
      error: () => this.error.set('La API rechazó los datos. Revisa el formulario.'),
    });
  }

  // Pasa los datos del alumno al formulario para poder editarlos.
  editar(alumno: Alumno): void {
    this.editandoId.set(alumno.id);
    this.nombre.set(alumno.nombre);
    this.grupo.set(alumno.grupo);
    this.promedio.set(alumno.promedio);
  }

  eliminar(alumno: Alumno): void {
    if (!confirm(`¿Seguro que quieres borrar a ${alumno.nombre}?`)) return;

    this.alumnoService.eliminar(alumno.id).subscribe({
      next: () => this.cargarAlumnos(),
      error: () => this.error.set('No se pudo borrar el alumno.'),
    });
  }

  limpiarFormulario(): void {
    this.editandoId.set(null);
    this.nombre.set('');
    this.grupo.set('');
    this.promedio.set(null);
    this.error.set('');
  }
}
