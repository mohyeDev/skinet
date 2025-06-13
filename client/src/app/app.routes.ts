import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { ShopComponent } from './features/shop/shop.component';
import { ProductDetailsComponent } from './features/shop/product-details/product-details.component';
import { TestErrorComponent } from './features/test-error/test-error.component';
import { NotFoundComponent } from './shared/component/not-found/not-found.component';
import { ServerErrorComponent } from './shared/component/server-error/server-error.component';
import { CartComponent } from './features/cart/cart.component';
import { CheckoutComponent } from './features/checkout/checkout.component';
import { LoginComponent } from './features/account/login/login.component';
import { RegisterComponent } from './features/account/register/register.component';
import { authGuard } from './core/guards/auth.guard';
import { emptyCardGuard } from './core/guards/empty-card.guard';
import { CheckoutSuccessComponent } from './features/checkout/checkout-success/checkout-success.component';
import { OrderComponent } from './features/orders/order/order.component';
import { OrderDetailedComponent } from './features/orders/order-detailed/order-detailed.component';
import { orderCompleteGuard } from './core/guards/order-complete.guard';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
  },
  {
    path: 'shop',
    component: ShopComponent,
  },
  {
    path: 'shop/:id',
    component: ProductDetailsComponent,
  },

  {
    path: 'test-error',
    component: TestErrorComponent,
  },
  {
    path: 'not-found',
    component: NotFoundComponent,
  },

  {
    path: 'server-error',
    component: ServerErrorComponent,
  },

  {
    path: 'cart',
    component: CartComponent,
  },

  {
    path: 'checkout',
    component: CheckoutComponent,
    canActivate:[authGuard , emptyCardGuard]
  },

  {
    path: 'account/login',
    component : LoginComponent 
  },

  {
    path:'checkout/success' , 
    component : CheckoutSuccessComponent,
    canActivate:[authGuard , orderCompleteGuard]
  },

   
  {
    path:'order',
    component:OrderComponent,
    canActivate:[authGuard]
  },

  {
    path:'order/:id',
    component:OrderDetailedComponent,
    canActivate:[authGuard]

  },




  {
    path : 'account/register',
    component : RegisterComponent
  },

  {
    path: '**',
    redirectTo: 'not-found',
    pathMatch: 'full',
  },
];
