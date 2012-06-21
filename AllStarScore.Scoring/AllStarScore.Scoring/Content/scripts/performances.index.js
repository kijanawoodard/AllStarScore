$(document).ready(function () {

    var mapping = {
        'performances': {
            create: function (options) {
                return new performanceModel(options.data);
            }
        }
    };

    var performanceModel = function (data) {
        data.performanceTime = new Date(data.performanceTime);
        ko.mapping.fromJS(data, mapping, this);
    };

    window.viewModel.performanceViewModel = ko.mapping.fromJS(window.performanceIndexData, mapping);
});