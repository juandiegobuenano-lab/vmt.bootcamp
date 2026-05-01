import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-header-component',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './header-component.html',
  styleUrl: './header-component.scss',
})
export class HeaderComponent {
  nombre = 'Juan Buenaño';
  conectado = true;

  cambiarEstado(): void {
    this.conectado = !this.conectado;
  }
}
