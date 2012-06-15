$(document).ready(function () {
    var viewModel = ko.mapping.fromJS({ viewModel: window.scoringScoreEntryData }, mapping);
    ko.applyBindings(viewModel, document.getElementById('scoring_scoreentry'));
});

var mapping = {
    'viewModel': {
        create: function (options) {
            return new FiveJudgePanelViewModel(options.data);
        }
    },
    'performance': {
        create: function (options) {
            return new PerformanceModel(options.data);
        }
    }
};

var FiveJudgePanelViewModel = function (data) {
    var self = this;
    ko.mapping.fromJS(data, mapping, this);

    self.getTemplate = function () {
        console.log(self.performance);
        var division = self.performance.divisionId();
        var level = self.performance.levelId();
        var map = self.scoringMap[division] || self.scoringMap[level];
        return map();
    };
};

var PerformanceModel = function (data) {
    data.performanceTime = new Date(data.performanceTime);
    ko.mapping.fromJS(data, mapping, this);
};