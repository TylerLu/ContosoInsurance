import {Injectable} from '@angular/core';

import {Http, Headers, Response} from '@angular/http';

import 'rxjs/add/operator/toPromise'; 

import {SearchClaimModel} from './search-claim';


@Injectable()
export class ClaimSearchService {

    private queryClaimUrl = 'app/searchClaims';  // URL to web api

    constructor(private http: Http) { }

    queryClaimsAsync(searchClaim: SearchClaimModel): Promise<SearchClaimModel[]> {
        return this.http.get(this.queryClaimUrl)
            .toPromise()
            .then(response => response.json().data as SearchClaimModel[])
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }

}