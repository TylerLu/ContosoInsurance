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
var claim_status_1 = require('./claim-status');
var ClaimStatusComponent = (function () {
    function ClaimStatusComponent() {
    }
    ClaimStatusComponent.prototype.initStatusList = function () {
        this.statusList = [
            new claim_status_1.ClaimStatus(1, "new"),
            new claim_status_1.ClaimStatus(2, "pending"),
            new claim_status_1.ClaimStatus(3, "closed"),
        ];
    };
    ClaimStatusComponent.prototype.ngOnInit = function () {
        this.initStatusList();
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', claim_status_1.ClaimStatus)
    ], ClaimStatusComponent.prototype, "selectedStatus", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Boolean)
    ], ClaimStatusComponent.prototype, "disabled", void 0);
    ClaimStatusComponent = __decorate([
        core_1.Component({
            selector: 'claim-status',
            templateUrl: 'app/approve/claim-status.component.html',
            styleUrls: ['app/shared/style.css'],
        }), 
        __metadata('design:paramtypes', [])
    ], ClaimStatusComponent);
    return ClaimStatusComponent;
}());
exports.ClaimStatusComponent = ClaimStatusComponent;
//# sourceMappingURL=claim-status.component.js.map