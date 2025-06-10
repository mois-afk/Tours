import { Component } from '@angular/core';
import { City, CityService } from '../shared/services/city.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
	selector: 'app-cities',
	imports: [CommonModule, RouterModule],
	templateUrl: './cities.html',
	styleUrl: './cities.scss'
})
export class Cities {
	cities: City[] = [];

  	constructor(private cityService: CityService) {}

	public ngOnInit(): void {
		this.cityService.getCities().subscribe(data => {
			this.cities = data;
		});
	}
}
