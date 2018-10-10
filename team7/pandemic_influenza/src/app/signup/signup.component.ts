import { Component, OnInit } from '@angular/core';
import { routerTransition } from '../router.animations';
import { Provider } from '../shared/models/provider';
import { User } from '../shared/models/user';
import { UserService } from '../shared/services/user.service';
import {mergeMap} from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-signup',
    templateUrl: './signup.component.html',
    styleUrls: ['./signup.component.scss'],
    animations: [routerTransition()]
})
export class SignupComponent implements OnInit {
    provider = new Provider();
    user = new User();
    loading = false;
    error: string;
    states = [
       {id: "GA", name: "Georgia"},
       {id: "TX", name: "Texas"},
       {id: "TN", name: "Tennesse"},
       {id: "NY", name: "New York"},
       {id: "NZ", name: "New Zersey"}
     ];

    constructor(private router: Router,
            private route: ActivatedRoute,
            private userService: UserService) {}

    ngOnInit() {}
    
    createProvider() {
            this.loading = true;
            
            this.user.location = this.provider.state;
            this.user.userStatus = "pending";
            this.provider.userName = this.user.userName;
            
            this.userService.createUser(this.user)
                .pipe(mergeMap(result => {
                    console.log( result)             
                    return this.userService.createProvider(this.provider); 
                }))
                .subscribe(
                (newResult: any) => {                
                    this.loading = false;
                    console.log(newResult);
                    this.router.navigate(['./message']);
                },
                err => {
                    this.error = err.error.error_description;
                    this.loading = false;
                }
                );
    }
}
