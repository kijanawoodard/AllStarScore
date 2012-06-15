$(document).ready(function () {
    var viewModel = ko.mapping.fromJS({ viewModel: window.scoringScoreEntryData }, mapping);
    ko.applyBindings(viewModel, document.getElementById('scoring_scoreentry'));
});

var mapping = {
    'viewModel': {
        create: function (options) {
            return new ScoreEntryViewModel(options.data);
        }
    },
    'performance': {
        create: function (options) {
            return new PerformanceModel(options.data);
        }
    }
};

var ScoreEntryViewModel = function (data) {
    var self = this;
    ko.mapping.fromJS(data, mapping, this);

    self.getTemplate = function () {
        var division = self.performance.divisionId();
        var level = self.performance.levelId();
        var map = self.scoringMap.templates[division] || self.scoringMap.templates[level];
        return map();
    };

    self.getScoring = function (performance, score) {
        var division = performance.divisionId();
        var level = performance.levelId();
        var map = self.scoringMap.categories[division] || self.scoringMap.categories[level];
        return { scores: score.scores, map: map.all() };
    };

    //set default scores if they don't exist
    (function () {
        var input = self.getScoring(self.performance, self.score);

        _.each(input.map, function (category) {

            input.scores[category.name()] = input.scores[category.name()] || ko.observable(0.0);

            if (category.includeExectionScore()) {
                input.scores[category.name() + '_execution'] = input.scores[category.name() + '_execution'] || ko.observable(0.0);
            }

            input.scores[category.name() + '_total'] = ko.computed(function () {
                var base = input.scores[category.name()]();
                var execution = category.includeExectionScore() ? input.scores[category.name() + '_execution']() : 0;
                return parseFloat(base) + parseFloat(execution);
            });
        });

        input.scores['total_base'] = ko.computed(function () {
            var memo = 0.0;
            for (var key in input.scores) {
                if (key.indexOf('_') == -1) {
                    memo += parseFloat(input.scores[key]());
                }
            }
            return memo;
        });

        input.scores['total_execution'] = ko.computed(function () {
            var memo = 0.0;
            for (var key in input.scores) {
                if (key.indexOf('_execution') > -1 && key != 'total_execution') {
                    memo += parseFloat(input.scores[key]());
                }
            }
            return memo;
        });

        input.scores['grand_total'] = ko.computed(function () {
            return input.scores['total_base']() + input.scores['total_execution']();
        });

        input.scores['min_total'] = ko.computed(function () {
            var result = _.reduce(input.map, function (memo, category) {
                return memo + category.min();
            }, 0);
            return result;
        });
        
        input.scores['max_total'] = ko.computed(function () {
            var result = _.reduce(input.map, function (memo, category) {
                return memo + category.max();
            }, 0);
            return result;
        });

    } ()); //define it and run it; a startup script
};

var PerformanceModel = function (data) {
    data.performanceTime = new Date(data.performanceTime);
    ko.mapping.fromJS(data, mapping, this);
};