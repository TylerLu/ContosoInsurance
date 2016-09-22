;
var claimDetailFn = function () {
    var self = this;

    self.init = function () {
        self.detail = new self.DetailFn();
        self.detail.init();

        self.tab = new self.tabFn();
        self.tab.init();

        ko.applyBindings(self, $(".claim-info").get(0));

        window.GetMap = function () {
            self.detail.showMap();
        };
    };

    self.relativeUrl = function (action) {
        var url = location.protocol + "//" + location.host + "/claims/" + action;
        return url;
    };


    self.Request = function (action, type, data, cb) {
        var url = self.relativeUrl(action);
        if (type == 'post')
            data = JSON.stringify(data);
        $.ajax({
            dataType: 'json',
            url: url,
            data: data,
            contentType: 'application/json',
            type: type,
            success: cb
        });
    };

    self.DetailFn = function () {
        var detail = this;
        detail.$dataId = "#claimData";
        detail.$data = (function () {
            return $(detail.$dataId).val();
        })();
        detail.disabled = ko.observable(true);
        detail.statusArray = ["Submitted","AutoApproved","AutoRejected","ManualApproved","ManualRejected"];
        detail.damageAssessmentArray = ["Minimal", "Moderate", "Severe"];
        detail.ViewModel = {};
        detail.init = function () {
            detail.ViewModel = (function getViewModel() {
                var viewModel = $.parseJSON(detail.$data);
                viewModel.timeLocation = {
                    hour: moment(viewModel.dateTime).format("h"),
                    min: moment(viewModel.dateTime).format("mm"),
                    apm: moment(viewModel.dateTime).format("A"),
                    month: moment(viewModel.dateTime).format("MMMM"),
                    day: moment(viewModel.dateTime).format("DD"),
                    year: moment(viewModel.dateTime).format("YYYY"),
                    location: ''
                };
                viewModel.dueDate = moment(viewModel.dueDate).format("MM-DD-YY");
                viewModel.dateTime = moment(viewModel.dateTime).format("MM-DD-YYYY");
                viewModel.customer.dob = moment(viewModel.customer.dob).format("MM-DD-YYYY");
                viewModel.customer.policyStart = moment(viewModel.customer.policyStart).format("MM-DD-YYYY");
                viewModel.otherParty.dob = moment(viewModel.otherParty.dob).format("MM-DD-YYYY");
                viewModel.hour = moment(viewModel.dateTime).format("h");
                viewModel.min = moment(viewModel.dateTime).format("mm");
                viewModel.apm = moment(viewModel.dateTime).format("A");
                viewModel.claimHisotry = detail.parseData(viewModel.claimHisotry);
                viewModel.selectedAssessment = ko.observable(viewModel.damageAssessment);
                viewModel.isShowBtn = ko.observable(viewModel.status == "AutoRejected");
                return viewModel;
            })();
        };
        detail.parseData = function (claimsData) {
            var result = [];
            $.each(claimsData, function (index, value) {
                result.push({
                    claimId: value.claimId,
                    claimType: value.claimType,
                    dueDate: moment(value.dueDate).format("MM-DD-YY"),
                    claimStatus: value.claimStatus,
                    damageAssessment: value.damageAssessment,
                    fullName: value.firstName + ", " + value.lastName,
                });
            });
            return result;
        };
        detail.approve = function () {
            detail.doSubmit(true);
        };
        detail.reject = function () {
            detail.doSubmit(false);
        };
        detail.doSubmit = function (approved) {
            var requestData = {
                id: detail.ViewModel.claimId,
                cid:detail.ViewModel.correlationId,
                approved: approved,
                damageAssessment: (function getDamageAssessment() {
                    var selected = detail.ViewModel.selectedAssessment();
                    if (selected != undefined)
                        return selected;
                    else
                        return '';
                })(),
            };
            self.Request('approve', 'post', requestData, function (data) {
                var msg = 'The Claim is ' + (approved?'approved':'rejected') + '.';
                alert(msg);
                location.href = location.href + "?t=" + new Date().getTime();
            });
        };
        detail.showDetail = function (item) {
            location.href = self.relativeUrl('details') + '/' + item.claimId;
        };
        detail.showMap = function () {
            var coordinates = detail.ViewModel.location;
            GeoHelper.BingMap.displayPin(coordinates.latitude, coordinates.longitude, "detail-map", "detail-location");
        };
    };

    self.tabFn = function () {
        var tab = this;
        tab.init = function () { }
        tab.selectTab = function (targetPnl,pnlSection,data, event) {
            $(event.currentTarget.parentElement).children().removeClass("active");
            $(event.currentTarget).addClass("active");
            $('.' + pnlSection).children().hide();
            $('.' + targetPnl).show();
        };
    };

};

$(function () {
    var claimDetail = new claimDetailFn();
    claimDetail.init();
});

