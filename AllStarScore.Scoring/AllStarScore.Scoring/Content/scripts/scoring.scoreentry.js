﻿$(document).ready(function () {
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

        //an array version for knockout foreach
        var categories = $.map(map, function (category, key) {
            return { key: key, category: category };
        });
        
        return { score: score, map: map, categories: categories };
    };

    //set default scores if they don't exist
    (function () {
        var input = self.getScoring(self.performance, self.score);
        var scores = input.score.scores;

        $.each(input.map, function (key, category) {
            scores[key] = scores[key] || {};
            scores[key].base = scores[key].base || ko.observable(0.0);
            
            if (category.includeExectionScore()) {
                scores[key].execution = scores[key].execution || ko.observable(0.0);
            }

            scores[key].total = ko.computed(function () {
                var base = scores[key].base();
                var execution = category.includeExectionScore() ? scores[key].execution() : 0;
                return parseFloat(base) + parseFloat(execution);
            });
        });

        input.score.totalBase = ko.computed(function () {
            var memo = 0.0;
            for (var key in scores) {
                memo += parseFloat(scores[key].base());
            }
            return memo;
        });

        input.score.totalExecution = ko.computed(function () {
            var memo = 0.0;
            for (var key in scores) {
                var execution = scores[key].execution ? scores[key].execution() : 0.0;
                memo += parseFloat(execution);
            }
            return memo;
        });

        input.score.grandTotal = ko.computed(function () {
            return input.score.totalBase() + input.score.totalExecution();
        });

        input.score.minTotal = ko.computed(function () {
            var result = _.reduce(input.map, function (memo, category) {
                return memo + category.min();
            }, 0);
            return result;
        });

        input.score.maxTotal = ko.computed(function () {
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