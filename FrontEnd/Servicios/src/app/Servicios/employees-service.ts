import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environment/environment';
import { Employee } from '../interfaces/employee.interface';

@Injectable({
  providedIn: 'root',
})
export class EmployeesService {
  private apiUrl = environment.apiUrl;
  private _http = inject(HttpClient);

  getAll(): Observable<Employee[]> {
    return this._http.get<Employee[]>(`${this.apiUrl}/employees`);
  }

  getById(id: string): Observable<Employee> {
    return this._http.get<Employee>(`${this.apiUrl}/employees/${id}`);
  }

  crearEmployee(employee: Partial<Employee>): Observable<Employee> {
    return this._http.post<Employee>(`${this.apiUrl}/employees`, employee);
  }

  actualizarEmployee(id: string, employee: Partial<Employee>): Observable<Employee> {
    return this._http.put<Employee>(`${this.apiUrl}/employees/${id}`, employee);
  }

  eliminarEmployee(id: string): Observable<void> {
    return this._http.delete<void>(`${this.apiUrl}/employees/${id}`);
  }
}
