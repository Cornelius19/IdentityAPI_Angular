import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, map } from 'rxjs';
import { AccountModule } from '../../account/account.module';
import { AccountService } from '../../account/account.service';
import { SharedService } from '../shared.service';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})

export class AuhorizationGuard{
  
  constructor(private accountService: AccountService, private sharedService: SharedService,private router:Router ){}




  canActivate(
    route: ActivatedRouteSnapshot,
    state:RouterStateSnapshot):Observable<boolean>{
      return this.accountService.user$.pipe(
        map((user : User | null) => {
          if(user){
            return true;
          }else{
            this.sharedService.showNotification(false,'Restricted Area', 'Leave immediately!!!');
            this.router.navigate(['account/login'], {queryParams: {returnUrl:state.url}});
            return false;
          }
        })
      );
    }
}
