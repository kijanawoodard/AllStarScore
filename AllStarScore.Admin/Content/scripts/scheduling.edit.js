AllStarScore.Scheduling = {
    Start: function() {
        $('#scheduling_edit .selectable').selectable({ filter: "li" });

        var viewModel = new AllStarScore.Scheduling.EditViewModel();
        ko.applyBindings(viewModel, document.getElementById('scheduling_edit'));

        $('#scheduling_edit .sortable').disableSelection(); //http://stackoverflow.com/a/9993099/214073 sortable and selectable
    }
};

AllStarScore.Scheduling.DayModel = function (data) {
    data.day = new Date(data.day);
    ko.mapping.fromJS(data, AllStarScore.Scheduling.DayMapping, this);
};

AllStarScore.Scheduling.EntryModel = function (data) {
    data.time = new Date(data.time);
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
    'include': [],
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

    var data = AllStarScore.CompetitionData;
    self.performances = data.performances;
    
    _.each(data.raw.divisions, function (division) {
        AllStarScore.Scheduling.ScheduleMapping.include.push(division.id); //skirt this issue: https://groups.google.com/forum/?fromgroups=#!topic/knockoutjs/QoubswdzIxI; this works because we know all the possible keys
    });

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

    var addPerformance = function (performance) {
        var model = {
            'performanceId': performance.id,
            'type': 'Performance',
            'time': new Date()
        };

        model = new EntryModel(model);
        self.unscheduled.push(model);
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
        'Performance': { duration: self.schedule.defaultDuration, template: 'registration-template' },
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