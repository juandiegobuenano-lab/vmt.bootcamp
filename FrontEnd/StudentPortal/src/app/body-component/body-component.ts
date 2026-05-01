import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

interface Materia {
  nombre: string;
  creditos: number;
  aprobada: boolean;
}

@Component({
  selector: 'app-body-component',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './body-component.html',
  styleUrl: './body-component.scss',
})
export class BodyComponent {
  busqueda: string = '';
  creditos: number = 45;

  materias: Materia[] = [
    { nombre: 'Cálculo', creditos: 4, aprobada: true },
    { nombre: 'Física', creditos: 4, aprobada: false },
    { nombre: 'Programación', creditos: 3, aprobada: true },
    { nombre: 'Base de Datos', creditos: 3, aprobada: false },
    { nombre: 'Inglés', creditos: 2, aprobada: true },
  ];

  porcentaje(): number {
    return Math.round((this.creditos / 120) * 100);
  }

  sumar(): void {
    if (this.creditos < 120) {
      this.creditos += 10;
    }
  }

  restar(): void {
    if (this.creditos > 0) {
      this.creditos -= 10;
    }
  }
}
