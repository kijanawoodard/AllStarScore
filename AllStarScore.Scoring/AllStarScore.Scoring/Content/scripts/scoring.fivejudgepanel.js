$(document).ready(function () {
    //console.log(AllStarScore);
    AllStarScore.Scoring = new FiveJudgePanelViewModel(Input.Scoring); //assigned the data in the view
    AllStarScore.Scoring.scoreEntryUrl = Input.scoreEntryUrl;
});

var FiveJudgePanelViewModel = function (data) {
    var self = this;

    //sort the scores by judge - results in 1,2,3,D,L - Hackish, might need to keep the sorting in a document
    data.panel.calculator.scores = _.sortBy(data.panel.calculator.scores, function (judge) {
        return judge.judgeId;
    });

    $.extend(self, data);

    self.performance = AllStarScore.CompetitionData.performances[self.performanceId];
    self.performance.scoringComplete = ko.observable(data.score.isScoringComplete);
    self.performance.didCompete = ko.observable(!data.score.didNotCompete);

    //self.panel.calculator = ko.mapping.fromJS(self.panel.calculator);

    self.getTemplate = function () {
        var division = self.performance.divisionIdWithoutCompanyId;
        var level = self.performance.levelIdWithoutCompanyId;
        var map = AllStarScore.ScoringMap.templates[division] || AllStarScore.ScoringMap.templates[level];
        return map;
    };

    self.getScoring = function (performance, panel) {
        var judgeScores = panel.calculator.scores;
        var division = performance.divisionIdWithoutCompanyId;
        var level = performance.levelIdWithoutCompanyId;
        var map = AllStarScore.ScoringMap.categories[division] || AllStarScore.ScoringMap.categories[level];

        var categories = $.map(map, function (category, key) {
            var scores = {};

            _.each(judgeScores, function (judge) {

                scores[judge.judgeId] = judge.scores[key] ? judge.scores[key].total.toFixed(1) : 0;
            });

            return { key: key, display: category.display, scores: scores };
        });

        var grandTotal = {};
        _.each(judgeScores, function (judge) {
            
            grandTotal[judge.judgeId] = judge.grandTotalServer;
        });

        _.each(panel.judges, function (judge) {
            grandTotal[judge.id] = grandTotal[judge.id] || 0;
        });
        
        return { performance: performance, grandTotal: grandTotal, categories: categories, panelJudges: panel.panelJudges };
    };

    self.markTeamDidNotCompete = function () {
        var form = $('#scoring_fivejudgepanel form.did_not_compete');

        form.ajaxPost({
            data: ko.mapping.toJSON({ performanceId: self.performance.id, divisionId: self.performance.divisionId, registrationId: self.performance.registrationId }),
            success: function (result) {
                //console.log(ko.toJSON(result));
                console.log('saved');
                $('.validation-summary-errors').empty();

                self.performance.didCompete(false);
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

                self.performance.didCompete(true);
            }
        });
    };

    self.markTeamScoringCompete = function () {
        var form = $('#scoring_fivejudgepanel form.scoring_complete');

        form.ajaxPost({
            data: ko.mapping.toJSON({ performanceId: self.performance.id, divisionId: self.performance.divisionId, registrationId: self.performance.registrationId }),
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