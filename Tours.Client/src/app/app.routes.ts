import { Routes } from '@angular/router';
import { Home } from './home/home';
import { Login } from './auth/components/login/login';
import { Signup } from './auth/components/signup/signup';
import { Tours } from './tours/tours';
import { Details } from './tours/details/details';
import { Cities } from './cities/cities';
import { Attractions } from './cities/attractions/attractions';
import { Create } from './tours/create/create';
import { Orders } from './orders/orders';

export const routes: Routes = [
	{
		path: '',
		redirectTo: 'home',
		pathMatch: 'full',
	},
	{
		path: 'home',
		component: Home,
	},
	{
		path: 'login',
		component: Login,
	},
	{
		path: 'signup',
		component: Signup,
	},
	{
		path: 'tours',
		component: Tours
	},
	{
		path: 'tours/:id',
		component: Details,
	},
	{
		path: 'cities',
		component: Cities
	},
	{
		path: 'attractions/:cityId',
		component: Attractions
	},
	{
		path: 'tour/create',
		component: Create,
	},
	{
		path: 'orders',
		component: Orders
	}
];
