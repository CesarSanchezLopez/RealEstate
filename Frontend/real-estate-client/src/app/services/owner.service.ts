import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface Owner {
  idOwner: number;
  name: string;
  address: string;
  birthday: string;
}

@Injectable({
  providedIn: 'root'
})
export class OwnerService {
  //private apiUrl = 'https://localhost:7115/api/owner'; // 👈 ajusta si tu ruta es distinta
  private apiUrl = `${environment.apiUrl}/owner`;



  constructor(private http: HttpClient) {}

  getOwners(): Observable<Owner[]> {
    return this.http.get<Owner[]>(this.apiUrl);
  }

  getOwnerById(id: number): Observable<Owner> {
    return this.http.get<Owner>(`${this.apiUrl}/${id}`);
  }

  createOwner(owner: Owner): Observable<Owner> {
    return this.http.post<Owner>(this.apiUrl, owner);
  }

  updateOwner(id: number, owner: Owner): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, owner);
  }

  deleteOwner(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}