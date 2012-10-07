$(document).ready(function () {
    //console.log(AllStarScore);
    AllStarScore.HighMediumLow = new AllStarScore.HighMediumLow();
});

AllStarScore.HighMediumLow = function () {
    var self = this;

    self.panelJudges = AllStarScore.CompetitionData.panelJudges;
    var performance = AllStarScore.ScoreEntry.performance;
    self.judgeScores = ko.observableArray();
    var division = performance.divisionIdWithoutCompanyId;
    var level = performance.levelIdWithoutCompanyId;
    var map = AllStarScore.ScoringMap.categories[division] || AllStarScore.ScoringMap.categories[level];

    self.panel = performance.panel;
    
    self.categories = ko.computed(function () {
        var result = $.map(map, function (category, key) {
            var scores = {};

            _.each(self.judgeScores(), function (judge) {
                var ok = _.contains(self.panelJudges, judge.judgeId);
                if (ok) {
                    scores[judge.judgeId] = judge.scores[key] ? judge.scores[key] : { base: 0.0, execution: 0.0, total: 0.0 };
                    var score = scores[judge.judgeId];
                    var base = score.base % 1; //http://stackoverflow.com/a/4512317/214073
                    score.isLow = base > 0 && base <= .3;
                    score.isMedium = base > .3 && base <= .6;
                    score.isHigh = base > .6 && base <= .9;
                    score.display = ko.observable(score.isLow ? "Low " : score.isMedium ? "Medium " : score.isHigh ? "High " : "");
                }
            });

            return { key: key, display: category.display, scores: AllStarScore.Utilities.asArray(scores) };
        });

        return result;
    }, self);

    $('#scoring_highmediumlow').on('click', 'button', function () {
        $.getJSON(AllStarScore.HighMediumLowLink, function (data) {
            //ko.mapping.fromJS(data.panel.calculator.scores, {}, self.judgeScores); //the observable elements introduced too much overhead in the computed method here
            self.judgeScores.removeAll();
            _.each(data.panel.calculator.scores, function(score) {
                self.judgeScores.push(score);
            });
        });
    });
};