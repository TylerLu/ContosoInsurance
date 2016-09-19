"use strict";
var CommonUtil = (function () {
    function CommonUtil() {
    }
    CommonUtil.getFormatDate = function (date) {
        return (date != null && date != undefined) ? date.toLocaleDateString() : "";
    };
    CommonUtil.getFormatDateByStr = function (dateStr) {
        return new Date(dateStr).toLocaleDateString();
    };
    return CommonUtil;
}());
exports.CommonUtil = CommonUtil;
//# sourceMappingURL=shared.js.map