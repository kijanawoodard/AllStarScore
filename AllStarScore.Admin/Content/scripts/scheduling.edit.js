﻿var viewModel;
var competitionDaysAreTheSame = true; //look out for this edge case; //TODO: need to recreate days; server?

$(document).ready(function () {

    $('#scheduling_edit .selectable').selectable({ filter: "li" });

    viewModel = new EditScheduleViewModel(window.editScheduleData);
    ko.applyBindings(viewModel, document.getElementById('scheduling_edit'));

    $('#scheduling_edit .sortable').disableSelection(); //http://stackoverflow.com/a/9993099/214073 sortable and selectable
});

var utilities = {
    asArray: function (obj) {
        var result = [];
        for (var key in obj) {
            result.push({ key: key, value: obj[key] });
        }

        return result;
    },
    asObject: function (array, keyFunc) {
        keyFunc = keyFunc || function (item) {
            return item.id;
        };

        var result = {};
        _.each(array, function (item, index) {
            result[keyFunc(item)] = item;
        });

        return result;
    }
};

var DayModel = function (data) {
    data.day = new Date(data.day);
    ko.mapping.fromJS(data, mapping, this);
    
    if (!competitionDaysAreTheSame) {
        this.entries.removeAll();
    }
};

var EntryModel = function (data, viewmodel) {
    data.time = new Date(data.time);
    ko.mapping.fromJS(data, {}, this);

    var self = this;
    
    self.isPerformance = ko.computed(function () {
        return self.peformanceId ? true : false;
    }, self);

    self.isNonTeamEntry = ko.computed(function () {
        return !self.isPerformance;
    }, self);

    self.originalPanel = data.panel;

    self.panel = ko.computed(function () {
        return self.isPerformance ? viewmodel.getPanel(self.performanceId())() : '';
    }, self);

    self.warmup = ko.computed(function () {
        return new Date(self.time().getTime() - self.warmupTime() * 60 * 1000);
    }, self);
};

var mapping = {
    'include': ["panel"],
    'days': {
        create: function (options) {
            return new DayModel(options.data);
        }
    },
    'entries': {
        create: function (options) {
            return new EntryModel(options.data);
        }
    }
};

