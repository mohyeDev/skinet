import { RouterLink } from '@angular/router';
import { MatButton } from '@angular/material/button';
import { StripeAddressElement, StripePaymentElement } from '@stripe/stripe-js';
import { MatStepperModule } from '@angular/material/stepper';
import { MatCheckboxChange, MatCheckboxModule } from '@angular/material/checkbox';
import { StripeService } from '../../core/services/stripe.service';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { SnackbarService } from '../../core/services/snackbar.service';
import { OrderSummaryComponent } from '../../shared/components/order-summary/order-summary.component';
import { StepperSelectionEvent } from '@angular/cdk/stepper';
import { Address } from '../../shared/models/user';
import { firstValueFrom } from 'rxjs';
import { AccountService } from '../../core/services/account.service';
import { CheckoutDeliveryComponent } from "./checkout-delivery/checkout-delivery.component";

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [
    OrderSummaryComponent,
    MatStepperModule,
    RouterLink,
    MatButton,
    MatCheckboxModule,
    CheckoutDeliveryComponent
],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss',
})
export class CheckoutComponent implements OnInit, OnDestroy {
  private stripeService = inject(StripeService);
  private snackbar = inject(SnackbarService);
  private accountService = inject(AccountService);
  addressElement?: StripeAddressElement;
  paymentElement? : StripePaymentElement;
  saveAddress : boolean = false ; 


  async ngOnInit() {
    try {
      this.addressElement = await this.stripeService.createAddressElement();
      this.addressElement?.mount('#address-element');
      this.paymentElement = await this.stripeService.createPaymentElement(); 
      console.log('zxczxczxc',this.paymentElement);
      
      this.paymentElement?.mount('#payment-element');
      
    } catch (error: any) {
      this.snackbar.error(error.message);
    }
  }

  onSaveAddressCheckBoxChange(event:MatCheckboxChange){

    this.saveAddress = event.checked;

  }

  async onStepChange(event:StepperSelectionEvent)
  {

    if(event.selectedIndex === 1)
    {
      if(this.saveAddress){
        const address = await this.getAddressFromStripeAddress();
        address && firstValueFrom(this.accountService.updateAdress(address));
      }
    }

    if(event.selectedIndex === 2 ){
      await firstValueFrom(this.stripeService.createOrUpdatePaymentIntent());
    }

  }


  private async getAddressFromStripeAddress() : Promise<Address | null> {
    const result = await this.addressElement?.getValue();
    const address = result?.value.address ; 

    if(address){
      return {
        line1 : address.line1,
        line2 : address.line2 || undefined ,
        city  : address.city,
        country : address.country,
        state : address.state,
        postalCode : address.postal_code

      }
    }

    else return  null
  }



  ngOnDestroy(): void {
    this.stripeService.disposeElement();
  }
}
