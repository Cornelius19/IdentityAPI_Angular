import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Register } from '../shared/models/register';
import { environment } from '../../environments/environment.development';
import { Login } from '../shared/models/login';
import { User } from '../shared/models/user';
import { ReplaySubject, map, of } from 'rxjs';
import { Route, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private userSource = new ReplaySubject<User | null>(1);
  user$ = this.userSource.asObservable();

  constructor(private http:HttpClient, private router:Router) { }

  register(model: Register){
    return this.http.post(`${environment.appUrl}/api/account/register`,model)
  }

  logout(){
    localStorage.removeItem(environment.userKey);
    this.userSource.next(null);
    this.router.navigateByUrl('/')

  }

refreshUser(jwt:string|null){
  if(jwt === null){
    this.userSource.next(null);
    return of(undefined);
  }

  let headers = new HttpHeaders();
  headers = headers.set('Authorization','Bearer ' + jwt);

  return this.http.get<User>(`${environment.appUrl}/api/account/refresh-user-token`,{headers}).pipe(
    map((user:User) => {
      if(user){
        this.setUser(user);
      }
    }
  ))
}

  login(model:Login){
    return this.http.post<User>(`${environment.appUrl}/api/account/login`,model).pipe( //for returning our user details from backend such as firstName lastName and JWT token and store it in local storage and in userSource
      map((user:User) => {
        if(user){
          this.setUser(user);
        }
      })
    );;
  }

  getJWT(){
    const key = localStorage.getItem(environment.userKey);
    if(key){
      const user: User = JSON.parse(key);
      return user.jwt;
    }else{
      return null;
    }
  }

  private setUser(user :User){
    localStorage.setItem(environment.userKey, JSON.stringify(user));
    this.userSource.next(user);
  }

}
