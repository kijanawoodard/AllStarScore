$(document).ready(function () {
    //console.log(AllStarScore);
    AllStarScore.HighMediumLow = new AllStarScore.HighMediumLow(window.highMediumLowData);
});

AllStarScore.HighMediumLow = function (data) {
    var self = this;

    self.panelJudges = _.pluck(data.panel.panelJudges, "id");
    var performance = AllStarScore.ScoreEntry.performance;
    var judgeScores = data.panel.calculator.scores;
    var division = performance.divisionIdWithoutCompanyId;
    var level = performance.levelIdWithoutCompanyId;
    var map = AllStarScore.ScoringMap.categories[division] || AllStarScore.ScoringMap.categories[level];

    self.panel = performance.panel;
    self.comments = $.map(judgeScores, function (score) {
        return { judgeId: score.judgeId, comment: score.comments };
    });

    self.categories = $.map(map, function (category, key) {
        var scores = {};

        _.each(judgeScores, function (judge) {
            var ok = _.contains(self.panelJudges, judge.judgeId);
            if (ok) {
                scores[judge.judgeId] = judge.scores[key] ? judge.scores[key] : { base: 0.0, execution: 0.0, total: 0.0 };
                var val = scores[judge.judgeId];
                var base = val.base % 1; //http://stackoverflow.com/a/4512317/214073
                scores[judge.judgeId].isLow = base > 0 && base <= .3;
                scores[judge.judgeId].isMedium = base > .3 && base <= .6;
                scores[judge.judgeId].isHigh = base > .6 && base <= .9;
                scores[judge.judgeId].display = val.isLow ? "Low " : val.isMedium ? "Medium " : val.isHigh ? "High " : "";
            }
        });

        return { key: key, display: category.display, scores: AllStarScore.Utilities.asArray(scores) };
    });
};