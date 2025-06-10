import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../shared/services/auth.service';
import { Router, RouterModule } from '@angular/router';
import { LoginRequest, LoginService } from '../../services/login.service';

@Component({
	selector: 'app-login',
	imports: [ReactiveFormsModule, RouterModule],
	templateUrl: './login.html',
	styleUrl: './login.scss'
})
export class Login {
	private readonly fb = inject(FormBuilder);
	private readonly loginService = inject(LoginService);
	private readonly router = inject(Router);
	private readonly authService = inject(AuthService);

	public form: FormGroup;
	public errorMessage: string | null = null;
	public formSubmitted: boolean = false;

	constructor() {
		this.form = this.fb.group({
			email: ['', [Validators.required, Validators.email]],
			password: ['', Validators.required]
		});
	}

	public onSubmit() {
		if (this.form.invalid) return;

		const loginData: LoginRequest = this.form.value;

		this.loginService.login(loginData).subscribe({
			next: (response) => {
				this.authService.setSession(response.token, 'user');
				this.router.navigate(['/home']);
			},
			error: (err) => {
				this.errorMessage = 'Неверный email или пароль';
			},
			complete: () => {
				this.formSubmitted = true;
			}
		});
	}
}
