import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../../core/services/shop.service';
import { ActivatedRoute } from '@angular/router';
import { Product } from '../../../shared/models/product';
import { CurrencyPipe } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import {MatInput} from '@angular/material/input'
import { MatDivider } from '@angular/material/divider';
import { CartService } from '../../../core/services/cart.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [CurrencyPipe , MatButton , MatIcon, MatFormField,MatInput,MatLabel,MatDivider ,  FormsModule],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent implements OnInit {
  private shopService = inject(ShopService);
  private activatedRoute = inject(ActivatedRoute);
  private cartServce = inject(CartService);
  product?: Product;
  quantityInCart = 0 ;
  quanitty  = 1 ; 


  ngOnInit(): void {
    this.loadProduct();    
  }

  loadProduct(){
    const id = this.activatedRoute.snapshot.paramMap.get('id');

    if(!id) return ;
    this.shopService.getProduct(+id).subscribe({
      next: product => {
        this.product = product,
        this.updateQuantityInCart();

      },
      error: error=>console.log(error),
    })
    
  }

  updateCart(){
    if(!this.product) return ;
    if(this.quanitty > this.quantityInCart){
      const itemsToAdd = this.quanitty  - this.quantityInCart ; 
      this.quantityInCart += itemsToAdd ; 
      this.cartServce.addItemToCart(this.product,itemsToAdd);
    }
    else{
      const itemsToRemove = this.quantityInCart - this.quanitty ; 
      this.quantityInCart -= itemsToRemove ; 
      this.cartServce.removeItemFromCart(this.product.id , itemsToRemove);
    }

  }


  updateQuantityInCart(){

    this.quantityInCart =  this.cartServce.cart()
    ?.items.find(x=> x.productId === this.product?.id)
    ?.quantity ||  0 ; 
    this.quanitty = this.quantityInCart || 1 ;
    

  }

  getButtonText(){
    return this.quantityInCart > 0 ? 'update Cart' : 'Add To Cart ';
  }

}
