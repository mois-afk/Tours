import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../../enviroments/enviroment.dev";

@Injectable({ providedIn: 'root' })
export class LoginService
{
	private readonly apiUrl = environment.apiBaseUrl + '/api/auth';

  	constructor(private http: HttpClient) {}

	public login(data: LoginRequest): Observable<any> {
		return this.http.post(`${this.apiUrl}/login`, data);
	}
}

export interface LoginRequest {
	email: string;
	password: string;
}