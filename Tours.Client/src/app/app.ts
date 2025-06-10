import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { Header } from './shared/components/header/header';
import { CommonModule } from '@angular/common';

@Component({
	selector: 'app-root',
	imports: [
		RouterOutlet,
		RouterModule,
		CommonModule, 
		Header
	],
	templateUrl: './app.html',
	styleUrl: './app.scss'
})
export class App {
	protected title = 'Tours.WebClient';
}
