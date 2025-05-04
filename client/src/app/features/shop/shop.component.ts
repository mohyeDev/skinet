import {MatCard} from '@angular/material/card'
import { MatIcon } from '@angular/material/icon';
import {MatDialog}  from '@angular/material/dialog'
import { Product } from '../../shared/models/product';
import { MatButton } from '@angular/material/button';
import {MatPaginator, PageEvent} from '@angular/material/paginator'
import { Component, inject, OnInit } from '@angular/core';
import { ShopParams } from '../../shared/models/shopParams';
import {MatMenu, MatMenuTrigger} from '@angular/material/menu'
import { ShopService } from '../../core/services/shop.service';
import { ProductItemComponent } from "./product-item/product-item.component";
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';
import { Pagination } from '../../shared/models/pagination';

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
    MatMenuTrigger,
    MatPaginator
],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent implements OnInit{
  private shopService= inject(ShopService);
  private dialogService = inject(MatDialog);
  products? : Pagination<Product>;

  sortOptions = [
    {name : "Alphabetical",value : 'name'},
    {name : "Price: Low-High",value : 'priceAsc'},
    {name : "Price: High-Low",value : 'PriceDesc'}

  ]

  shopParams = new ShopParams();
  pageSizeOptions = [5,10,15,20];

  ngOnInit(): void {
     this.initalizeShop();
  }

  initalizeShop(){
    this.shopService.getBrands();
    this.shopService.getTypes();
    this.getProducts();

   
  }

  getProducts()
  {
    this.shopService.getProducts(this.shopParams).subscribe(
      {
        next : response => this.products = response,
        error: error => console.log(error)
        
      }
    )
  }

  handlePageEvent(event: PageEvent){
    this.shopParams.pageNumber = event.pageIndex + 1  ;
    this.shopParams.pageSize = event.pageSize ; 
    this.getProducts();
  }

 onSortChange(event: MatSelectionListChange)
 {
  const selectedOption = event.options[0];
  if(selectedOption)
  {
    this.shopParams.sort = selectedOption.value ; 
    this.shopParams.pageNumber = 1 ; 
    this.getProducts();
    
  }
 }

  openFilterDialog(){
    const dialogRef = this.dialogService.open(FiltersDialogComponent,{
       minWidth: "500px" ,
       data : {
        selectedBrands : this.shopParams.brands, 
        selectedTypes : this.shopParams.types 
       }
    });
    dialogRef.afterClosed().subscribe(
      {
        next: result => {
          if(result){
            this.shopParams.brands = result.selectedBrands;
            this.shopParams.types = result.selectedTypes ; 
            this.shopParams.pageNumber = 1 ; 

            this.shopService.getProducts(this.shopParams).subscribe({
              next: response => this.products = response,
              error: error => console.log(error)
              
            });
            
          }
        }
      }
    );
  }


}
