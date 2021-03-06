﻿AllStarScore.Scheduling = {
    Start: function() {
        $('#scheduling_edit .selectable').selectable({ filter: "li" });

        var viewModel = new AllStarScore.Scheduling.EditViewModel();
        ko.applyBindings(viewModel, document.getElementById('scheduling_edit'));

        $('#scheduling_edit .sortable').disableSelection(); //http://stackoverflow.com/a/9993099/214073 sortable and selectable
    }
};

AllStarScore.Scheduling.DayModel = function (data) {
    ko.mapping.fromJS(data, AllStarScore.Scheduling.DayMapping, this);
};

AllStarScore.Scheduling.EntryModel = function (data) {
    ko.mapping.fromJS(data, {}, this);

    var self = this;

    self.isPerformance = ko.computed(function () {
        return self.peformanceId ? true : false;
    }, self);

    self.isNonTeamEntry = ko.computed(function () {
        return !self.isPerformance;
    }, self);

    self.display = ko.computed(function () {
        return (self.text || self.type)();
    }, self);

};

AllStarScore.Scheduling.ScheduleMapping = {
    'include': ['duration'],
    'days': {
        create: function (options) {
            return new AllStarScore.Scheduling.DayModel(options.data);
        }
    }
};

AllStarScore.Scheduling.DayMapping = {
    'entries': {
        create: function (options) {
            return new AllStarScore.Scheduling.EntryModel(options.data);
        }
    }
};

