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
}
