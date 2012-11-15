$(document).ready(function () {
    //AllStarScore.ReportingViewModel = ko.mapping.fromJS(window.reportingData);
    AllStarScore.ReportingViewModel = new AllStarScore.ReportingViewModel(window.reportingData);
});

AllStarScore.ReportingViewModel = function (data) {
    var self = this;

    data.reporting.levels = _.sortBy(data.reporting.levels, function (level) {
        return _.indexOf(AllStarScore.ReportOrder.levels, level.key);
    });

    data.reporting.divisions = _.sortBy(data.reporting.divisions, function (division) {
        return _.indexOf(AllStarScore.ReportOrder.divisions, division.key);
    });
    //console.log(data);
    ko.mapping.fromJS(data, {}, self);
};