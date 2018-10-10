import { Component, OnInit } from '@angular/core';
import { routerTransition } from '../router.animations';

@Component({
    selector: 'signup-message',
    templateUrl: './message.component.html',
    styleUrls: ['./message.component.scss'],
    animations: [routerTransition()]
})
export class MessageComponent implements OnInit {
    constructor() {}

    ngOnInit() {}
}
