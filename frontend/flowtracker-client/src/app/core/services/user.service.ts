import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserLoginRequestInterface } from '../models/user/userLoginRequest.interface';
import { UserAuthenticationResponseInterface } from '../models/user/userAuthenticationResponse.interface';
import { UserRegisterRequestInterface } from '../models/user/userRegisterRequest.interface';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7229/api/Users';

  public loginUser(request: UserLoginRequestInterface):Observable<UserAuthenticationResponseInterface>{
    return this.http.post<UserAuthenticationResponseInterface>(`${this.apiUrl}/login`, request);    
  }

  public registerUser(request: UserRegisterRequestInterface): Observable<UserAuthenticationResponseInterface> {
    return this.http.post<UserAuthenticationResponseInterface>(`${this.apiUrl}/register`, request);
  }  
  
}
