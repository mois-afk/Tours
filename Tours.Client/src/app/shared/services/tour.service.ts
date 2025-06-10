// tour.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../enviroments/enviroment.dev';

export interface Tour {
	tourId: string;
	tourName: string;
	tourDescription: string;
	tourPrice: number;
	url: string;
	startDate: string; // ISO string
	endDate: string;   // ISO string
}

export interface TourModel {
	tourName: string;
	tourDescription: string;
	tourPrice: number;
	startDate: Date;
	attractions: Attraction[];
	attractionDate: { [key: string]: string }; // key = attractionId, value = date string
}

export interface Attraction {
	attractionId: string;
	attractionName: string;
	attractionDescription: string;
	attractionPhotoUrl: string;
	cityId: string;
}

export interface AttractionVisit {
	attraction: Attraction;
	visitDate: string;
}

@Injectable({ providedIn: 'root' })
export class TourService {
	private readonly apiUrl = environment.apiBaseUrl + '/api/tour';
	constructor(private http: HttpClient) {}

	public getTours(): Observable<Tour[]> {
		return this.http.get<Tour[]>(`${this.apiUrl}`); // ваш эндпоинт
	}

	public getTourDetails(tourId: string): Observable<AttractionVisit[]> {
		return this.http.get<AttractionVisit[]>(`${this.apiUrl}/${tourId}/details`, {
			params: { tourId }
		});
	}

	public create(tour: TourModel): Observable<any> {
		return this.http.post(`${this.apiUrl}/create`, tour);
	}

	public addToOrder(tourId: string): Observable<any> {
		return this.http.post(`${environment.apiBaseUrl}/api/order/add-tour`, null, { params: { tourId: tourId } });
	}

	public addToMyTour(tourId: string): Observable<any> {
		return this.http.post('/Tour/AddToMyTour', { tourId });
	}

	public getTickets(tourId: string): Observable<any> {
		const params = new HttpParams().set('tourId', tourId.toString());
		return this.http.get(`${environment.apiBaseUrl}/api/ticket/tikets`, { params: { tourId: tourId } });
	}
}
