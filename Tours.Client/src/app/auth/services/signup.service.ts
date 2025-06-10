import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../../enviroments/enviroment.dev";

@Injectable({ providedIn: 'root' })
export class SignupService
{
	private readonly apiUrl = environment.apiBaseUrl + '/api/auth';

  	constructor(private http: HttpClient) {}

	public register(data: any): Observable<any> {
		return this.http.post(`${this.apiUrl}/register`, data);
	}

	public sendCodeToEmail(email: string): Observable<any> {
		return this.http.post(`${this.apiUrl}/send-verification-code`, null, { params: { email } });
	}

	public verifyEmailCode(code: string): Observable<string> {
		return this.http.get(`${this.apiUrl}/verify-email`, { params: { code }, responseType: 'text' });
	}
}

export interface LoginRequest {
	email: string;
	password: string;
}