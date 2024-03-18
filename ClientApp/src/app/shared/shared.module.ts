import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotFoundComponent } from './components/errors/not-found/not-found.component';
import { ValidationMessagesComponent } from './components/errors/validation-messages/validation-messages.component';
import { Router, RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { NotificationComponent } from './components/modals/notification/notification.component';
import { ModalModule } from 'ngx-bootstrap/modal';



@NgModule({
  declarations: [
    NotFoundComponent,
    ValidationMessagesComponent,
    NotificationComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    ModalModule.forRoot()
  ],
  exports:[
    RouterModule,
    ReactiveFormsModule,
    ValidationMessagesComponent

  ]
})
export class SharedModule { }