AllStarScore.Scheduling.EditViewModel = function () {
    var self = this;
    var hook = $('#scheduling_edit');
    var form = hook.find('form');

    var utilities = window.AllStarScore.Utilities;
    var data = AllStarScore.CompetitionData;
    self.performances = data.performances;

    _.each(data.raw.divisions, function (division) {
        AllStarScore.Scheduling.ScheduleMapping.include.push(division.id); //skirt this issue: https://groups.google.com/forum/?fromgroups=#!topic/knockoutjs/QoubswdzIxI; this works because we know all the possible keys
    });

    self.divisionSheet = utilities.asArray(_.groupBy(self.performances, 'divisionId'));
    self.divisionSheet = _.sortBy(self.divisionSheet, function (item) { return item.value[0].level + ' ' + item.value[0].division; });
    _.each(self.divisionSheet, function (division) {
        division.value = _.sortBy(division.value, function (performance) {
            return performance.gym + ' ' + performance.team + ' ' + performance.location;
        });

        division.value = _.uniq(division.value, false, function (performance) { return performance.registrationId; });
    });

    self.blockSchedule = utilities.asArray(_.groupBy(self.performances, 'level'));
    self.blockSchedule = _.sortBy(self.blockSchedule, 'key');

    self.schedule = ko.mapping.fromJS(data.schedule, AllStarScore.Scheduling.ScheduleMapping);

    _.each(self.schedule.days(), function (day) {
        day.starthour = ko.observable(day.day().getHours());
        day.startminutes = ko.observable(day.day().getMinutes());

        day.starthour.subscribe(function () {
            var time = day.day; //yes, i know the day.day thing is weird. bad name.
            time(time().set({ hour: day.starthour() }));
        });

        day.startminutes.subscribe(function () {
            var time = day.day;
            time(time().set({ minute: day.startminutes() }));
        });
    });

    self.panels = ko.computed(function () {
        return _.map(_.range(self.schedule.numberOfPanels()), function (i) {
            return String.fromCharCode(65 + i);
        });
    });

    self.getPanel = function (node) {
        if (!node.performanceId) return "";

        var performanceId = node.performanceId();
        var divisionId = self.performances[performanceId].divisionId;
        var result =
            self.schedule.divisionPanels[divisionId] ?
                self.schedule.divisionPanels[divisionId] :
                self.schedule.divisionPanels[divisionId] = ko.observable(self.panels()[0]);

        return result;
    };

    self.shiftPanel = function (node) {
        var dp = self.getPanel(node);

        var panels = ko.toJS(self.panels);
        var index = _.indexOf(panels, dp()) + 1;
        var result = panels[index] || panels[0]; //get the next one or the first one

        dp(result);
    };

    self.unscheduled = ko.observableArray();
    //console.log(data.competition);

    var autoSchedulingStrategy;

    var oneDayAutoSchedulingStrategy = function (days, performance, model) {
        days[0].entries.push(model);
    };

    var twoDayOnePerformanceAutoSchedulingStrategy = function (days, performance, model) {
        var dayTwoLevels = ["level/school/", "level/2/", "level/4/", "level/5/", "level/6/"];
        var putOnDayTwo = _.any(dayTwoLevels, function (level) {
            return performance.divisionId.indexOf(level) > -1;
        });

        if (days.length == 1) {
            days[0].entries.push(model);
        }
        else if (!putOnDayTwo) {
            days[0].entries.push(model);
        }
        else {
            days[1].entries.push(model);
        }
    };

    var twoDayTwoPerformanceAutoSchedulingStrategy = function (days, performance, model) {
        if (days.length == 1 || performance.orderId == 1) { //making sure days is the length we expect
            days[0].entries.push(model);
        }
        else if (performance.orderId == 2) {
            days[1].entries.push(model);
        }
        else {
            self.unscheduled.push(model);
        }
    };

    if (self.schedule.days().length == 1) {
        autoSchedulingStrategy = oneDayAutoSchedulingStrategy;
    }
    else if (data.competition.numberOfPerformances == 1) {
        autoSchedulingStrategy = twoDayOnePerformanceAutoSchedulingStrategy;
    }
    else if (data.competition.numberOfPerformances == 2) {
        autoSchedulingStrategy = twoDayTwoPerformanceAutoSchedulingStrategy;
    } else { //safety
        autoSchedulingStrategy = oneDayAutoSchedulingStrategy;
    }

    var addPerformance = function (performance) {
        var model = {
            'performanceId': performance.id,
            'type': 'Performance',
            'time': new Date()
        };

        model = new AllStarScore.Scheduling.EntryModel(model);
        //console.log(performance);
        var days = self.schedule.days();

        autoSchedulingStrategy(days, performance, model);
    };

    (function () {
        var scheduled =
            _.chain(self.schedule.days())
                .map(function (day) {
                    return day.entries();
                })
                .flatten()
                .filter(function (item) {
                    return item.performanceId;
                })
                .map(function (item) {
                    return item.performanceId();
                })
                .value();

        var unscheduled = _.difference(_.keys(self.performances), scheduled);
        unscheduled = _.sortBy(unscheduled, function (key) {
            //console.log(self.performances[key]);
            var performance = self.performances[key];
            return [performance.gym, performance.team, performance.order, performance.level, performance.division]; //order will only work as long as there are less 10 performances - never heard of more than 2
        });

        _.each(unscheduled, function (id) {
            var performance = self.performances[id];
            addPerformance(performance);
        });
    } ()); //self executing

    self.toPerformance = function (entry) {
        if (!entry.performanceId) return entry;
        return self.performances[entry.performanceId()];
    };

    var prototype = function () {
        return new AllStarScore.Scheduling.EntryModel({
            'type': '',
            'time': new Date()
        });
    };

    self.addBreak = function (day) {
        var item = prototype();
        item.type('Break');
        day.entries.push(item);
    };

    self.addAwards = function (day) {
        var item = prototype();
        item.type('Awards');
        day.entries.push(item);
    };

    self.addOpen = function (day) {
        var item = prototype();
        item.type('Open');
        day.entries.push(item);
    };

    self.entryTypes = {
        'Performance': { duration: self.schedule.defaultDuration, template: 'performance-template' },
        'Open': { duration: self.schedule.defaultDuration, template: 'block-template' },
        'Break': { duration: self.schedule.defaultBreakDuration, template: 'block-template' },
        'Awards': { duration: self.schedule.defaultAwardsDuration, template: 'block-template' }
    };


    //recalculate time when we move items around
    $.each(self.schedule.days(), function (index, day) {
        day.change = ko.computed(function () {
            //invoke all the things that should trigger the update
            day.day();
            day.entries();
            self.schedule.defaultDuration();
            self.schedule.defaultWarmupTime();
            self.schedule.defaultBreakDuration();
            self.schedule.defaultAwardsDuration();
        }).extend({ throttle: 1 });

        day.change.subscribe(function () {
            var entries = day.entries();
            for (var i = 0, j = entries.length; i < j; i++) {
                var entry = entries[i];
                if (i == 0) {
                    entry.time(day.day());
                }
                else {
                    var prev = entries[i - 1];
                    var duration = prev.duration || self.entryTypes[prev.type()].duration;
                    entry.time(new Date(prev.time().getTime() + duration() * 60 * 1000));
                }
            }
        }, day.entries);

        day.entries.valueHasMutated(); //force recalc now; may have new unscheduled items
    });

    self.calculateWarmup = function (entry) {
        var warmup = entry.warmupTime || self.schedule.defaultWarmupTime;
        var result = new Date(entry.time().getTime() - warmup() * 60 * 1000);
        return result;
    };

    self.getTemplate = function (entry) {
        return self.entryTypes[entry.type()].template;
    };

    self.displayOptions = ko.observable(false);
    self.toggleOptions = function () {
        self.displayOptions(!self.displayOptions());
    };

    self.active = ko.observable();
    self.chosen = ko.observable();
    self.activeOptions = function (entry) {
        var isPerformance = entry.type() == "Performance";

        var result = {
            type: entry.type(),
            duration: entry.duration || self.entryTypes[entry.type()].duration,
            max: isPerformance ? 30 : 90,
            canRemove: !isPerformance
        };
        result.duration = ko.observable(result.duration());
        return result;
    };

    self.edit = function (entry) {
        var edit = self.activeOptions(entry);
        self.active(edit);
        self.chosen(entry);
    };

    self.saveActive = function () {
        var value = self.active().duration();
        var duration = self.chosen().duration;
        if (duration) {
            duration(value);
        }
        else {
            self.chosen().duration = ko.observable(value);
        }

        self.schedule.defaultDuration.valueHasMutated(); //hack to force schedule to update
        //        self.closeActive();
    };

    self.closeActive = function () {
        self.active(null);
        self.chosen(null);
    };

    self.removeActive = function () {
        removeEntry(self.chosen());
        self.active(null);
        self.chosen(null);
    };

    var removeEntry = function (target) {
        target.removeMe = true;
        _.each(self.schedule.days(), function (day) {
            _.each(day.entries(), function (entry) {
                if (entry.removeMe) {
                    day.entries.remove(entry);
                }
            });
        });
    };

    self.justSaved = ko.observable(false);

    self.save = function () {
        var json = ko.mapping.toJSON(self.schedule);

        form.ajaxPost({
            data: json,
            success: function (result) {
                self.justSaved(true);
                setTimeout(function () {
                    self.justSaved(false);
                }, 2000);
            }
        });
    };

    return self;
};