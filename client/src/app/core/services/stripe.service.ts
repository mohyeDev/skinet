 import { inject, Injectable } from '@angular/core';
 import {loadStripe, Stripe, StripeAddressElement, StripeAddressElementOptions, StripeElements} from '@stripe/stripe-js'
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { CartService } from './cart.service';
import { Cart } from '../../shared/models/cart';
import { firstValueFrom, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StripeService {
  baseUrl = environment.apiUrl;
  private cartSerivce = inject(CartService);
  private http = inject(HttpClient);
  private stripePromse? : Promise<Stripe | null>;
  private elements? : StripeElements;
  private addressElement? : StripeAddressElement

  constructor(){
    this.stripePromse = loadStripe(environment.stiprePublicKey);
  }

  getStripeInstance(){
    return this.stripePromse;
  };

  async initializeElements(){
    if(!this.elements){
      const stripe = await this.getStripeInstance();
      if(stripe)
      {
        const cart =await firstValueFrom(this.createOrUpdatePaymentIntent());
        this.elements = stripe.elements({clientSecret : cart.clientSecret , appearance : {labels:'floating'}})

      }

      else{
        throw new Error('Stripe Has Not been Loaded!')
      }
    }

    return this.elements;
  }

  async createAddressElement(){
    if(!this.addressElement)
    {
      const elements = await this.initializeElements();
      if(elements){
        const options : StripeAddressElementOptions = {
          mode : 'shipping',
        };

        this.addressElement = elements.create('address' , options);
      }

      else{
        throw new Error('Elements instance has not been loaded!');
      }
    }

    return this.addressElement;
  }


  createOrUpdatePaymentIntent(){
    const cart = this.cartSerivce.cart();
    if(!cart) throw new Error('Problem With Cart!');
    return this.http.post<Cart>(this.baseUrl + 'payment/' + cart.id,{}).pipe(
      map(cart => {
        this.cartSerivce.cart.set(cart);
        return cart
      })
    );
  }
}
