import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Attraction, TourModel, TourService } from '../../shared/services/tour.service';
import { ActivatedRoute } from '@angular/router';
import { AttractionsService } from '../../shared/services/attractions.service';

@Component({
	selector: 'app-create',
	imports: [CommonModule, FormsModule],
	templateUrl: './create.html',
	styleUrl: './create.scss'
})
export class Create {
	private readonly attractionService = inject(AttractionsService);

	attractions: Attraction[] = [];
	selectedAttractions: { attractionId: string; visitDate: string }[] = [];

	tour: TourModel = {
		tourName: '',
		tourDescription: '',
		tourPrice: 0,
		startDate: new Date(),
		attractions: [],
		attractionDate: {}
	};
	selectedAttractionId = '';
	selectedDate: Date | null = null;

  	constructor(private tourService: TourService, private route: ActivatedRoute) {}

	public ngOnInit() {
		this.attractionService.get()
			.subscribe({
				next: (data) => this.attractions = data,
				complete: () => this.addAttraction()
			});
	}

	// public addAttraction() {
	// 	if (!this.selectedDate) {
	// 		alert('Выебрите дату');
	// 		return;
	// 	}

	// 	if (this.selectedDate <= this.tour.startDate) {
	// 		alert('Дата посещения не может быть раньше даты начала тура');
	// 		return;
	// 	}

	// 	this.tour.attractionDate[this.selectedAttractionId] = this.selectedDate.toLocaleString();
	// }

	public addAttraction() {
		this.selectedAttractions.push({ attractionId: '', visitDate: '' });
		this.tour.tourPrice = this.selectedAttractions.length * 1000;
	}

	public removeAttraction(index: number) {
		this.selectedAttractions.splice(index, 1);
	}

	public checkDate(datestring: string) {
		const date = new Date(datestring);
		
		if (!date) {
			alert('Выебрите дату');
			return;
		}

		if (date <= this.tour.startDate) {
			alert('Дата посещения не может быть раньше даты начала тура');
			return;
		}
	}

	// public removeAttraction(id: string) {
	// 	delete this.tour.attractionDate[id];
	// }

	public attractionIds() {
		return Object.keys(this.tour.attractionDate);
	}

	public getAttractionName(id: string) {
		return this.tour.attractions.find(a => a.attractionId === id)?.attractionName || '';
	}

	public submit() {
		if (!this.tour.tourName.trim() || !this.tour.tourDescription.trim()) {
			alert('Заполните все поля');
			return;
		}

		const filteredAttractions = this.selectedAttractions.filter(attr => attr.attractionId.length > 0 && attr.visitDate.length > 0);
		this.tour.attractionDate = {};
		for (const attr of filteredAttractions) {
			this.tour.attractionDate[attr.attractionId] = attr.visitDate;
		}

		this.tourService.create(this.tour).subscribe(() => {
			alert('Тур успешно куплен!');
		});
	}
}
