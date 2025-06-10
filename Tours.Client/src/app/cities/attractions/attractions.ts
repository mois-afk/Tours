import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { CityService } from '../../shared/services/city.service';
import { AttractionsService } from '../../shared/services/attractions.service';
import { Attraction } from '../../shared/services/tour.service';

@Component({
	selector: 'app-attractions',
	imports: [],
	templateUrl: './attractions.html',
	styleUrl: './attractions.scss'
})
export class Attractions implements OnInit, OnDestroy {
	private readonly route = inject(ActivatedRoute);
	private readonly attractionsService = inject(AttractionsService);

	private cityId = '';
	private routeSub: Subscription | null = null;

	public attractions: Attraction[] = [];

	public ngOnInit() {
		this.routeSub = this.route.params.subscribe(params => {
			this.cityId = params['cityId'];
		});

		this.loadAttractions();
	}

	public ngOnDestroy(): void {
		this.routeSub?.unsubscribe();
	}

	private loadAttractions(): void {
		this.attractionsService.getByCity(this.cityId).subscribe({
			next: (data) => (this.attractions = data),
			error: (err) => console.error(err),
		});
	}
}
