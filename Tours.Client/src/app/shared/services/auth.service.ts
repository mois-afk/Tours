import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
	providedIn: 'root'
})
export class AuthService {
	private readonly router = inject(Router);

	private readonly TOKEN_KEY = 'auth_token';
	private readonly ROLE_KEY = 'user_role';

	public setSession(token: string, role: string): void {
		localStorage.setItem(this.TOKEN_KEY, token);
		localStorage.setItem(this.ROLE_KEY, role);
	}

	public getToken(): string | null {
		return localStorage.getItem(this.TOKEN_KEY);
	}

	public getRole(): string | null {
		return localStorage.getItem(this.ROLE_KEY);
	}

	public logout(): void {
		localStorage.removeItem(this.TOKEN_KEY);
		localStorage.removeItem(this.ROLE_KEY);
		this.router.navigate(['/login']);
	}

	public get isAuthenticated(): boolean {
		return !!this.getToken();
	}

	public get isAdmin(): boolean {
		return this.getRole() === 'admin';
	}
}
