import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../../enviroments/enviroment.dev";
import { Attraction } from "./tour.service";

@Injectable({
	providedIn: 'root'
})
export class AttractionsService {
	private readonly apiUrl = environment.apiBaseUrl + '/api/attraction';

	constructor(private readonly http: HttpClient) {}

	public getByCity(cityId: string) {
		return this.http.get<Attraction[]>(`${this.apiUrl}/by-city`, { params: { cityId } })
	}

	public get() {
		return this.http.get<Attraction[]>(`${this.apiUrl}`);
	}
}