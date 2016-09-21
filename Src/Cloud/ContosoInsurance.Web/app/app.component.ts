import { Component} from '@angular/core';

@Component({
    selector: 'contosoInsurance-app',
    template: `
        <nav-header></nav-header>
        <router-outlet></router-outlet>
    `,
     
})
export class AppComponent {
    title = 'Search Form';
}
