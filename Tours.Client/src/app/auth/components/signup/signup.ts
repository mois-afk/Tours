import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { SignupService } from '../../services/signup.service';
import { Router, RouterModule } from '@angular/router';

@Component({
	selector: 'app-signup',
	imports: [ReactiveFormsModule, RouterModule],
	templateUrl: './signup.html',
	styleUrl: './signup.scss'
})
export class Signup {
	private readonly fb = inject(FormBuilder);
	private readonly router = inject(Router);
	private readonly signupService = inject(SignupService);

	public form: FormGroup;
	public emailSent = false;
	public codeVerified = false;
	public verificationMessage = '';
	public errorMessage = '';

	constructor() {
		this.form = this.fb.group({
			email: ['', [Validators.required, Validators.email]],
			code: [''],
			username: ['', Validators.required],
			password: ['', Validators.required]
		});
	}

	public sendCodeToEmail() {
		const email = this.form.get('email')?.value;
		this.signupService.sendCodeToEmail(email).subscribe({
			next: (res) => {
				if (res.success) {
					this.emailSent = true;
				} else {
					this.errorMessage = 'Email уже занят или ошибка отправки.';
				}
			},
			error: () => this.errorMessage = 'Ошибка при отправке кода.'
		});
  	}

	public verifyCode() {
		const code = this.form.get('code')?.value;
		this.signupService.verifyEmailCode(code).subscribe({
			next: (res) => {
				if (res === 'success') {
					this.codeVerified = true;
					this.verificationMessage = 'Код подтвержден';
				} else {
					this.codeVerified = false;
					this.verificationMessage = 'Неверный код подтверждения';
				}
			},
			error: () => this.verificationMessage = 'Ошибка проверки кода'
		});
	}

	public onSubmit() {
		if (this.form.invalid) return;

		this.signupService.register(this.form.value).subscribe({
			next: () => this.router.navigate(['/login']),
			error: (data) => this.errorMessage = data['error'].error
		});
	}
}
