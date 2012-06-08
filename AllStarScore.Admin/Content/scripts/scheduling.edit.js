$(document).ready(function () {
    
    $('#scheduling_edit .selectable').selectable({ filter: "li" });

    var viewModel = new EditScheduleViewModel(window.editScheduleData);
    ko.applyBindings(viewModel, document.getElementById('scheduling_edit'));

    $('#scheduling_edit .sortable').disableSelection(); //http://stackoverflow.com/a/9993099/214073 sortable and selectable
});

var EditScheduleViewModel = (function (data) {
    var self = this;
    var hook = $('#scheduling_edit');

    //clean up datetimes
    data.schedule.days = _.map(data.schedule.days, function (day) {
        return new Date(day);
    });

    //flatten divisions 
    data.divisions = _.map(data.divisions, function (item) {
        return item.divisionId;
    });

    //sort registrations in the division order
    data.registrations = _.sortBy(data.registrations, function (item) {
        return _.indexOf(data.divisions, item.divisionId);
    });

    //add tracking flag
    $.each(data.registrations, function (index, item) {
        item.scheduled = false;
        item.selected = true;
    });

    self.schedule = ko.mapping.fromJS(data.schedule);
    self.registrations = ko.mapping.fromJS(data.registrations);
    self.unscheduled = ko.computed(function () {
        return $.grep(self.registrations(), function (item) {
            return item.scheduled() == false;
        });
    }, self);

    self.panels = ko.computed(function () {
        return _.map(_.range(self.schedule.numberOfPanels()), function (i) {
            return String.fromCharCode(65 + i);
        });
    });

    self.divisions = data.divisions;

    var getDivisionPanel = function (node) {
        node = ko.toJS(node);
        return 'A';
        return ko.utils.arrayFirst(self.schedule.divisionPanels(), function (dp) {
            return dp.divisionId() == node.data.divisionId;
        });
    };

    self.getPanel = function (node) {
        return (getDivisionPanel(node) || {}).panel;
    };
    self.shiftPanel = function (node) {
        var dp = getDivisionPanel(node);

        var panels = ko.toJS(self.panels);
        var index = _.indexOf(panels, dp.panel()) + 1;
        var result = panels[index] || panels[0]; //get the next one or the first one

        dp.panel(result);
    };

    self.calculateWarmup = function (node) {
        return new Date(node.time().getTime() - node.warmupTime() * 60 * 1000);
    };

    self.scheduleTeam = function (day) {
        self.scheduleTeams(day, self.unscheduled());
    };

    self.scheduleTeams = function (day, registrations) {
        ko.utils.arrayForEach(registrations, function (registration) {
            if (registration.selected()) {
                var json = prototype();
                json.data = registration;
                json.registrationId(registration.id());
                json.time(day);
                json.panel = ko.computed(function () {
                    return self.getPanel(this);
                }, json);

                json.duration(self.schedule.defaultDuration());
                json.template('registration-template');

                //if panel undefined for the division, push the first panel in for this division
                self.getPanel(json) ||
                    self.schedule.divisionPanels.push({ divisionId: json.data.divisionId, panel: ko.observable(self.panels()[0]) });

                self.schedule.entries.push(json);
            }
        });
    };

    var prototype = function () {
        return {
            data: { text: ko.observable('') },
            registrationId: ko.observable(''),
            time: ko.observable(''),
            index: ko.observable(-1),
            duration: ko.observable(20),
            warmupTime: ko.observable(self.schedule.defaultWarmupTime()),
            template: ko.observable('block-template')
        };
    };

    self.addBreak = function (day) {
        var item = prototype();
        item.data.registrationId('break');
        item.data.text('Break');
        day.entries.push(item);
    };

    self.addAwards = function (day) {
        var item = prototype();
        item.data.registrationId('awards');
        item.data.text('Awards');
        day.entries.push(item);
    };

    self.addOpen = function (day) {
        var item = prototype();
        item.data.registrationId('open');
        item.data.text('Open');
        item.duration(self.schedule.defaultDuration());
        day.entries.push(item);
    };

    //recalculate time when we move items around
    self.schedule.entries.subscribe(function () {
        var entries = self.schedule.entries();
        for (var i = 0, j = entries.length; i < j; i++) {
            var entry = entries[i];
            if (i == 0) {
                var day = entry.time();
                entry.time(new Date(day.getFullYear(), day.getMonth(), day.getDate()));
            }
            else {
                var prev = entries[i - 1];
                entry.time(new Date(prev.time().getTime() + prev.duration() * 60 * 1000));
            }

            //flag this item as scheduled
            var registrations = self.registrations();
            for (var key in registrations) {
                if (registrations[key].id() == entry.registrationId()) {
                    registrations[key].scheduled(true);
                    break;
                }
            }
        }
    }, self.schedule.entries);


    self.schedule.entries.valueHasMutated(); //we loaded the items before subscribe, so force subscribe function now

    var r = self.unscheduled().slice();
    _.each(self.schedule.days, function (day) {
        //self.scheduleTeams(day, r);
    });

    self.calculatePerformancePosition = function (target) {
        var result = 0;

        _.chain(self.schedule.entries())
            .find(function (entry) {
                if (target.id() == entry.registrationId()) {
                    result++;
                }

                return target == entry;
            });

        return [, '1st', '2nd', '3rd', '4th', '5th'][result];
    };


    self.save = function () {
        $('#scheduling_edit form').ajaxPost({
            data: ko.toJSON(self.schedule),
            success: function (result) {
                //console.log(ko.toJSON(result));
                console.log('saved');
            }
        });
    };

    return self;
});