import { DatePipe } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { Tour, TourService } from '../shared/services/tour.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { AuthService } from '../shared/services/auth.service';
import { TicketsModal } from '../shared/components/tickets-modal/tickets-modal';

@Component({
	selector: 'app-tours',
	imports: [DatePipe, RouterModule],
	templateUrl: './tours.html',
	styleUrl: './tours.scss'
})
export class Tours implements OnInit {
	private readonly authService = inject(AuthService);
	private readonly tourService = inject(TourService);
	private readonly modalService = inject(NgbModal);
	
	public tours: Tour[] = [];
	public isOrderPage = false;
	public isLogPage = false;

	public ticketsHtml: string = '';
  	public selectedTourId: string | null = null;
	
	public get isLoggedIn() {
		return this.authService.isAuthenticated;
	}

	public ngOnInit() {
		this.isOrderPage = false;
		this.isLogPage = false;

		this.loadTours();
	}


	loadTours() {
		this.tourService.getTours().subscribe(tours => {
			this.tours = tours;
		});
	}

	buyTour(tourId: string) {
		this.tourService.addToOrder(tourId).subscribe({
			next: () => console.log('Добавлено в заказ'),
			error: err => console.error('Ошибка добавления в заказ', err)
		});
	}

	addToMyTour(tourId: string) {
		this.tourService.addToMyTour(tourId).subscribe({
			next: () => console.log('Добавлено в мой тур'),
			error: err => console.error('Ошибка добавления в мой тур', err)
		});
	}

	viewTickets(tourId: string) {
		this.tourService.getTickets(tourId).subscribe({
			next: tickets => {
				const modalRef = this.modalService.open(TicketsModal, { size: 'xl' });
				modalRef.componentInstance.tickets = tickets;
			},
			error: err => {
				this.ticketsHtml = '<p>Не удалось загрузить билеты. Попробуйте позже.</p>';
				console.error('Ошибка загрузки билетов', err);
				this.modalService.open(TicketsModal, { ariaLabelledBy: 'ticketsModalLabel' });
			}
		});
	}

	// viewTickets(tourId: string, content: any) {
    // 	this.selectedTourId = tourId;
    // 	this.ticketsHtml = 'Загрузка...';

	// 	this.tourService.getTickets(tourId).subscribe({
	// 		next: html => {
	// 			this.ticketsHtml = html;
	// 			this.modalService.open(content, { ariaLabelledBy: 'ticketsModalLabel' });
	// 		},
	// 		error: err => {
	// 			this.ticketsHtml = '<p>Не удалось загрузить билеты. Попробуйте позже.</p>';
	// 			console.error('Ошибка загрузки билетов', err);
	// 			this.modalService.open(content, { ariaLabelledBy: 'ticketsModalLabel' });
	// 		}
	// 	});
  	// }

	groupToursById() {
		// для isOrderPage - группировка туров по TourId
		const groups = new Map<string, Tour[]>();
		this.tours.forEach(tour => {
			if (!groups.has(tour.tourId)) {
				groups.set(tour.tourId, []);
			}
			groups.get(tour.tourId)!.push(tour);
		});
		return groups;
	}

	removeItem(tourId: string) {
		// здесь вызывайте API для удаления из заказа и обновляйте список
		console.log('Удалить тур', tourId);
		// например, после удаления
		this.tours = this.tours.filter(t => t.tourId !== tourId);
	}
}
