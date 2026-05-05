import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environment/environment';
import { Department } from '../interfaces/department.interface';

@Injectable({
  providedIn: 'root',
})
export class DepartmentsService {
  private apiUrl = environment.apiUrl;
  private _http = inject(HttpClient);

  getAll(): Observable<Department[]> {
    return this._http.get<Department[]>(`${this.apiUrl}/departments`);
  }

  getById(id: string): Observable<Department> {
    return this._http.get<Department>(`${this.apiUrl}/departments/${id}`);
  }

  crearDepartment(department: Partial<Department>): Observable<Department> {
    return this._http.post<Department>(`${this.apiUrl}/departments`, department);
  }

  actualizarDepartment(id: string, department: Partial<Department>): Observable<Department> {
    return this._http.put<Department>(`${this.apiUrl}/departments/${id}`, department);
  }

  eliminarDepartment(id: string): Observable<void> {
    return this._http.delete<void>(`${this.apiUrl}/departments/${id}`);
  }
}
