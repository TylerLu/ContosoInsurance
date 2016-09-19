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
var claim_detail_service_1 = require('./claim-detail.service');
var claim_detail_1 = require('./claim-detail');
var claim_status_1 = require('./claim-status');
var ClaimApproveComponent = (function () {
    function ClaimApproveComponent(claimService, route) {
        this.claimService = claimService;
        this.route = route;
        this.claim = new claim_detail_1.ClaimDetailModel();
        this.close = new core_1.EventEmitter();
        this.navigated = false;
        this.disabled = true;
        this.selectedStatus = new claim_status_1.ClaimStatus(0, "");
    }
    ClaimApproveComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.route.params.forEach(function (params) {
            if (params["id"] != undefined) {
                var id = +params['id'];
                _this.navigated = true;
                _this.claimService.getClaimAsync(id)
                    .then(function (claim) { return _this.claim = _this.convertToClaimDetail(claim); });
            }
        });
    };
    //mock data
    ClaimApproveComponent.prototype.convertToClaimDetail = function (claimModel) {
        var result = new claim_detail_1.ClaimDetailModel();
        if (claimModel.claimId == 21) {
            result.claimId = '12345559A323P';
            result.claimDate = new Date('2016-7-8');
            result.status = claimModel.claimStatus;
            //set status
            this.selectedStatus = new claim_status_1.ClaimStatus(1, "pending");
            result.customer = {
                name: 'albert xie',
                street: '10700 NE 4th str unit 333',
                city: 'Believue',
                state: 'WA',
                zip: '98004',
                DOB: '12-12-95',
                phone: '206-291-8280',
                email: 'mpache@yahoo.com',
                policyStart: '01-01-15',
                policyEnd: '01-01-25',
                policyId: 'abc1234-po-id',
                vehicleNumber: 'sb-12344ujf-vn',
                licensePlate: 'JED-131',
                driverLicenseNumber: 'fu8744443-14#',
            };
        }
        return result;
    };
    ClaimApproveComponent.prototype.goBack = function () {
        window.history.back();
    };
    ClaimApproveComponent.prototype.approve = function () {
        var _this = this;
        this.claimService
            .approveClaim(this.claim)
            .then(function (claim) {
            _this.goBack();
        });
    };
    __decorate([
        core_1.Output(), 
        __metadata('design:type', Object)
    ], ClaimApproveComponent.prototype, "close", void 0);
    ClaimApproveComponent = __decorate([
        core_1.Component({
            selector: 'claim-approve',
            templateUrl: 'app/approve/claim-approve.component.html',
            styleUrls: ['app/shared/style.css'],
        }), 
        __metadata('design:paramtypes', [claim_detail_service_1.ClaimDetailService, router_1.ActivatedRoute])
    ], ClaimApproveComponent);
    return ClaimApproveComponent;
}());
exports.ClaimApproveComponent = ClaimApproveComponent;
//# sourceMappingURL=claim-approve.component.js.map