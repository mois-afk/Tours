import { Component, inject } from '@angular/core';
import { AuthService } from '../shared/services/auth.service';

@Component({
	selector: 'app-home',
	imports: [],
	templateUrl: './home.html',
	styleUrl: './home.scss'
})
export class Home {
	private readonly authService = inject(AuthService);

	public get isLoggedIn() {
		return this.authService.isAuthenticated;
	}
}
