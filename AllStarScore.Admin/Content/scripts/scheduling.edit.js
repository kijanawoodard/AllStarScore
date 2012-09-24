var viewModel;

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
    ko.mapping.fromJS(data, dayMapping, this);
};

var EntryModel = function (data) {
    data.time = new Date(data.time);
    ko.mapping.fromJS(data, {}, this);

    var self = this;

    self.isPerformance = ko.computed(function () {
        return self.peformanceId ? true : false;
    }, self);

    self.isNonTeamEntry = ko.computed(function () {
        return !self.isPerformance;
    }, self);
};

var scheduleMapping = {
    'include' : [],
    'days': {
        create: function (options) {
            return new DayModel(options.data);
        }
    }
};

var dayMapping = {
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
        performance.participants = registration.participantCount;
        performance.isShowTeam = registration.isShowTeam;

        var gym = self.gyms[registration.gymId];
        performance.gym = gym.name;
        performance.isSmallGym = gym.isSmallGym;
        performance.location = gym.location;

        performance.order = [, '1st', '2nd', '3rd', '4th', '5th'][performance.id.substr(performance.id.length - 1)];
    });

    _.each(data.divisions, function (division) {
        scheduleMapping.include.push(division.id); //skirt this issue: https://groups.google.com/forum/?fromgroups=#!topic/knockoutjs/QoubswdzIxI; this works because we know all the possible keys
    });

    self.schedule = ko.mapping.fromJS(data.schedule, scheduleMapping);

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

    var addPerformance = function (performance) {
        var json = {};
        json.performanceId = performance.id;
        json.type = 'Performance';
        json.time = new Date();
        json.warmupTime = self.schedule.defaultWarmupTime(),
        json.index = -1,
        json.template = 'registration-template';

        json = new EntryModel(json);

        self.unscheduled.push(json);
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
        return new EntryModel({
            type: '',
            time: '',
            text: '',
            index: -1,
            warmupTime: self.schedule.defaultWarmupTime(),
            template: 'block-template'
        });
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
        day.entries.push(item);
    };

    self.entryTypeDurations = {
        'Performance': self.schedule.defaultDuration,
        'Open': self.schedule.defaultDuration,
        'Break': self.schedule.defaultBreakDuration,
        'Awards': self.schedule.defaultAwardsDuration
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
                    var duration = prev.duration || self.entryTypeDurations[prev.type()];
                    entry.time(new Date(prev.time().getTime() + duration() * 60 * 1000));
                }
            }
        }, day.entries);
    });

    self.calculateWarmup = function (entry) {
        var warmup = entry.warmupTime || self.schedule.defaultWarmupTime;
        var result = new Date(entry.time().getTime() - warmup() * 60 * 1000);
        return result;
    };

    self.displayOptions = ko.observable(false);
    self.toggleOptions = function () {
        self.displayOptions(!self.displayOptions());
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
});