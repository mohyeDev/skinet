import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Address, User } from '../../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = environment.apiUrl;
  private http = inject(HttpClient) ; 
  currentUser = signal<User | null>(null);

  login(values :any){
    let params  = new HttpParams();
    params = params.append('userCookies' , true); 
    return this.http.post<User>(this.baseUrl + 'login' , values , {params});
  }

  register(values:any){
    return this.http.post(this.baseUrl + 'account/register' , values );
  }
 

  getUserInfo(){
    return this.http.get<User>(this.baseUrl + 'account/user-info').subscribe({
       next : user => this.currentUser.set(user),
       error : error => console.log(error)
       
    })
  }

  logout(){
    return this.http.post(this.baseUrl + 'account/logout' , {});
  }

  updateAdress(address : Address){
    
    return this.http.post(this.baseUrl + 'account/address' , address);
  }
}
