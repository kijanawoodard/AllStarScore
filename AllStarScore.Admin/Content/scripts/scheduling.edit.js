$(document).ready(function () {
    //console.log(ko.toJSON(window.editScheduleData));
    
    var data = {
        schedule: window.editScheduleData.schedule,
        registrations: window.editScheduleData.registrations
    };

    //    data.schedule[0].items.push({ data: { text: 'Break', id: '' }, time: '', index: -1, duration: 20, template: 'block-template' });
    //    data.schedule[1].items.push({ data: { text: 'Awards', id: '' }, time: '', index: -1, duration: 20, template: 'block-template' });

    $('#scheduling_edit .selectable').selectable({ filter: "li" });

    var viewModel = new EditScheduleViewModel(data);
    ko.applyBindings(viewModel, document.getElementById('scheduling_edit'));

    //http://stackoverflow.com/a/9993099/214073 sortable and selectable
    $('#scheduling_edit .sortable').disableSelection();
});

var EditScheduleViewModel = (function (data) {
    var self = this;
    var hook = $('#scheduling_edit');

    //clean up datetimes
    $.each(data.schedule.days, function (index, item) {
        item.day = new Date(item.day);
    });

    //add tracking flag
    $.each(data.registrations, function (index, item) {
        item.scheduled = false;
        item.selected = true;
    });

    self.registrations = ko.mapping.fromJS(data.registrations);
    self.schedule = ko.mapping.fromJS(data.schedule);
    self.unscheduled = ko.computed(function () {
        return $.grep(self.registrations(), function (item) {
            return item.scheduled() == false;
        });
    }, self);
    self.panels = ko.computed(function () {
        var result = [];
        for (var i = 0; i < self.schedule.numberOfPanels(); i++) {
            result.push(String.fromCharCode(65 + i));
        }
        return result;
    });

    self.scheduleTeam = function (node) {
        ko.utils.arrayForEach(self.unscheduled(), function (r) {
            if (r.selected()) {
                var json = {
                    data: r,
                    id: ko.observable(r.id()),
                    time: ko.observable(''),
                    index: ko.observable(-1),
                    duration: ko.observable(self.schedule.defaultDuration()),
                    template: ko.observable('registration-template')
                };

                node.entries.push(json);
            }
        });
    };

    var prototype = function () {
        return {
            data: { text: ko.observable('') },
            id: ko.observable(''),
            time: ko.observable(''),
            index: ko.observable(-1),
            duration: ko.observable(20),
            template: ko.observable('block-template')
        };
    };

    self.addBreak = function (day) {
        var item = prototype();
        item.data.text('Break');
        console.log(item);
        day.entries.push(item);
    };

    self.addAwards = function (day) {
        var item = prototype();
        item.data.text('Awards');
        day.entries.push(item);
    };

    self.addOpen = function (day) {
        var item = prototype();
        item.data.text('Open');
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

                //flag this item as scheduled
                var registrations = self.registrations();
                for (var key in registrations) {
                    if (registrations[key].id() == entry.id()) {
                        registrations[key].scheduled(true);
                        break;
                    }
                }
            }
        }, unit.entries);
    });


    $.each(self.schedule.days(), function (index, unit) {
        unit.entries.valueHasMutated(); //we loaded the items before subscribe, so force subscribe function now
    });

    return self;
});