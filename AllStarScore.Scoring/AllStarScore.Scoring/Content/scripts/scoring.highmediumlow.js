$(document).ready(function () {
    //console.log(AllStarScore);
    AllStarScore.HighMediumLow = new AllStarScore.HighMediumLow();
});

AllStarScore.HighMediumLow = function () {
    var self = this;

    self.judgeScores = ko.observableArray();
    self.panelJudges = AllStarScore.CompetitionData.panelJudges;

    var performance = AllStarScore.ScoreEntry.performance;
    var maps = AllStarScore.ScoringMap.getMaps(performance);

    self.panel = performance.panel;

    self.categories = ko.computed(function () {
        var result = $.map(maps.categories, function (category, key) {
            var scores = {};

            _.each(self.judgeScores(), function (judge) {
                var ok = _.contains(self.panelJudges, judge.judgeId);
                if (ok) {
                    scores[judge.judgeId] = judge.scores[key] ? judge.scores[key] : { base: 0.0, execution: 0.0, total: 0.0 };
                    var score = scores[judge.judgeId];
                    var base = score.base % 1; //http://stackoverflow.com/a/4512317/214073
                    base = base.toFixed(1);
                    
                    score.isLow = base > 0 && base < 0.4;
                    score.isMedium = base >= 0.4 && base < 0.7;
                    score.isHigh = base >= 0.7;
                    score.display = ko.observable(score.isLow ? "Low " : score.isMedium ? "Medium " : score.isHigh ? "High " : "");
                }
            });

            scores = AllStarScore.Utilities.asArray(scores);
            scores = _.pluck(scores, 'value');

            return { key: key, display: category.display, scores: scores };
        });

        return result;
    }, self);

    $('#scoring_highmediumlow').on('click', 'button', function () {
        $.getJSON(AllStarScore.HighMediumLowLink, function (data) {
            //ko.mapping.fromJS(data.panel.calculator.scores, {}, self.judgeScores); //the observable elements introduced too much overhead in the computed method here
            self.judgeScores.removeAll();
            _.each(data.panel.calculator.scores, function (score) {
                self.judgeScores.push(score);
            });
        });
    });
};