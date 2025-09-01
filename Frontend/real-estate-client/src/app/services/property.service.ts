import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PropertyDto } from '../models/property.model';
import { PropertyImageDto } from 'src/app/models/property-image.model'
import { PropertyTraceDto } from 'src/app/models/PropertyTrace.model'
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
//'../models/property-image.model';

@Injectable({
  providedIn: 'root'
})
export class PropertyService {
  //private apiUrl = 'https://localhost:7115/api';
  private apiUrl = `${environment.apiUrl}`;
  constructor(private http: HttpClient) {}

  createProperty(dto: PropertyDto): Observable<number> {
    return this.http.post<number>(`${this.apiUrl}/property`, dto);
  }

addImage(propertyId: number, file: File): Observable<any> {
  const formData = new FormData();
  formData.append('file', file); // "file" debe coincidir con el nombre del parámetro IFormFile en el backend
  return this.http.post(`${this.apiUrl}/property/${propertyId}/images`, formData);
}

changePrice(propertyId: number, newPrice: number, changedBy: string): Observable<any> {
  return this.http.patch(
    `${this.apiUrl}/property/${propertyId}/price?changedBy=${encodeURIComponent(changedBy)}`,
    newPrice,
    { headers: { 'Content-Type': 'application/json' } }
  );
}

  updateProperty(propertyId: number, dto: PropertyDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/property/${propertyId}`, dto);
  }

  listProperties(): Observable<PropertyDto[]> {
    return this.http.get<PropertyDto[]>(`${this.apiUrl}/property`);
  }
  
  getImages(propertyId: number): Observable<PropertyImageDto[]> {
  return this.http.get<PropertyImageDto[]>(`${this.apiUrl}/property/${propertyId}/images`);
}
// getTraces(propertyId: number): Observable<PropertyTraceDto[]> {
//   return this.http.get<PropertyTraceDto[]>(`${this.apiUrl}/property/${propertyId}`);
// }

getTraces(propertyId: number): Observable<PropertyTraceDto[]> {
  return this.http.get<PropertyDto>(`${this.apiUrl}/property/${propertyId}`)
    .pipe(map(p => p.traces)); // extraemos las trazas del objeto PropertyDto
}
}
