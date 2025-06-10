import { CommonModule, DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { Tour } from '../shared/services/tour.service';
import { OrderService } from '../shared/services/order.service';

@Component({
	selector: 'app-orders',
	imports: [CommonModule],
	templateUrl: './orders.html',
	styleUrl: './orders.scss'
})
export class Orders {
	tours: Tour[] = [];
	groupedTours: {
		tourId: string;
		tourName: string;
		tourDescription: string;
		tourPrice: number;
		startDate: string;
		endDate: string;
		count: number;
	}[] = [];

	total: number = 0;

	constructor(private orderService: OrderService) {}

	ngOnInit() {
		this.orderService.getOrderList().subscribe(res => {
			this.tours = res.tours;
			this.total = res.total;
			this.groupTours();
		});
	}

	groupTours() {
		const map = new Map<string, any>();

		for (const tour of this.tours) {
			if (!map.has(tour.tourId)) {
				map.set(tour.tourId, { ...tour, count: 1 });
			} else {
				map.get(tour.tourId).count++;
			}
		}

		this.groupedTours = Array.from(map.values());
	}

	removeTour(tourId: string) {
		this.orderService.removeItem(tourId).subscribe(() => {
			this.tours = this.tours.filter(t => t.tourId !== tourId);
			this.groupTours();
			this.total = this.tours.reduce((sum, t) => sum + t.tourPrice, 0);
		});
	}

	submitOrder() {
		this.orderService.submitOrder().subscribe(() => {
			alert('Заказ оформлен!');
			this.tours = [];
			this.groupedTours = [];
			this.total = 0;
		});
	}
}
