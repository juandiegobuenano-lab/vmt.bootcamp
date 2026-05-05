import { Component, computed, inject, OnInit, signal } from '@angular/core';

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
  search = signal('');
  loading = signal(false);
  error = signal('');
  filteredEmployees = computed(() => {
    const value = this.search().toLowerCase().trim();

    if (!value) {
      return this.employees();
    }

    return this.employees().filter((employee) => employee.name.toLowerCase().includes(value));
  });

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

  agregarEmployee(): void {
    const payload: Partial<Employee> = {
      createdAt: new Date().toISOString(),
      name: 'Juan Buenano',
      avatar: 'https://avatars.githubusercontent.com/u/1',
      email: 'juan.buenano@gmail.com',
      phone: '0996101033',
      position: 'Junior',
      department: 'Engineering',
      salary: '1000',
    };

    this.employeesService.crearEmployee(payload).subscribe({
      next: (nuevo) => {
        this.employees.update((employees) => [nuevo, ...employees]);
      },
      error: () => {
        this.error.set('Error al crear el empleado');
      },
    });
  }

  editarEmployee(): void {
    const empleado = this.employees()[0];

    if (!empleado) {
      return;
    }

    const payload: Partial<Employee> = {
      name: `${empleado.name} Editado`,
      position: 'Empleado actualizado',
      department: empleado.department,
      email: empleado.email,
      phone: empleado.phone,
      salary: empleado.salary,
      avatar: empleado.avatar,
    };

    this.employeesService.actualizarEmployee(empleado.id, payload).subscribe({
      next: (actualizado) => {
        this.employees.update((employees) =>
          employees.map((employee) => (employee.id === empleado.id ? actualizado : employee)),
        );
      },
      error: () => {
        this.error.set('Error al editar el empleado');
      },
    });
  }

  eliminarEmployee(id: string): void {
    this.employeesService.eliminarEmployee(id).subscribe({
      next: () => {
        this.employees.update((employees) => employees.filter((employee) => employee.id !== id));
      },
      error: () => {
        this.error.set('Error al eliminar el empleado');
      },
    });
  }
}
