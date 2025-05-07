import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { AccountService } from '../../../core/services/account.service';
import { Router } from '@angular/router';
import { SnackbarService } from '../../../core/services/snackbar.service';
import { JsonPipe } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCard,
    MatFormField,
    MatLabel,
    MatInput,
    MatButton,
    JsonPipe,
    MatError
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {

  private fb = inject(FormBuilder);
  private accountService = inject(AccountService) ;
  private router = inject(Router);
  private snack = inject(SnackbarService);
  validtionErrors? : string[];

  registerFrom = this.fb.group({
    firstName : ['', Validators.required],
    lastName : ['', Validators.required],
    email : ['', [Validators.required, Validators.email]],
    password : ['', Validators.required]
  })


  onSubmit(){
    this.accountService.register(this.registerFrom.value).subscribe({
      next: () =>{
          this.snack.success('Registration Succesful -  you Can Now Login');
          this.router.navigateByUrl('/');
      },
      error:errors => this.validtionErrors = errors 
    })
  }

}
