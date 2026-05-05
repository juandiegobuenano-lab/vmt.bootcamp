import { Component, inject, OnInit, signal } from '@angular/core';

import { EmployeesService } from '../Servicios/employees-service';
import { Employee } from '../interfaces/employee.interface';

@Component({
  selector: 'app-employees-component',
  imports: [],
  templateUrl: './employees-component.html',
  styleUrl: './employees-component.scss',
})
export class EmployeesComponent implements OnInit {
  private employeesService = inject(EmployeesService);

  employees = signal<Employee[]>([]);
  loading = signal(false);
  error = signal('');

  ngOnInit(): void {
    this.loadEmployees();
  }

  loadEmployees(): void {
    this.loading.set(true);
    this.error.set('');

    this.employeesService.getAll().subscribe({
      next: (employees) => {
        this.employees.set(employees);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Error al cargar los empleados');
        this.loading.set(false);
      },
    });
  }
}
