$(document).ready(function () {

    var mapping = {
        'reporting': {
            create: function (options) {
                return new reportingViewModel(options.data);
            }
        }
    };

    var reportingViewModel = function (data) {
        var self = this;

        self.averages = data.averages;
        //        ko.mapping.fromJS(data, mapping, self);

        self.asArray = function () {
            return AllStarScore.Utilities.asArray(self.averages);
        };

        self.getScoring = function (item) {
            var info = AllStarScore.CompetitionData;
            var maps = AllStarScore.ScoringMap.categories;

            console.log(maps);

            var division = info.divisions[item.key];
            var level = info.levels[division.levelId];
            var map = maps[division.withoutCompanyId] || maps[level.withoutCompanyId];
            var scores = AllStarScore.Utilities.asArray(item.value);

            map = _.extend(map, maps['judges-deductions'], maps['judges-legalities']);

            _.each(scores, function (score) {
                score.category = map[score.key].display;
            });
            console.log(scores);
            return {
                level: level.name,
                division: division.name,
                map: map,
                scores: scores
            };
        };
    };

    console.log(AllStarScore);
    AllStarScore.AveragesViewModel = ko.mapping.fromJS(window.averagesData, mapping);
    //    var viewModel = ko.mapping.fromJS({ viewModel: window.averagesData }, mapping);
    //    ko.applyBindings(viewModel, document.getElementById('reporting_averages'));
});