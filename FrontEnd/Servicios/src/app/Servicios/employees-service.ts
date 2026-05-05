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
}
