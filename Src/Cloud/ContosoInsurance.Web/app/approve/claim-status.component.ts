import {Component, OnInit,Input } from '@angular/core';

import {ClaimStatus} from './claim-status';

@Component({
    selector: 'claim-status',
    templateUrl: 'app/approve/claim-status.component.html',
    styleUrls: ['app/shared/style.css'],
})

export class ClaimStatusComponent implements OnInit {

    constructor() { }
    @Input() selectedStatus: ClaimStatus;
    @Input() disabled: boolean;
    statusList: ClaimStatus[];
    initStatusList() {
        this.statusList = [
            new ClaimStatus(1, "new"),
            new ClaimStatus(2, "pending"),
            new ClaimStatus(3, "closed"),
        ];
    }

    ngOnInit(): void {
        this.initStatusList();
    }

}