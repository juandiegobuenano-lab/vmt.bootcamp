import { Component, computed, inject, OnInit, signal } from '@angular/core';

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
  search = signal('');
  loading = signal(false);
  error = signal('');
  filteredDepartments = computed(() => {
    const value = this.search().toLowerCase().trim();

    if (!value) {
      return this.departments();
    }

    return this.departments().filter((department) => department.name.toLowerCase().includes(value));
  });

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

  agregarDepartment(): void {
    const payload: Partial<Department> = {
      createdAt: new Date().toISOString(),
      name: 'Departamento Nuevo',
      description: 'Software and Security',
      managerName: 'Juan Buenano',
    };

    this.departmentsService.crearDepartment(payload).subscribe({
      next: (nuevo) => {
        this.departments.update((departments) => [nuevo, ...departments]);
      },
      error: () => {
        this.error.set('Error al crear el departamento');
      },
    });
  }

  editarDepartment(): void {
    const department = this.departments()[0];

    if (!department) {
      return;
    }

    const payload: Partial<Department> = {
      name: `${department.name} Editado`,
      description: 'Descripcion actualizada',
      managerName: 'Manager actualizado',
    };

    this.departmentsService.actualizarDepartment(department.id, payload).subscribe({
      next: (actualizado) => {
        this.departments.update((departments) =>
          departments.map((item) => (item.id === department.id ? actualizado : item)),
        );
      },
      error: () => {
        this.error.set('Error al editar el departamento');
      },
    });
  }

  eliminarDepartment(id: string): void {
    this.departmentsService.eliminarDepartment(id).subscribe({
      next: () => {
        this.departments.update((departments) => departments.filter((department) => department.id !== id));
      },
      error: () => {
        this.error.set('Error al eliminar el departamento');
      },
    });
  }
}
