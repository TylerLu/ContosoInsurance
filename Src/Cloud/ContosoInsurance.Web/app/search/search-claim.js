"use strict";
var SearchClaimModel = (function () {
    function SearchClaimModel(claimId, firstName, lastName, policyHolderId, claimType, claimStatus, dueDate, damanageAssessment) {
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
    return SearchClaimModel;
}());
exports.SearchClaimModel = SearchClaimModel;
//# sourceMappingURL=search-claim.js.map