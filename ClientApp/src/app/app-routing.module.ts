import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './shared/components/errors/not-found/not-found.component';
import { PlayComponent } from './play/play.component';

const routes: Routes = [
  {path: '',component:HomeComponent},
  {path: 'play',component:PlayComponent},
  {path:'account',loadChildren: () => import('./account/account.module').then(module => module.AccountModule)}, //lazy loading when we are anvigating to account router is gonna load our account modules
  {path: 'not-found',component:NotFoundComponent},
  {path: '**',component:NotFoundComponent,pathMatch:'full'}//in case we put some addres that doesnt exist is going to go to not found
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
