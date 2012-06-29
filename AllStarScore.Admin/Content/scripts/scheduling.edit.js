var viewModel;
var competitionDaysAreTheSame = true; //look out for this edge case; //TODO: need to recreate days; server?

$(document).ready(function () {

    $('#scheduling_edit .selectable').selectable({ filter: "li" });

    viewModel = new EditScheduleViewModel(window.editScheduleData);
    ko.applyBindings(viewModel, document.getElementById('scheduling_edit'));

    $('#scheduling_edit .sortable').disableSelection(); //http://stackoverflow.com/a/9993099/214073 sortable and selectable
    viewModel.doLoad();
});

var DayModel = function (data) {
    data.day = new Date(data.day);
    ko.mapping.fromJS(data, mapping, this);

    if (!competitionDaysAreTheSame) {
        this.entries.removeAll();
    }
};

var EntryModel = function (data) {
    data.time = new Date(data.time);
    ko.mapping.fromJS(data, {}, this);

    var self = this;
    this.registration = ko.computed(function () {
        if (!this.registrationId)
            return undefined;

        var id = getRegistrationId(this.registrationId());
        return window.viewModel.registrations[id];
    }, self);

    this.panel = ko.computed(function () {
        return self.registration() ? viewModel.getPanel(self.registration().divisionId())() : '';
    }, self);
};

var getRegistrationId = function(id) {
    return id.charAt(0).toLowerCase() + id.slice(1); //might change name of registration class in raven to avoid this
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

    //check to see that competition days haven't changed on us
    $.each(data.schedule.days, function (index, day) {
        if (competitionDaysAreTheSame) { //once this is false, leave it false
            var comp = new Date(data.competitionDays[index]);
            competitionDaysAreTheSame = areSameDay(new Date(day.day), comp);
        }
    });

    //map divisions to just id
    data.divisions = _.pluck(data.divisions, 'divisionId');

    //add tracking flag
    $.each(data.registrations, function (index, item) {
        //item.selected = false;
    });

    self.schedule = ko.mapping.fromJS(data.schedule, mapping);
    self.registrations = ko.mapping.fromJS(data.registrations);

    var getAllEntries = function () {
        return _.chain(self.schedule.days())
                    .map(function (day) {
                        return day.entries();
                    })
                    .flatten()
                    .value();
    };

    var registrationIsScheduled = function (id) {
        return _.any(getAllEntries(), function (entry) {
            return entry.registrationId && entry.registrationId() == id;
        });
    };

//    self.scheduled = ko.computed(function () {
//        var all = getAllEntries();
//        var grouped = _.groupBy(all, function (entry) {
//            return entry.registrationId ? entry.registrationId() : entry.type();
//        });
//        return grouped;
//    }, self);

    self.competitionDays = data.competitionDays;

    self.panels = ko.computed(function () {
        return _.map(_.range(self.schedule.numberOfPanels()), function (i) {
            return String.fromCharCode(65 + i);
        });
    });

    self.divisions = data.divisions;
    self.numberOfPerformances = data.numberOfPerformances;

    self.getPanel = function (divisionId) {
        var result =
            self.schedule.divisionPanels[divisionId] ?
            self.schedule.divisionPanels[divisionId] :
            self.schedule.divisionPanels[divisionId] = ko.observable(self.panels()[0]);

        return result;
    };

    self.shiftPanel = function (node) {
        var dp = self.getPanel(node.registration().divisionId());

        var panels = ko.toJS(self.panels);
        var index = _.indexOf(panels, dp()) + 1;
        var result = panels[index] || panels[0]; //get the next one or the first one

        dp(result);
    };

    self.calculateWarmup = function (node) {
        return new Date(node.time().getTime() - node.warmupTime() * 60 * 1000);
    };

    self.scheduleTeams = function (day, registrations) {
        ko.utils.arrayForEach(registrations, function (registration) {
            var json = {}; // prototype();
            json.registrationId = registration.id();
            json.time = day.day ? day.day().clone().clearTime() : new Date();
            json.duration = self.schedule.defaultDuration();
            json.warmupTime = self.schedule.defaultWarmupTime(),
            json.index = -1,
            json.template = 'registration-template';

            json = new EntryModel(json, self);

            day.entries.push(json);
        });
    };

    self.unscheduled = ko.observableArray();

    self.doLoad = function () {
        var list = _.filter(self.registrations, function (item) {
            return item.id && !registrationIsScheduled(item.id());
        });

        var sorted = _.sortBy(list, function (item) {
            return _.indexOf(data.divisions, item.divisionId());
        });

        var result = {};
        result.entries = self.unscheduled; //HACK to reuse scheduleTeams

        for (var i = 0; i < self.numberOfPerformances; i++) {
            self.scheduleTeams(result, sorted);
        }
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

    self.schedule.days.valueHasMutated(); //we loaded the items before subscribe, so force subscribe function now

    self.calculatePerformancePosition = function (target) {
        var result = 0;
        if (!target.registration)
            return '';

        _.chain(getAllEntries())
            .find(function (entry) {
                if (target.registrationId &&
                    entry.registrationId &&
                    target.registrationId() == entry.registrationId()) {
                    result++;
                }
                return target == entry;
            });

        return [, '1st', '2nd', '3rd', '4th', '5th'][result];
    };


    self.save = function () {
        form.ajaxPost({
            data: ko.mapping.toJSON(self.schedule),
            success: function (result) {
                console.log('saved');
            }
        });
    };

    return self;
});