$(document).ready(function () {

    var reportingViewModel = function (data) {
        var self = this;

        self.averages = data.reporting.averages;

        self.getScoring = function (item) {
            var info = AllStarScore.CompetitionData;
            var maps = AllStarScore.ScoringMap.categories;

            var division = info.divisions[item.key];
            var level = info.levels[division.levelId];
            var map = maps[division.withoutCompanyId] || maps[level.withoutCompanyId];
            var scores = AllStarScore.Utilities.asArray(item.value);

            map = _.extend(map, maps['judges-deductions'], maps['judges-legalities']);

            _.each(scores, function (score) {
                score.category = map[score.key].display;
            });

            return {
                level: level.name,
                division: division.name,
                map: map,
                scores: scores
            };
        };
    };

    AllStarScore.AveragesViewModel = new reportingViewModel(window.averagesData);
});