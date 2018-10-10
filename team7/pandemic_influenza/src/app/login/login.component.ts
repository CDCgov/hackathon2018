import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { routerTransition } from '../router.animations';
import { AuthenticationService } from '../shared/services/authentication.service';
import { UserService } from '../shared/services/user.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    animations: [routerTransition()]
})
export class LoginComponent implements OnInit {

    model: any = {};
    loading = false;
    error = '';
    redirectUrl: string;
    
    constructor(private router: Router,
            private route: ActivatedRoute,
            private authenticationService: AuthenticationService,
            private userService: UserService) { }

    ngOnInit() {}
    
    login() {
            this.loading = true;
            this.userService.reset();
            this.authenticationService.login(this.model)
                .subscribe(
                (newResult: any) => {                
                    this.loading = false;
                    if (newResult) {
                        console.log(newResult);
                        this.userService.login(newResult);
                        this.router.navigate(['dashboard']);
                    }
                },
                err => {
                    console.log(err);
                    this.error = err.error.error_description;
                    this.loading = false;
                });
    }
}
