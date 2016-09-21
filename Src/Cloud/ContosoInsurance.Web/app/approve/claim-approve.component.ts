import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {ActivatedRoute, Params} from '@angular/router';

import {ClaimDetailService} from './claim-detail.service';

import {SearchClaimModel} from '../search/search-claim';
import {ClaimDetailModel} from './claim-detail';
import {ClaimStatus} from './claim-status';

@Component({
    selector: 'claim-approve',
    templateUrl: 'app/approve/claim-approve.component.html',
    styleUrls: ['app/shared/style.css'],
})

export class ClaimApproveComponent implements OnInit {

    constructor(private claimService: ClaimDetailService, private route: ActivatedRoute) { }

    claim: ClaimDetailModel = new ClaimDetailModel();
    @Output() close = new EventEmitter();
    error: any;
    navigated = false;
    disabled = true;

    selectedStatus: ClaimStatus = new ClaimStatus(0,"");

    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            if (params["id"] != undefined) {
                let id = +params['id'];
                this.navigated = true;
                this.claimService.getClaimAsync(id)
                    .then(claim => this.claim = this.convertToClaimDetail(claim));
            } 
        });
    }

    //mock data
    convertToClaimDetail(claimModel: SearchClaimModel) {
        var result = new ClaimDetailModel();
        if (claimModel.claimId == 21) {
            result.claimId = '12345559A323P';
            result.claimDate = new Date('2016-7-8');
            result.status = claimModel.claimStatus;
            //set status
            this.selectedStatus = new ClaimStatus(1, "pending");
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
    }


    goBack(): void {
        window.history.back();
    }

    approve(): void {
        this.claimService
            .approveClaim(this.claim)
            .then(claim => {
                this.goBack();
            })
    }
}