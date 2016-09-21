export class SearchClaimModel {
    claimId: number;
    firstName: string;
    lastName: string;
    policyHolderId: number;
    claimType: string;
    claimStatus: string;
    dueDate: Date;
    damangeAssessment: string;

    constructor(claimId: number, firstName: string, lastName: string, policyHolderId: number,claimType:string,claimStatus:string,dueDate:Date,damanageAssessment:string) {
        this.claimId = claimId;
        this.firstName = firstName;
        this.lastName = lastName;
        this.policyHolderId = policyHolderId;
        this.dueDate = dueDate;
        this.claimType = claimType;
        this.claimStatus = claimStatus;
        this.damangeAssessment = damanageAssessment;
        this.dueDate.toLocaleDateString();
    }
}