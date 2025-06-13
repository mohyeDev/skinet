import { Component, inject } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { SignalrService } from '../../../core/services/signalr.service';
import { CurrencyPipe, DatePipe, NgIf } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AddressPipe } from '../../../shared/pipes/address.pipe';
import { PaymentCardPipe } from '../../../shared/pipes/payment-card.pipe';

@Component({
  selector: 'app-checkout-success',
  standalone: true,
  imports: [MatButton ,RouterLink,CurrencyPipe,MatProgressSpinnerModule , DatePipe,CurrencyPipe,AddressPipe,PaymentCardPipe,NgIf],
  templateUrl: './checkout-success.component.html',
  styleUrl: './checkout-success.component.scss'
})
export class CheckoutSuccessComponent {

  signalrService = inject(SignalrService);




}
