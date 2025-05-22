import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map, switchMap } from 'rxjs';
import { RegisterDto } from '../models/register-dto';
import { User } from '../models/user';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'http://localhost:5261/api/users';
  private apiKey = 'AIzaSyDdVU5KFv3SSBlOl0ZHVvYYLoyXi9zThko';
  private firebaseLoginUrl = `https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=${this.apiKey}`;

  constructor(private http: HttpClient) {}

  register(data: RegisterDto): Observable<User> {
    return this.http.post<User>(this.apiUrl, data);
  }

  login(email: string, password: string): Observable<any> {
    const payload = {
      email,
      password,
      returnSecureToken: true
    };

    return this.http.post(this.firebaseLoginUrl, payload).pipe(
      switchMap((res: any) => {
        localStorage.setItem('token', res.idToken);
        localStorage.setItem('email', res.email);
        return this.http.get<User[]>(this.apiUrl); 
      }),
      map(users => {
        const user = users.find(u => u.email === localStorage.getItem('email'));
        if (user) {
          localStorage.setItem('name', user.name); 
        }
        return user;
      })
    );
  }
}
