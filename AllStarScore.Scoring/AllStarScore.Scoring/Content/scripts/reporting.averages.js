﻿$(document).ready(function () {

    var mapping = {
        'reporting': {
            create: function (options) {
                return new reportingViewModel(options.data);
            }
        }
    };

    var reportingViewModel = function (data) {
        var self = this;
        var vm = window.viewModel;
        var lookup = vm.lookup;

        ko.mapping.fromJS(data, mapping, self);

        self.asArray = function () {
            return vm.utilities.asArray(self.averages);
        };

        self.getScoring = function (item) {
            var info = lookup.info;
            var maps = lookup.scoringMap.categories;
            
            var division = item.key;
            var level = info.divisions[division].levelId();
            var map = maps[division] || maps[level];
            var scores = vm.utilities.asArray(item.value);

            map = _.extend(map, maps['judges-deductions'], maps['judges-legalities']);

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

    window.viewModel.averagesViewModel = ko.mapping.fromJS(window.averagesData, mapping);
    //    var viewModel = ko.mapping.fromJS({ viewModel: window.averagesData }, mapping);
    //    ko.applyBindings(viewModel, document.getElementById('reporting_averages'));
});