import {Customer} from './customer';
import {OtherParty} from './other-party';

import {CommonUtil} from '../shared/shared';

export class ClaimDetailModel {
    customer: Customer; 
    other: OtherParty;
    status: string;
    claimId: string;
    claimDate: Date;

    getDisplayClaimDate() {
        return CommonUtil.getFormatDate(this.claimDate);
    }

}