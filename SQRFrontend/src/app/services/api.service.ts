import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Order, SetProduction, SetProductionResponse, Production } from '../models/index';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = 'http://localhost:5267/api/orders';

  constructor(private http: HttpClient) { }

  getOrders(): Observable<{ orders: Order[] }> {
    return this.http.get<{ orders: Order[] }>(`${this.apiUrl}/GetOrders`);
  }

  setProduction(data: SetProduction): Observable<SetProductionResponse> {
    return this.http.post<SetProductionResponse>(`${this.apiUrl}/SetProduction`, data);
  }

  getProduction(email: string): Observable<{ productions: Production[] }> {
    return this.http.get<{ productions: Production[] }>(`${this.apiUrl}/GetProduction?email=${email}`);
  }
}