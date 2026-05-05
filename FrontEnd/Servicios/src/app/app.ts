import { Component, signal } from '@angular/core';

import { DepartmentsComponent } from './departments-component/departments-component';
import { EmployeesComponent } from './employees-component/employees-component';

@Component({
  selector: 'app-root',
  imports: [DepartmentsComponent, EmployeesComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('Servicios');
}
