$(document).ready(function () {
    var viewModel = ko.mapping.fromJS({ viewModel: window.scoringScoreEntryData }, mapping);
    ko.applyBindings(viewModel, document.getElementById('scoring_scoreentry'));

    var textboxes = $("input[type=text]:visible");
    var scorepad = $(".scorepad");
    var active;
    var highPadSelected = $(".scorepad table.high td:first");
    var selectedClass = "selected";
    var lowOnly = false;

    textboxes.focus(function () {
        if (active) {
            active.removeClass(selectedClass);
        }

        active = $(this);
        active.select();
        active.addClass(selectedClass);

        var index = active.parent().index();
        if (index == 1) {
            scorepad.addClass("scorepad_base");
            scorepad.removeClass("scorepad_execution");
            lowOnly = false;
        }
        else if (index == 2) {
            scorepad.addClass("scorepad_execution");
            scorepad.removeClass("scorepad_base");
            lowOnly = true;
        }

    });

    if ($.browser.mozilla) {
        $(textboxes).keypress(checkForEnter);
    } else {
        $(textboxes).keydown(checkForEnter);
    }

    function checkForEnter(event) {
        if (event.which != 13) return true;
        moveNext(this);
        event.preventDefault();
        return false;
    }

    function moveNext(box) {
        var nextBoxNumber = textboxes.index(box) + 1;
        if (nextBoxNumber < textboxes.length) {
            var nextBox = textboxes[nextBoxNumber]
            nextBox.focus();
            nextBox.select();
        }
        else {
            $("button").first().focus();
        }
    }

    $("button").first().focus(function () {
        if (active) {
            active.removeClass(selectedClass);
        }
        active = undefined;
    });

    $(".scorepad table.high td").click(function () {
        highPadSelected.removeClass(selectedClass);
        highPadSelected = $(this);
        highPadSelected.addClass(selectedClass);
    });

    $(".scorepad table.low td").click(function () {
        var low = $(this).text();
        var high = highPadSelected.text();
        var value = high + low;

        if (lowOnly)
            value = "0" + low; //cheap hack

        active.val(value);
        active.change();
        moveNext(active);
    });

    textboxes.first().focus();
    highPadSelected.click();
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

    self.save = function () {
        var form = $('#scoring_scoreentry form');

        form.ajaxPost({
            data: ko.mapping.toJSON(self.score),
            success: function (result) {
                console.log(result);
                console.log('saved');
            }
        });
    };

    /* occurs on object creation */
    //set default scores if they don't exist
    (function () {
        var input = self.getScoring(self.performance, self.score);
        var scores = input.score.scores;

        $.each(input.map, function (key, category) {
            scores[key] = scores[key] || {};
            scores[key].base = scores[key].base || ko.observable();

            if (category.includeExectionScore()) {
                scores[key].execution = scores[key].execution || ko.observable();
            }

            scores[key].total = ko.computed(function () {
                var base = scores[key].base();
                var execution = category.includeExectionScore() ? scores[key].execution() : 0;
                var result = (parseFloat(base) + parseFloat(execution)) || 0;
                return formatNumber(result);
            });

            var executionMax = 1;

            scores[key].isBaseBelowMin = ko.computed(function () {
                var base = scores[key].base();
                return parseFloat(base) != 0 && (parseFloat(base) + executionMax) < category.min();
            });

            scores[key].isBaseAboveMax = ko.computed(function () {
                var base = scores[key].base();
                return (parseFloat(base) + executionMax) > category.max();
            });

            scores[key].isExecutionBelowMin = ko.computed(function () {
                var execution = category.includeExectionScore() ? scores[key].execution() : 0;
                return parseFloat(execution) < 0;
            });

            scores[key].isExecutionAboveMax = ko.computed(function () {
                var execution = category.includeExectionScore() ? scores[key].execution() : 0;
                return parseFloat(execution) > executionMax;
            });
        });

        input.score.totalBase = ko.computed(function () {
            var memo = 0.0;
            for (var key in scores) {
                memo += parseFloat(scores[key].base() || 0);
            }
            return formatNumber(memo);
        });

        input.score.totalExecution = ko.computed(function () {
            var memo = 0.0;
            for (var key in scores) {
                var execution = scores[key].execution ? scores[key].execution() : 0.0;
                memo += parseFloat(execution) || 0;
            }
            return formatNumber(memo);
        });

        input.score.grandTotal = ko.computed(function () {
            var result = parseFloat(input.score.totalBase()) + parseFloat(input.score.totalExecution());
            return formatNumber(result);
        });

        input.score.isGrandTotalBelowMin = ko.computed(function () {
            return input.score.grandTotal() > 0 && input.score.grandTotal() < input.score.minTotal();
        });

        input.score.isGrandTotalAboveMax = ko.computed(function () {
            return input.score.grandTotal() > 0 && input.score.grandTotal() > input.score.maxTotal();
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

        var formatNumber = function (num) {
            num *= 10;
            num = Math.round(num) / 10;
            num = num.toFixed(1);
            return num;
        };

    } ()); //define it and run it; a startup script
};

var PerformanceModel = function (data) {
    data.performanceTime = new Date(data.performanceTime);
    ko.mapping.fromJS(data, mapping, this);
};