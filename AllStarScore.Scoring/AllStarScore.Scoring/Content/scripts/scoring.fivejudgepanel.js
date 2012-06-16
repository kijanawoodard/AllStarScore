$(document).ready(function () {
    var viewModel = ko.mapping.fromJS({ viewModel: window.scoringFiveJudgePanelData }, mapping);
    ko.applyBindings(viewModel, document.getElementById('scoring_fivejudgepanel'));
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
        var division = self.performance.divisionId();
        var level = self.performance.levelId();
        var map = self.scoringMap.templates[division] || self.scoringMap.templates[level];
        return map();
    };

    self.getScoring = function (performance, judges) {
        var division = performance.divisionId();
        var level = performance.levelId();
        var map = self.scoringMap.categories[division] || self.scoringMap.categories[level];
        
        //an array version for knockout foreach
        var categories = $.map(map, function (category, key) {
            return { key: key, category: category };
        });

        return { performance: performance, judges: judges, map: map, categories: categories };
    };
};

var PerformanceModel = function (data) {
    data.performanceTime = new Date(data.performanceTime);
    ko.mapping.fromJS(data, mapping, this);
};