$(document).ready(function () {
    //console.log(AllStarScore);
    AllStarScore.LevelDetails = new AllStarScore.LevelDetails(window.levelData); //assigned the data in the view
    ko.applyBindings(AllStarScore);
});

AllStarScore.LevelDetailsMapping = {
    'divisions': {
        create: function (options) {
            var result = new AllStarScore.DivisionViewModel(options.data);
            return result;
        }
    }
};

AllStarScore.DivisionViewModel = function(data) {
    var self = this;
    self.id = data.id;
    self.name = ko.protectedObservable(data.name);
    return self;
};

AllStarScore.LevelDetails = function (data) {
    var self = this;

    ko.mapping.fromJS(data, AllStarScore.LevelDetailsMapping, self);

    self.sortFunction = function (a, b) {
        return a.name().toLowerCase() > b.name().toLowerCase() ? 1 : -1;
    };

    self.sorted = ko.computed(function () {
        return self.divisions.slice().sort(self.sortFunction);
    }, self);

    self.selectedItem = ko.observable();

    self.templateToUse = function (item) {
        return self.selectedItem() === item ? 'editTmpl' : 'itemTmpl';
    };

    this.deleteItem = function (itemToDelete) {
        self.divisions.remove(itemToDelete);
        self.selectedItem(null);
    };

    this.editItem = function (item) {
        self.selectedItem(item);
        $('tbody input').select().focus();
    };

    var doActionOnSelected = function (action) {
        var item,
            selected = self.selectedItem();

        for (var prop in selected) {
            if (selected.hasOwnProperty(prop)) {
                item = selected[prop];
                if (ko.isObservable(item) && item[action]) {
                    item[action]();
                }
            }
        }
    };

    this.acceptItemEdit = function () {
        doActionOnSelected("commit");
        var item = self.selectedItem();
        console.log(item.id);
        var post = JSON.stringify({ id: item.id, name: item.name() });
        $.ajax({
            url: window.divisionEditLink,
            type: 'POST',
            data: post,
            dataType: "json",
            contentType: 'application/json, charset=utf-8',
            success: function () {
                self.selectedItem(null);
            }
        });
    };

    this.cancelItemEdit = function () {
        doActionOnSelected("reset");
        self.selectedItem(null);
    };

    $('#level_details').on('submit', 'form', function (event) {
        event.preventDefault();

        var form = $(this);
        var post = JSON.stringify({ levelid: self.level.id(), name: form.find('input[name="name"]').val() });

        form.ajaxPost({
            data: post,
            success: function (result) {
                var division = new AllStarScore.DivisionViewModel(result);
                self.divisions.push(division);
                form[0].reset();
            }
        });
    });
};