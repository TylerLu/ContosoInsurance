"use strict";
var shared_1 = require('../shared/shared');
var ClaimDetailModel = (function () {
    function ClaimDetailModel() {
    }
    ClaimDetailModel.prototype.getDisplayClaimDate = function () {
        return shared_1.CommonUtil.getFormatDate(this.claimDate);
    };
    return ClaimDetailModel;
}());
exports.ClaimDetailModel = ClaimDetailModel;
//# sourceMappingURL=claim-detail.js.map