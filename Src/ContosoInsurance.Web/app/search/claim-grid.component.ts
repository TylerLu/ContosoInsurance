import { Component, OnInit, Input } from '@angular/core';
import {Router} from '@angular/router';

import {ClaimSearchService} from './claim-search.service';
import {SearchClaimModel} from './search-claim';
import {CommonUtil} from '../shared/shared';


@Component({
    selector: 'claim-grid',
    templateUrl: 'app/search/claim-grid.component.html',
    styleUrls: ['app/shared/style.css'],
})

export class ClaimGridComponent implements OnInit {

    @Input() afterQuery: boolean;
    @Input() claims: SearchClaimModel[];
    selectedClaim: SearchClaimModel;
    error: any;

    constructor(private router: Router,private claimServcie: ClaimSearchService) { }

    onSelect(selectedClaim: SearchClaimModel) {
        this.selectedClaim = selectedClaim;
        this.router.navigate(['/approve', this.selectedClaim.claimId]);
    }

    convertDate(claimDateStr: string):string{
        return CommonUtil.getFormatDateByStr(claimDateStr);
    }

    aprove() {
    }

    ngOnInit() {
    }
}