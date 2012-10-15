$(document).ready(function () {

    var reportingViewModel = function (data) {
        var self = this;

        self.averages = data.reporting.averages;

        self.getScoring = function (item) {
            var map = AllStarScore.ScoringMap.getMaps(item.key);
            var categories = _.extend(map.categories, AllStarScore.ScoringMap.categories['judges-deductions'], AllStarScore.ScoringMap.categories['judges-legalities']);
            var scores = AllStarScore.Utilities.asArray(item.value);
            
            _.each(scores, function (score) {
                score.category = categories[score.key].display;
            });

            return {
                level: map.level.name,
                division: map.division.name,
                scores: scores
            };
        };
    };

    AllStarScore.AveragesViewModel = new reportingViewModel(window.averagesData);
});