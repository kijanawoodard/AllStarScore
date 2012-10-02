var scorepad_cookie_name = 'scorepad.high.';

$(document).ready(function() {
    AllStarScore.Entry = ko.mapping.fromJS({ viewModel: window.scoringScoreEntryData }, mapping);
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

    self.performance = AllStarScore.CompetitionData.performances[self.performanceId()];

    var getJudgeKey = function (id) {
        //not sure about this; but move ahead for now; refactor later
        if (id == 'D')
            return 'judges-deductions';
        else if (id == 'L')
            return 'judges-legalities';
        else
            return 'judges-panel';
    };

    self.getScorePanelCookieName = function () {
        var judge = getJudgeKey(self.score.judgeId());
        var division = getJudgeKey(self.performance.divisionId);
        var level = self.performance.levelId;
        var result = AllStarScore.ScoringMap.templates[judge] ? judge :
                     AllStarScore.ScoringMap.templates[division] ? division :
                     level;
        return result;
    };

    self.getTemplate = function () {
        var judge = getJudgeKey(self.score.judgeId());
        var division = self.performance.divisionIdWithoutCompanyId;
        var level = self.performance.levelIdWithoutCompanyId;
        var map = AllStarScore.ScoringMap.templates[judge] || AllStarScore.ScoringMap.templates[division] || AllStarScore.ScoringMap.templates[level];
        return map;
    };

    //take parms to prepare for multiple renderings
    self.getScoring = function (performance, score) {
        var judge = getJudgeKey(score.judgeId());
        var division = performance.divisionIdWithoutCompanyId;
        var level = performance.levelIdWithoutCompanyId;
        var map = AllStarScore.ScoringMap.categories[judge] || AllStarScore.ScoringMap.categories[division] || AllStarScore.ScoringMap.categories[level];
        //        console.log(judge);
        //        console.log(division);
        //        console.log(level);
        //        console.log(map);
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
                //console.log(ko.toJSON(result));
                //                console.log('saved');
                //                $('.validation-summary-errors').empty();
                console.log(result);
                window.location = result;
                console.log('hi');
            }
        });
    };

    self.onAfterRender = function () {
        setupScorePad();
    };

    /* occurs on object creation */
    //set default scores if they don't exist
    (function () {
        var input = self.getScoring(self.performance, self.score);
        var scores = input.score.scores;

        $.each(input.map, function (key, category) {
            scores[key] = scores[key] || {};
            scores[key].base = scores[key].base || ko.observable();
            scores[key].execution = scores[key].execution || ko.observable();

            scores[key].total = ko.computed(function () {
                var base = scores[key].base();
                var execution = category.includeExectionScore ? scores[key].execution() : 0;
                var result = (parseFloat(base) + parseFloat(execution)) || 0;
                return formatNumber(result);
            });

            var executionMax = 1;

            scores[key].isBaseBelowMin = ko.computed(function () {
                var base = scores[key].base();
                var executionFactor = category.includeExectionScore ? executionMax : 0;
                return parseFloat(base) != 0 && (parseFloat(base) + executionFactor) < category.min;
            });

            scores[key].isBaseAboveMax = ko.computed(function () {
                var base = scores[key].base();
                var executionFactor = category.includeExectionScore ? executionMax : 0;
                return (parseFloat(base) + executionFactor) > category.max;
            });

            scores[key].isExecutionBelowMin = ko.computed(function () {
                var execution = category.includeExectionScore ? scores[key].execution() : 0;
                return parseFloat(execution) < 0;
            });

            scores[key].isExecutionAboveMax = ko.computed(function () {
                var execution = category.includeExectionScore ? scores[key].execution() : 0;
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

        input.score.allBaseScoresInputted = ko.computed(function () {
            return _.all(scores, function (score) {
                return score.base();
            });
        });

        input.score.grandTotal = ko.computed(function () {
            var result = parseFloat(input.score.totalBase()) + parseFloat(input.score.totalExecution());
            return formatNumber(result);
        });

        input.score.isGrandTotalBelowMin = ko.computed(function () {
            return input.score.allBaseScoresInputted() && input.score.grandTotal() < input.score.minTotal();
        });

        input.score.isGrandTotalAboveMax = ko.computed(function () {
            return input.score.allBaseScoresInputted() > 0 && input.score.grandTotal() > input.score.maxTotal();
        });

        input.score.minTotal = ko.computed(function () {
            var result = _.reduce(input.map, function (memo, category) {
                return memo + category.min;
            }, 0);
            return result;
        });

        input.score.maxTotal = ko.computed(function () {
            var result = _.reduce(input.map, function (memo, category) {
                return memo + category.max;
            }, 0);
            return result;
        });

        //we will save scorepad settings in a cookie; establish the cookie name
        scorepad_cookie_name += self.getScorePanelCookieName();

    } ()); //define it and run it; a startup script
};

var PerformanceModel = function (data) {
    var self = this;
    $.extend(self, data);

    self.performanceTime = new Date(self.performanceTime);
};

var formatNumber = function (num) {
    num *= 10;
    num = Math.round(num) / 10;
    num = num.toFixed(1);
    return num;
};

//score pad and textbox entry stuff ported from old code. convert to knockout? maybe, if we add a 2nd visible score pad;
var setupScorePad = function () {
    var textboxes = $("input[type=text]:visible");
    var scorepad = $(".scorepad");
    var active;

    var highPadSelected = $(".scorepad table.high td").eq($.cookie(scorepad_cookie_name)) || $(".scorepad table.high td:first");
    var selectedClass = "selected";
    var lowOnly = false;

    textboxes.focus(function () {
        scorepad.show();

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

    textboxes.change(function () {
        var val = $(this).val();
        var f = formatNumber(parseFloat(val));

        if (isNaN(f)) {
            f = 0;
        }
        else if (f == val) {
            return true;
        }

        $(this).val(f);
        $(this).change();
        return false;
    });

    $(textboxes).keydown(function (evt) {
        var event = evt || window.event;
        var key = event.keyCode || event.which;

        //move next on enter
        if (key == 13) {
            moveNext(this);
            event.preventDefault();
            return false;
        }
        return (event.ctrlKey || event.altKey
            || (47 < key && key < 58 && event.shiftKey == false)
            || (95 < key && key < 106)
            || (key == 110)
            || (key == 190)
            || (key == 8)
            || (key == 9)
            || (key > 34 && key < 40)
            || (key == 46));
    });

    function moveNext(box) {
        var nextBoxNumber = textboxes.index(box) + 1;
        if (nextBoxNumber < textboxes.length) {
            var nextBox = textboxes[nextBoxNumber];
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
        $.cookie(scorepad_cookie_name, $(this).text(), { expires: 365, path: '/' });
    });

    $(".scorepad table.low td").click(function () {
        
        if (!active) {
            return;
        }
        
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
};