$(document).ready(function () {
    //console.log(ko.toJSON(window.editScheduleData));

    var data = {
        schedule: window.editScheduleData.schedule,
        registrations: window.editScheduleData.registrations
    };

    data.schedule[0].items.push({ data: { text: 'Break', id: '' }, time: '', index: -1, duration: 20, template: 'block-template' });
    data.schedule[1].items.push({ data: { text: 'Awards', id: '' }, time: '', index: -1, duration: 20, template: 'block-template' });

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
    $.each(data.schedule, function (index, item) {
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

    self.scheduleTeam = function (node) {
        ko.utils.arrayForEach(self.unscheduled(), function (r) {
            if (r.selected()) {
                var json = {
                    data: r,
                    time: ko.observable(''),
                    index: ko.observable(-1),
                    duration: ko.observable(15),
                    template: ko.observable('registration-template')
                };
                
                node.items.push(json);
            }
        });
    };
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

                //flag this item as scheduled
                var registrations = self.registrations();
                for (var key in registrations) {
                    if (registrations[key].id() == item.data.id()) {
                        registrations[key].scheduled(true);
                        break;
                    }
                }
            }
        }, unit.items);
    });


    $.each(self.schedule(), function (index, unit) {
        unit.items.valueHasMutated(); //we loaded the items before subscribe, so force subscribe function now
    });

    return self;
});