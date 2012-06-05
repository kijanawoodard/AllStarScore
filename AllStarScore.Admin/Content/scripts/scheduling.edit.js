$(document).ready(function () {
    var schedule = [{ day: new Date(2012, 0, 1, 8, 15, 0), items: []}];

    var registrations = $.map(window.editScheduleData.registrations, function (item) {
        return { item: item, time: '', index: -1, duration: 10, template: 'registration-template' };
    });

    schedule[0].items.push.apply(schedule[0].items, registrations);

    schedule[0].items.push({ item: { text: 'Break' }, time: '', index: -1, duration: 20, template: 'block-template' });
    schedule[0].items.push({ item: { text: 'Awards' }, time: '', index: -1, duration: 20, template: 'block-template' });


    //console.log(ko.toJSON(window.editScheduleData));
    //console.log(ko.toJSON(schedule));
    var data = {
        schedule: schedule
    }; 
    
    var viewModel = new EditScheduleViewModel(data);
    ko.applyBindings(viewModel, document.getElementById('scheduling_edit'));

    //http://stackoverflow.com/a/9993099/214073 sortable and selectable
    $('#scheduling_edit .sortable').disableSelection();
});

var EditScheduleViewModel = (function (data) {
    var self = this;
    var hook = $('#scheduling_edit');

    self.starttime = new Date(2012, 0, 1, 8, 15, 0);

    self.items = ko.mapping.fromJS(data.schedule[0].items);
    self.schedule = data.schedule;
    
    self.items.subscribe(function () {
        var items = self.items();
        for (var i = 0, j = items.length; i < j; i++) {
            var item = items[i];
            if (i == 0) {
                item.time(self.starttime);
            }
            else {
                var prev = items[i - 1];
                item.time(new Date(prev.time().getTime() + prev.duration() * 60 * 1000));
            }
        }
    }, self);

    self.items.valueHasMutated(); //we loaded the items before subscribe, so force subscribe function now
    
    return self;
});