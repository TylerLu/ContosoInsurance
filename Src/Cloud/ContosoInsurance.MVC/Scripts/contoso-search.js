;
var claimSearchFn = function () {
    var self = this;

    self.init = function () {
        self.searchForm = new self.SearchFormFn();
        self.searchForm.init();

        self.claimGrid = new self.claimGridFn();
        self.claimGrid.init();

        ko.applyBindings(self, $(".search-main").get(0));
    };

    self.relativeUrl = function (action) {
        var url = location.protocol + "//" + location.host + "/claims/" + action;
        return url;
    };


    self.Request = function (action,type,data, cb) {
        var url = self.relativeUrl(action);
        if (type == 'get')
            data["t"] = new Date().getTime();
        $.ajax({
            dataType: 'json',
            url: url,
            data: data,
            contentType: 'application/json',
            type: type,
            success: cb
        });
    };

    self.SearchFormFn = function () {
        var searchForm = this;
        searchForm.init = function () { }
        searchForm.formData = {
            firstName:ko.observable(''),
            lastName : ko.observable(''),
            policyHolderId : ko.observable(''),
            claimId: ko.observable(''),
            phFirstName: 'Enter policyHolder first name'
        };

        searchForm.getPostData = function (formData) {
            return {
                firstName: formData.firstName(),
                lastName: formData.lastName(),
                policyHolderId: formData.policyHolderId(),
                claimId:formData.claimId(),
            };
        };
        
        searchForm.doSearch = function () {
            self.Request('Search','get', searchForm.getPostData(searchForm.formData), function (data) {
                self.claimGrid.bindData(data);
                self.claimGrid.afterQuery(true);
            });
        }
    };

    self.claimGridFn = function () {
        var claimGrid = this;
        claimGrid.afterQuery = ko.observable(false);
        claimGrid.claims = ko.observableArray();
        claimGrid.bindData = function (claimsData) {
            claimGrid.claims([]);
            claimGrid.claims(claimGrid.parseData(claimsData));
        }
        claimGrid.parseData = function (claimsData) {
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
        claimGrid.showDetail = function (item) {
            location.href = self.relativeUrl('details') + '/' + item.claimId + "?t="+new Date().getTime();
        };
        claimGrid.init = function () {

        };
    };

};

$(function () {
    var claimSearch = new claimSearchFn();
    claimSearch.init();
});

