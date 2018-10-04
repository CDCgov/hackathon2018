import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { API_BASE_URL } from '../services/auth.constant';
import { map } from 'rxjs/operators';

@Injectable()
export class UserService {
  
  userObject: any;
  isAdmin :boolean;
  isPatient :boolean;
  isProvider :boolean;

  constructor(private httpClient: HttpClient) { }
  
  static PROVIDER_CREATE_URL = API_BASE_URL + '/providers';
  static USER_CREATE_URL = API_BASE_URL + '/users';

  login(user) {
    this.userObject = user;
    sessionStorage.setItem("LOGIN_USER", user);
  }

  logout() {
    sessionStorage.removeItem("LOGIN_USER");
  }


  isAdminUser() {
    return this.userObject.userType === 'admin';
  }
  
  isPatientUser() {
      return this.userObject.userType === 'patient';
  }
  
  isProviderUser() {
      return this.userObject.userType === 'provider';
  }
  
  reset() {
  	this.userObject = null;
	  this.isAdmin = null;
	  this.isPatient = null;
  	this.isProvider = null;
  	sessionStorage.removeItem("LOGIN_USER");
  }
  
  createUser(user) {
	const headers = new HttpHeaders()
				      .set('Content-Type', 'application/json');
		console.log(headers);
		return this.httpClient.post(UserService.USER_CREATE_URL, user, {headers})
		    .pipe(map((res: any) => {
		      if (res) {
			return res;
		      }
		      return null;
	    }));
  
  }
  
  createProvider(provider) {
          
          const headers = new HttpHeaders()
                                .set('Content-Type', 'application/json');
          console.log(headers);
          return this.httpClient.post(UserService.PROVIDER_CREATE_URL, provider, {headers})
              .pipe(map((res: any) => {
                if (res) {
                  return res;
                }
                return null;
              }));
    }
  
}
