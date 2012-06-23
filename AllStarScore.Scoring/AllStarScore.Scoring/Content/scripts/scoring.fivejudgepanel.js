$(document).ready(function () {
    viewModel.scoring = ko.mapping.fromJS({viewModel: window.scoringFiveJudgePanelData}, mapping);
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

    data.calculator.scores = _.sortBy(data.calculator.scores, function (judge) {
        return judge.judgeId;
    });

    ko.mapping.fromJS(data, mapping, self);

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

    self.markTeamDidNotCompete = function () {
        var form = $('#scoring_fivejudgepanel form.did_not_compete');

        form.ajaxPost({
            data: ko.mapping.toJSON({ performanceId: self.performance.id }),
            success: function (result) {
                //console.log(ko.toJSON(result));
                console.log('saved');
                $('.validation-summary-errors').empty();

                self.performance.didNotCompete(true);
            }
        });
    };

    self.markTeamDidCompete = function () {
        var form = $('#scoring_fivejudgepanel form.did_compete');

        form.ajaxPost({
            data: ko.mapping.toJSON({ performanceId: self.performance.id }),
            success: function (result) {
                //console.log(ko.toJSON(result));
                console.log('saved');
                $('.validation-summary-errors').empty();

                self.performance.didNotCompete(false);
            }
        });
    };

    self.markTeamScoringCompete = function () {
        var form = $('#scoring_fivejudgepanel form.scoring_complete');

        form.ajaxPost({
            data: ko.mapping.toJSON({ performanceId: self.performance.id }),
            success: function (result) {
                //console.log(ko.toJSON(result));
                console.log('saved');
                $('.validation-summary-errors').empty();

                self.performance.scoringComplete(true);
            }
        });
    };

    self.markTeamScoringOpen = function () {
        var form = $('#scoring_fivejudgepanel form.scoring_open');

        form.ajaxPost({
            data: ko.mapping.toJSON({ performanceId: self.performance.id }),
            success: function (result) {
                //console.log(ko.toJSON(result));
                console.log('saved');
                $('.validation-summary-errors').empty();

                self.performance.scoringComplete(false);
            }
        });
    };
};

var PerformanceModel = function (data) {
    var self = this;

    data.performanceTime = new Date(data.performanceTime);
    ko.mapping.fromJS(data, mapping, self);

    self.didCompete = ko.computed(function () {
        return !self.didNotCompete();
    }, self);

    self.scoringIsNotComplete = ko.computed(function () {
        return !self.scoringComplete();
    }, self);
};