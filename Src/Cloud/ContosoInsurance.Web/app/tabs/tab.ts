import { Component, Input } from '@angular/core';

@Component({
    selector: 'tab',
    templateUrl: 'app/tabs/tab.html',
    styleUrls: ['app/tabs/tabs.css'],
})
export class Tab {
    @Input('tabTitle') title: string;
    @Input() active = false;
}