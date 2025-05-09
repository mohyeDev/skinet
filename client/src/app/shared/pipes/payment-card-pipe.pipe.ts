import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'paymentCardPipe',
  standalone: true
})
export class PaymentCardPipePipe implements PipeTransform {

  transform(value: unknown, ...args: unknown[]): unknown {
    return null;
  }

}
