import { DatePipe } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { AttractionVisit, TourService } from '../../shared/services/tour.service';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
	selector: 'app-details',
	imports: [DatePipe],
	templateUrl: './details.html',
	styleUrl: './details.scss'
})
export class Details implements OnInit, OnDestroy {
	public tourId = 'someTourId'; // задай актуальный tourId
	public attractions: AttractionVisit[] = [];

	private routeSub: Subscription | null = null;

	constructor(private tourService: TourService, private route: ActivatedRoute) {}

	public ngOnInit(): void {
		this.routeSub = this.route.params.subscribe(params => {
			this.tourId = params['id'];
		});

		this.loadAttractions();
	}

	public ngOnDestroy(): void {
		this.routeSub?.unsubscribe();
	}

	private loadAttractions() {
		this.tourService.getTourDetails(this.tourId).subscribe({
			next: (data) => (this.attractions = data),
			error: (err) => console.error(err),
		});
	}
}
