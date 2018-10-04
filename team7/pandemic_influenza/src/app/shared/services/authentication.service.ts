import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import * as constants from '../services/auth.constant';

@Injectable()
export class AuthenticationService {
    constructor(private httpClient: HttpClient) { }

    static AUTH_TOKEN_URL = constants.API_BASE_URL + '/users/find?from=0&order=1&size=-1';

    login(user) {
        
        const headers = new HttpHeaders()
                              .set('Content-Type', 'application/json');
        console.log(headers);
        return this.httpClient.post(AuthenticationService.AUTH_TOKEN_URL, user, {headers})
            .pipe(map((res: any) => {
              if (res) {
                return res;
              }
              return null;
            }));
    }
}
