"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('@angular/core');
var http_1 = require('@angular/http');
require('rxjs/add/operator/toPromise');
var ClaimDetailService = (function () {
    function ClaimDetailService(http) {
        this.http = http;
        this.claimDetailUrl = 'app/claimdetail'; // URL to web api
        this.claimApproveUrl = 'app/claimdetail'; // URL to web api
        this.queryClaimUrl = 'app/searchClaims'; // URL to web api
    }
    ClaimDetailService.prototype.getClaimAsync = function (id) {
        return this.queryClaimsAsync()
            .then(function (claims) { return claims.find(function (claim) { return claim.claimId == id; }); });
    };
    ClaimDetailService.prototype.queryClaimsAsync = function () {
        return this.http.get(this.queryClaimUrl)
            .toPromise()
            .then(function (response) { return response.json().data; })
            .catch(this.handleError);
    };
    ClaimDetailService.prototype.approveClaim = function (claim) {
        return this.approve(claim);
    };
    ClaimDetailService.prototype.approve = function (claim) {
        var headers = new http_1.Headers({
            'Conent-Type': 'application/json'
        });
        return this.http
            .post(this.claimApproveUrl, JSON.stringify(claim), { headers: headers })
            .toPromise()
            .then(function (res) { return res.json().data; })
            .catch(this.handleError);
    };
    ClaimDetailService.prototype.handleError = function (error) {
        console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    };
    ClaimDetailService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [http_1.Http])
    ], ClaimDetailService);
    return ClaimDetailService;
}());
exports.ClaimDetailService = ClaimDetailService;
//# sourceMappingURL=claim-detail.service.js.map