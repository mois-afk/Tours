import { CommonModule, DatePipe, NgClass } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
	selector: 'app-tickets-modal',
	imports: [DatePipe, NgClass, CommonModule],
	templateUrl: './tickets-modal.html',
	styleUrl: './tickets-modal.scss'
})
export class TicketsModal {
	@Input() tickets: Ticket[] = [];

	readonly moscowOffset = 3;
	modal: any;

	getTimeOffset(city: string): string {
		const timeZoneMap: Record<string, number> = {
		'Москва': 3,
		'Санкт-Петербург': 3,
		'Калининград': 2,
		'Иркутск': 8
		};
		const offset = timeZoneMap[city] ?? this.moscowOffset;
		const diff = offset - this.moscowOffset;
		return `(${diff >= 0 ? '+' : ''}${diff})`;
	}

	getFilteredTickets(type: 'Самолёт' | 'Поезд') {
		return this.tickets.filter(ticket => ticket.transportType === type);
	}

	getRowClass(status: string): string {
		return {
		success: 'table-success',
		warning: 'table-warning',
		danger: 'table-danger'
		}[status] ?? '';
	}
}

export interface Ticket {
	transportType: 'Самолёт' | 'Поезд';
	fromCity: string;
	toCity: string;
	departureDate: string; // ISO строка
	arrivalDate: string;
	price: number;
	status: 'success' | 'warning' | 'danger' | string;
}