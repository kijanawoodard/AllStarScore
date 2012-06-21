$(document).ready(function () {
    var viewModel = ko.mapping.fromJS({ viewModel: window.reportingData });
    ko.applyBindings(viewModel, document.getElementById('reporting_singleperformance'));
});