var EditScheduleViewModel = (function (data) {
    var self = this;
    var hook = $('#scheduling_edit');
    var form = hook.find('form');

    var areSameDay = function (a, b) {
        return a.clone().clearTime().equals(b.clone().clearTime()); //have to clone otherwise original is modified
    };

    competitionDaysAreTheSame = data.schedule.days.length == data.competitionDays.length;

    //check to see that competition days haven't changed on us
    $.each(data.schedule.days, function (index, day) {
        if (competitionDaysAreTheSame) { //once this is false, leave it false
            var comp = new Date(data.competitionDays[index]);

            competitionDaysAreTheSame = areSameDay(new Date(day.day), comp);
        }
    });

    self.raw = data.schedule;
    self.competition = data.competition;
    self.levels = utilities.asObject(data.levels);
    self.divisions = utilities.asObject(data.divisions);
    self.gyms = utilities.asObject(data.gyms);
    self.registrations = utilities.asObject(data.registrations);
    self.performances = utilities.asObject(data.performances);

    _.each(self.performances, function (performance) {
        var division = self.divisions[performance.divisionId];
        performance.division = division.name;
        performance.level = self.levels[division.levelId].name;

        var registration = self.registrations[performance.registrationId];
        performance.team = registration.teamName;
        performance.particpants = registration.participantCount;
        performance.isShowTeam = registration.isShowTeam;

        var gym = self.gyms[registration.gymId];
        performance.gym = gym.name;
        performance.isSmallGym = gym.isSmallGym;
        performance.location = gym.location;

        performance.order = [, '1st', '2nd', '3rd', '4th', '5th'][performance.id.substr(performance.id.length - 1)];
    });

    self.schedule = ko.mapping.fromJS(data.schedule, mapping);
    self.competitionDays = data.competitionDays;

    self.panels = ko.computed(function () {
        return _.map(_.range(self.schedule.numberOfPanels()), function (i) {
            return String.fromCharCode(65 + i);
        });
    });

    self.divisionPanels = {};

    //initialize division panels
    _.each(data.schedule.days, function (day) {
        _.each(day.entries, function (entry) {
            if (entry.performanceId) {
                var divisionId = self.performances[entry.performanceId].divisionId;
                self.divisionPanels[divisionId] = self.divisionPanels[divisionId] || ko.observable(entry.panel);
            }
        });
    });

    self.getPanel = function (performanceId) {
        var divisionId = self.performances[performanceId].divisionId;
        var result =
            self.divisionPanels[divisionId] ?
                self.divisionPanels[divisionId] :
                self.divisionPanels[divisionId] = ko.observable(self.panels()[0]);

        return result;
    };

    self.shiftPanel = function (node) {
        var dp = self.getPanel(node.performanceId());

        var panels = ko.toJS(self.panels);
        var index = _.indexOf(panels, dp()) + 1;
        var result = panels[index] || panels[0]; //get the next one or the first one

        dp(result);
    };

    self.unscheduled = ko.observableArray();

    var addPerformance = function (performance) {
        var json = {};
        json.performanceId = performance.id;
        json.time = new Date(); // day.day ? day.day().clone().clearTime() : new Date();
        json.duration = self.schedule.defaultDuration();
        json.warmupTime = self.schedule.defaultWarmupTime(),
        json.index = -1,
        json.template = 'registration-template';

        json = new EntryModel(json, self);

        self.unscheduled.push(json);
    };

    var load = function () {
        var scheduled =
            _.chain(self.schedule.days())
                .map(function (day) {
                    return day.entries();
                })
                .flatten()
                .filter(function (item) {
                    return item.performanceId;
                })
                .pluck("performanceId")
                .value();

        var unscheduled =
            _.chain(_.keys(self.performances))
                .without(scheduled)
                .value();

        _.each(unscheduled, function (id) {
            var performance = self.performances[id];
            addPerformance(performance);
        });
    } (); //self executing

    self.toPerformance = function (entry) {
        //console.log(entry);
        return self.performances[entry.performanceId()];
    };

    var prototype = function () {
        return new EntryModel({
            type: '',
            time: '',
            text: '',
            panel: '',
            index: -1,
            duration: 20,
            warmupTime: self.schedule.defaultWarmupTime(),
            template: 'block-template'
        }, self);
    };

    self.addBreak = function (day) {
        var item = prototype();
        item.type('Break');
        item.text(item.type());
        day.entries.push(item);
    };

    self.addAwards = function (day) {
        var item = prototype();
        item.type('Awards');
        item.text(item.type());
        day.entries.push(item);
    };

    self.addOpen = function (day) {
        var item = prototype();
        item.type('Open');
        item.text(item.type());
        item.duration(self.schedule.defaultDuration());
        day.entries.push(item);
    };

    //recalculate time when we move items around
    $.each(self.schedule.days(), function (index, unit) {
        unit.entries.subscribe(function () {
            var entries = unit.entries();
            for (var i = 0, j = entries.length; i < j; i++) {
                var entry = entries[i];
                if (i == 0) {
                    entry.time(unit.day());
                }
                else {
                    var prev = entries[i - 1];
                    entry.time(new Date(prev.time().getTime() + prev.duration() * 60 * 1000));
                }
            }
        }, unit.entries);
    });

    //    self.schedule.days.valueHasMutated(); //we loaded the items before subscribe, so force subscribe function now

    self.displayOptions = ko.observable(self.performances.length == 0);
    self.toggleOptions = function () {
        self.displayOptions(!self.displayOptions());
    };

    self.justSaved = ko.observable(false);

    self.save = function () {
        form.ajaxPost({
            data: ko.mapping.toJSON(self.schedule),
            success: function (result) {
                self.justSaved(true);
                setTimeout(function () {
                    self.justSaved(false);
                }, 2000);
            }
        });
    };

    return self;
});