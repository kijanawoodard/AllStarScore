$(document).ready(function () {
    $('#scheduling_edit .sortable')
        .sortable({ handle: ".handle" })
        .selectable()
        .find("li")
        .addClass("ui-corner-all")
        .prepend("<div class='handle'><span class='ui-icon ui-icon-carat-2-n-s'></span></div>");
});

$(document).ready(function () {
    var schedule = [{ text: 'Hello', order: 1 }, { text: 'There', order: 1}];
    var viewModel = new EditScheduleViewModel(schedule);
    ko.applyBindings(viewModel, document.getElementById('scheduling_edit'));
});

var EditScheduleViewModel = (function (data) {
    var self = this;
    var hook = $('#scheduling_edit');



    self.items = ko.mapping.fromJS(data);

    self.items.subscribe(function () {
        console.log('hi');
        var items = self.items();
        for (var i = 0, j = items.length; i < j; i++) {
            var item = items[i];
            if (!item.index) {
                item.index = ko.observable(i);
            } else {
                item.index(i);
            }
        }
    }, self);

    //setInterval(function() { self.items.valueHasMutated(); }, 500);
    return self;
});