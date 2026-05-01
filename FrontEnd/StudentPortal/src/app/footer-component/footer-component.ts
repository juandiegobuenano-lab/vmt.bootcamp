import { Component } from '@angular/core';

@Component({
  selector: 'app-footer-component',
  standalone: true,
  imports: [],
  templateUrl: './footer-component.html',
  styleUrl: './footer-component.scss',
})
export class FooterComponent {
  year = new Date().getFullYear();
  mensaje = '';

  generar(): void {
    this.mensaje = 'Copyright: Todos los derechos reservados';
  }
}
