import { Component,OnInit} from '@angular/core';

import {ClaimSearchService} from './claim-search.service';
import {SearchClaimModel} from './search-claim';


@Component({
    selector: 'claim-search',
    templateUrl: 'app/search/claim-search.component.html',
    styleUrls: ['app/shared/style.css'],
})
export class ClaimSearchComponent implements OnInit{

    title: string = 'claim search';

    afterQuery: boolean = false;

    searchClaim: SearchClaimModel = new SearchClaimModel(100000, 'Albert', 'Xie', 200001,'','',new Date(),'');
    searchedClaimArray: SearchClaimModel[];

    constructor(private claimSearchService: ClaimSearchService) { }

    search(searchClaim: SearchClaimModel): void{
        this.claimSearchService.queryClaimsAsync(searchClaim)
            .then(claims => this.searchedClaimArray = claims);
        this.afterQuery = true; 
    }


    ngOnInit() {
    }

}
