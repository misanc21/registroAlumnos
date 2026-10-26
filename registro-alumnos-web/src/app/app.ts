import { Component } from '@angular/core';
import { AlumnosComponent } from './alumnos/alumnos.component';

@Component({
  imports: [AlumnosComponent],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {}
