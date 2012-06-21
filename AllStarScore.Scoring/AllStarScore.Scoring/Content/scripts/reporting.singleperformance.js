$(document).ready(function () {
    var viewModel = ko.mapping.fromJS({ viewModel: window.reportingData }, mapping);
    ko.applyBindings(viewModel, document.getElementById('reporting_singleperformance'));
});


var mapping = {
    'viewModel': {
        create: function (options) {
            return new ReportingViewModel(options.data);
        }
    }
};

var ReportingViewModel = function (data) {
    var self = this;

    ko.mapping.fromJS(data, mapping, self);

    self.asArray = function (obj) {
        var result = [];
        for (var key in obj) {
            result.push({ key: key, value: obj[key] });
        }

        return result;
    };
}