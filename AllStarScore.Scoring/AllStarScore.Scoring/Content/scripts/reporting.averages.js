$(document).ready(function () {
    var viewModel = ko.mapping.fromJS({ viewModel: window.averagesData }, mapping);
    ko.applyBindings(viewModel, document.getElementById('reporting_averages'));
});


var mapping = {
    'viewModel': {
        create: function (options) {
            return new ReportingViewModel(options.data);
        }
    }
};

var ReportingViewModel = function (data) {
    var self = this;

    ko.mapping.fromJS(data, mapping, self);

    self.asArray = function (obj) {
        var result = [];
        for (var key in obj) {
            result.push({ key: key, value: obj[key] });
        }

        return result;
    };

    self.getScoring = function (item) {
        var vm = window.infoViewModel;
        var info = vm.info;

        var division = item.key;
        var level = info.divisions[division].levelId();
        var map = vm.scoringMap.categories[division] || vm.scoringMap.categories[level];
        var scores = self.asArray(item.value);

        map = _.extend(map, vm.scoringMap.categories['judges-deductions'], vm.scoringMap.categories['judges-legalities']);

        _.each(scores, function (score) {
            score.category = map[score.key].display();
        });
        
        return {
            level: info.levels[level].name,
            division: info.divisions[division].name,
            map: map,
            scores: scores
        };
    };
};