// city.service.ts
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../enviroments/enviroment.dev';

export interface City {
	cityId: string;
	cityName: string;
	cityDescription: string;
	photoUrl: string;
}

@Injectable({ providedIn: 'root' })
export class CityService {
	private readonly apiUrl = environment.apiBaseUrl + '/api/city';
	
	constructor(private http: HttpClient) {}

	public getCities(): Observable<City[]> {
		return this.http.get<City[]>(`${this.apiUrl}`);
	}
}
