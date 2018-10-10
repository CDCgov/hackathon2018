import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SignupComponent } from './signup.component';
import { MessageComponent } from './message.component';

const routes: Routes = [
    {
        path: '', component: SignupComponent
    },
    {
            path: 'message', component: MessageComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class SignupRoutingModule {
}
