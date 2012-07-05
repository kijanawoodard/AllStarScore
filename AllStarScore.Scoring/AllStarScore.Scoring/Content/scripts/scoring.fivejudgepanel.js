$(document).ready(function () {
    viewModel.scoring = new FiveJudgePanelViewModel(window.scoringFiveJudgePanelData); 
});

var FiveJudgePanelViewModel = function (data) {
    var self = this;

    data.panel.calculator.scores = _.sortBy(data.panel.calculator.scores, function (judge) {
        return judge.judgeId;
    });

    $.extend(self, data);
   
    self.performance = new PerformanceModel(self.performance);
    self.panel.calculator = ko.mapping.fromJS(self.panel.calculator);

    self.getTemplate = function () {
        var division = self.performance.divisionId;
        var level = self.performance.levelId;
        var map = viewModel.scoringMap.templates[division] || viewModel.scoringMap.templates[level];
        return map;
    };

    self.getScoring = function (performance, panel) {
        var judges = panel.calculator.scores;
        var division = performance.divisionId;
        var level = performance.levelId;
        var map = viewModel.scoringMap.categories[division] || viewModel.scoringMap.categories[level];

        var categories = $.map(map, function (category, key) {
            var scores = {};

            _.each(judges(), function (judge) {
                scores[judge.judgeId()] = judge.scores[key] ? judge.scores[key].total().toFixed(1) : '';
            });

            return { key: key, display: category.display, scores: scores };
        });

        var grandTotal = {};
        _.each(judges(), function (judge) {
            grandTotal[judge.judgeId()] = judge.grandTotalServer().toFixed(1);
        });

        return { performance: performance, grandTotal: grandTotal, categories: categories, panelJudges: panel.panelJudges };
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
    $.extend(self, data);

    self.performanceTime = new Date(data.performanceTime);

    self.didCompete = ko.computed(function () {
        return !self.didNotCompete;
    }, self);

    self.scoringIsNotComplete = ko.computed(function () {
        return !self.scoringComplete;
    }, self);
};