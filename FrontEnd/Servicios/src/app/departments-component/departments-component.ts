import { Component, inject, OnInit, signal } from '@angular/core';

import { DepartmentsService } from '../Servicios/departments-service'; 
import { Department } from '../interfaces/department.interface';

@Component({
  selector: 'app-departments-component',
  imports: [],
  templateUrl: './departments-component.html',
  styleUrl: './departments-component.scss',
})
export class DepartmentsComponent implements OnInit {
  private departmentsService = inject(DepartmentsService);

  departments = signal<Department[]>([]);
  loading = signal(false);
  error = signal('');

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {
    this.loading.set(true);
    this.error.set('');

    this.departmentsService.getAll().subscribe({
      next: (departments) => {
        this.departments.set(departments);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Error al cargar los departamentos');
        this.loading.set(false);
      },
    });
  }
}
