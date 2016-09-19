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
var claim_search_service_1 = require('./claim-search.service');
var search_claim_1 = require('./search-claim');
var ClaimSearchComponent = (function () {
    function ClaimSearchComponent(claimSearchService) {
        this.claimSearchService = claimSearchService;
        this.title = 'claim search';
        this.afterQuery = false;
        this.searchClaim = new search_claim_1.SearchClaimModel(100000, 'Albert', 'Xie', 200001, '', '', new Date(), '');
    }
    ClaimSearchComponent.prototype.search = function (searchClaim) {
        var _this = this;
        this.claimSearchService.queryClaimsAsync(searchClaim)
            .then(function (claims) { return _this.searchedClaimArray = claims; });
        this.afterQuery = true;
    };
    ClaimSearchComponent.prototype.ngOnInit = function () {
    };
    ClaimSearchComponent = __decorate([
        core_1.Component({
            selector: 'claim-search',
            templateUrl: 'app/search/claim-search.component.html',
            styleUrls: ['app/shared/style.css'],
        }), 
        __metadata('design:paramtypes', [claim_search_service_1.ClaimSearchService])
    ], ClaimSearchComponent);
    return ClaimSearchComponent;
}());
exports.ClaimSearchComponent = ClaimSearchComponent;
//# sourceMappingURL=claim-search.component.js.map