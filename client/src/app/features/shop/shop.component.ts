import { Component, inject, OnInit } from '@angular/core';
import { ShopService } from '../../core/services/shop.service';
import { Product } from '../../shared/models/product';
import {MatCard} from '@angular/material/card'
import { ProductItemComponent } from "./product-item/product-item.component";
import {MatDialog}  from '@angular/material/dialog'
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import {MatMenu, MatMenuTrigger} from '@angular/material/menu'
import { MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [
    MatCard,
    ProductItemComponent,
    MatButton ,
    MatIcon,
    MatMenu,
    MatSelectionList,
    MatListOption,
    MatMenuTrigger
],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit{
  private shopService= inject(ShopService);
  private dialogService = inject(MatDialog);
  products : Product[] = [];
  selectedBrands : string[] = [] ;
  selectedTypes :string [] = [] ;
  selectedSort : string = 'name';
  sortOptions = [
    {name : "Alphabetical",value : 'name'},
    {name : "Price: Low-High",value : 'priceAsc'},
    {name : "Price: High-Low",value : 'PriceDesc'}

  ]

  ngOnInit(): void {
     this.initalizeShop();
  }

  initalizeShop(){
    this.shopService.getBrands();
    this.shopService.getTypes();
    this.getProduct();

   
  }

  getProduct()
  {
    this.shopService.getProducts(this.selectedBrands,this.selectedTypes,this.selectedSort).subscribe(
      {
        next : response => this.products = response.data,
        error: error => console.log(error)
        
      }
    )
  }

 onSortChange(event: MatSelectionListChange)
 {
  const selectedOption = event.options[0];
  if(selectedOption)
  {
    this.selectedSort = selectedOption.value ; 
    this.getProduct();
    
  }
 }

  openFilterDialog(){
    const dialogRef = this.dialogService.open(FiltersDialogComponent,{
       minWidth: "500px" ,
       data : {
        selectedBrands : this.selectedBrands,
        selectedTypes : this.selectedTypes
       }
    });
    dialogRef.afterClosed().subscribe(
      {
        next: result => {
          if(result){
            this.selectedBrands = result.selectedBrands;
            this.selectedTypes = result.selectedTypes ; 

            this.shopService.getProducts(this.selectedBrands , this.selectedTypes).subscribe(
              {
                next: response => this.products = response.data,
                error: error => console.log(error)
                
              }
            )
            
          }
        }
      }
    );
  }


}
