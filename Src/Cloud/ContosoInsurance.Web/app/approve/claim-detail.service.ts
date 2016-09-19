import {Injectable} from '@angular/core';

import {Http, Headers, Response} from '@angular/http';

import 'rxjs/add/operator/toPromise';

import {ClaimDetailModel} from './claim-detail';
import {SearchClaimModel} from '../search/search-claim';

@Injectable()
export class ClaimDetailService {

    private claimDetailUrl = 'app/claimdetail';  // URL to web api
    private claimApproveUrl = 'app/claimdetail';  // URL to web api
    private queryClaimUrl = 'app/searchClaims';  // URL to web api

    constructor(private http: Http) { }


    getClaimAsync(id: number) {
        return this.queryClaimsAsync()
            .then(claims => claims.find(claim => claim.claimId == id));
    }

    queryClaimsAsync(): Promise<SearchClaimModel[]> {
        return this.http.get(this.queryClaimUrl)
            .toPromise()
            .then(response => response.json().data as SearchClaimModel[])
            .catch(this.handleError);
    }

    approveClaim(claim: ClaimDetailModel): Promise<ClaimDetailModel> {
        return this.approve(claim);
    }

    private approve(claim: ClaimDetailModel): Promise<ClaimDetailModel> {
        let headers = new Headers({
            'Conent-Type': 'application/json'
        });

        return this.http
            .post(this.claimApproveUrl, JSON.stringify(claim), { headers: headers })
            .toPromise()
            .then(res => res.json().data)
            .catch(this.handleError);
    }


    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }

}