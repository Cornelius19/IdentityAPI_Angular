import { Component, OnInit } from '@angular/core';
import { AccountService } from '../account.service';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit{
  registerForm: FormGroup = new FormGroup({});
  submitted = false;
  errorMessager: string[] = [];
  
  constructor(private accountService: AccountService,private formBuilder:FormBuilder){}
  
  ngOnInit(): void {
    this.initializeFomr();
  }

  initializeFomr(){
    this.registerForm = this.formBuilder.group({
      firstName:['',[Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
      lastName:['',[Validators.required, Validators.minLength(3), Validators.maxLength(15)]],
      email:['',[Validators.required,Validators.pattern('^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$')]],
      password:['',[Validators.required, Validators.minLength(6), Validators.maxLength(15)]]
    })
  }






  register(){
    this.submitted = true;
    this.errorMessager = [];

    if(this.registerForm.valid){
      this.accountService.register(this.registerForm.value).subscribe({
        next:(response) => {
          console.log(response);
        },
        error: error => {
          console.log(error);
          
          if (error.error.errors){
            this.errorMessager = error.error.errors
          }else{
            this.errorMessager.push(error.error);
          }
        }
        
      })
    }
    
  }

}
