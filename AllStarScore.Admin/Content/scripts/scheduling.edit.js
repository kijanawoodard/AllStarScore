$(document).ready(function () {
    var registrations = $.map(window.editScheduleData.registrations, function (item) {
        return { item: item, time: '', index: -1, duration: 15, template: 'registration-template' };
    });
        
    //console.log(ko.toJSON(window.editScheduleData));
    //console.log(ko.toJSON(schedule));

    var data = {
        schedule: window.editScheduleData.schedule,
        registrations: window.editScheduleData.registrations
    };

    data.schedule[0].items.push.apply(data.schedule[0].items, registrations);
    data.schedule[0].items.push({ item: { text: 'Break' }, time: '', index: -1, duration: 20, template: 'block-template' });
    data.schedule[1].items.push({ item: { text: 'Awards' }, time: '', index: -1, duration: 20, template: 'block-template' });


    var viewModel = new EditScheduleViewModel(data);
    ko.applyBindings(viewModel, document.getElementById('scheduling_edit'));

    //http://stackoverflow.com/a/9993099/214073 sortable and selectable
    $('#scheduling_edit .sortable').disableSelection();
    
});

var EditScheduleViewModel = (function (data) {
    var self = this;
    var hook = $('#scheduling_edit');

    //clean up datetimes
    $.each(data.schedule, function (index, item) {
        item.day = new Date(item.day);
    });

    self.registrations = data.registrations;
    self.schedule = ko.mapping.fromJS(data.schedule);
    self.unscheduled = ko.mapping.fromJS(function () {
        return $.grep(data.registrations, function (registration) {
            var schedules = $.map(data.schedule, function(item) { return item.items; });
            return $.grep(schedules, function (day) {
                return registration.id == day.id;
            }).length == 0;
        });
    });

    //recalculate time when we move items around
    $.each(self.schedule(), function (index, unit) {
        unit.items.subscribe(function () {
            var items = unit.items();
            for (var i = 0, j = items.length; i < j; i++) {
                var item = items[i];
                if (i == 0) {
                    item.time(unit.day());
                }
                else {
                    var prev = items[i - 1];
                    item.time(new Date(prev.time().getTime() + prev.duration() * 60 * 1000));
                }
            }
        }, unit.items);
    });


    $.each(self.schedule(), function (index, unit) {
        unit.items.valueHasMutated(); //we loaded the items before subscribe, so force subscribe function now
    });
    return self;
});