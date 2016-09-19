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
var router_1 = require('@angular/router');
var claim_search_service_1 = require('./claim-search.service');
var shared_1 = require('../shared/shared');
var ClaimGridComponent = (function () {
    function ClaimGridComponent(router, claimServcie) {
        this.router = router;
        this.claimServcie = claimServcie;
    }
    ClaimGridComponent.prototype.onSelect = function (selectedClaim) {
        this.selectedClaim = selectedClaim;
        this.router.navigate(['/approve', this.selectedClaim.claimId]);
    };
    ClaimGridComponent.prototype.convertDate = function (claimDateStr) {
        return shared_1.CommonUtil.getFormatDateByStr(claimDateStr);
    };
    ClaimGridComponent.prototype.aprove = function () {
    };
    ClaimGridComponent.prototype.ngOnInit = function () {
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Boolean)
    ], ClaimGridComponent.prototype, "afterQuery", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Array)
    ], ClaimGridComponent.prototype, "claims", void 0);
    ClaimGridComponent = __decorate([
        core_1.Component({
            selector: 'claim-grid',
            templateUrl: 'app/search/claim-grid.component.html',
            styleUrls: ['app/shared/style.css'],
        }), 
        __metadata('design:paramtypes', [router_1.Router, claim_search_service_1.ClaimSearchService])
    ], ClaimGridComponent);
    return ClaimGridComponent;
}());
exports.ClaimGridComponent = ClaimGridComponent;
//# sourceMappingURL=claim-grid.component.js.map