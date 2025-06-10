import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Tour } from './tour.service';
import { environment } from '../../../enviroments/enviroment.dev';

export interface OrderResponse {
  tours: Tour[];
  total: number;
}

@Injectable({ providedIn: 'root' })
export class OrderService {
	private readonly apiUrl = environment.apiBaseUrl + '/api/order';
	constructor(private http: HttpClient) {}

	getOrderList() {
		return this.http.get<OrderResponse>(`${this.apiUrl}`);
	}

	removeItem(tourId: string) {
		return this.http.delete(`${this.apiUrl}/remove-tour`, { params: { tourId: tourId } });
	}

	submitOrder() {
		return this.http.post('/api/order/submit', {});
	}
}