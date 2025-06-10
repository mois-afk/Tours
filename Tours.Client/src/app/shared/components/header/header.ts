import { Component, inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { RouterModule } from '@angular/router';

@Component({
	selector: 'header',
	imports: [RouterModule],
	templateUrl: './header.html',
	styleUrl: './header.scss'
})
export class Header {
	private readonly authService = inject(AuthService);

	public get isLoggedIn() {
		return this.authService.isAuthenticated;
	}

	public get isAdmin() {
		return this.authService.isAdmin;
	}

	public logout(): void {
		return this.authService.logout();
	}
}
