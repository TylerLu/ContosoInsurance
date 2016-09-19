import {Component} from '@angular/core';

@Component({
    selector: 'nav-header',
    templateUrl: 'app/nav/nav.component.html',
    styleUrls: ['app/shared/style.css'],
})

export class NavComponent {
    //login info
    loginName: string = "John Smith";
    loginStatus: string = "logged in"; 
}