$(document).ready(function () {
    var viewModel = ko.mapping.fromJS({ viewModel: window.reportingData }, mapping);
    ko.applyBindings(viewModel, document.getElementById('reporting_twoperformance'));
});


var mapping = {
    //    'viewModel': {
    //        create: function (options) {
    //            return new FiveJudgePanelViewModel(options.data);
    //        }
    //    },
    //    'performance': {
    //        create: function (options) {
    //            return new PerformanceModel(options.data);
    //        }
    //    }
};