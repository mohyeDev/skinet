 import { inject, Injectable } from '@angular/core';
 import {loadStripe, Stripe, StripeAddressElement, StripeAddressElementOptions, StripeElements} from '@stripe/stripe-js'
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { CartService } from './cart.service';
import { Cart } from '../../shared/models/cart';
import { firstValueFrom, map } from 'rxjs';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class StripeService {
  baseUrl = environment.apiUrl;
  private cartService = inject(CartService);
  private http = inject(HttpClient);
  private stripePromise? : Promise<Stripe | null>;
  private elements? : StripeElements;
  private addressElement? : StripeAddressElement
  private accountService = inject(AccountService);

  constructor(){
    this.stripePromise = loadStripe(environment.stripePublicKey);
  }

  getStripeInstance(){
    return this.stripePromise;
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
        const user = this.accountService.currentUser();
        let defaultValues : StripeAddressElementOptions['defaultValues'] = {};

        if(user){
          defaultValues.name = user.firstName + ' ' + user.lastName;
        }

        if(user?.address != null)
        {

          console.log(user.address);
          
          
          
          defaultValues.address = {
            line1 : user.address.line1 ,
            line2  : user.address.line2,
            city : user.address.city ,
            state : user.address.state , 
            postal_code : user.address.postalCode,
            country : user.address.country

          }
        }
        const options : StripeAddressElementOptions = {
          mode : 'shipping', 
          defaultValues
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
    const cart = this.cartService.cart();
    if(!cart) throw new Error('Problem With Cart!');
    return this.http.post<Cart>(this.baseUrl + 'payment/' + cart.id,{}).pipe(
      map(cart => {
        this.cartService.setCart(cart);
        return cart
      })
    );
  }

  disposeElement(){
    this.elements= undefined ;
    this.addressElement = undefined ;
  }
}
