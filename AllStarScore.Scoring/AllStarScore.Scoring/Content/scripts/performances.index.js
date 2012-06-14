$(document).ready(function () {
    var viewModel = ko.mapping.fromJS(window.performanceIndexData, mapping);
   ko.applyBindings(viewModel, document.getElementById('performance_index'));
});

var mapping = {
    'performances': {
        create: function (options) {
            return new PerformanceModel(options.data);
        }
    }
};

var PerformanceModel = function (data) {
    data.performanceTime = new Date(data.performanceTime);
    ko.mapping.fromJS(data, mapping, this);
};