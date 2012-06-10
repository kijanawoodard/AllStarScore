$(document).ready(function () {
    var viewModel = new CreateGymViewModel(window.gymCreateData);
    ko.applyBindings(viewModel, document.getElementById('gym_create'));

    $.subscribe('/gym/create/cancelled', function () {
        viewModel.cancelCreation();
    });
});

//http://www.joezimjs.com/javascript/javascript-closures-and-the-module-pattern/
var CreateGymViewModel = (function (data) {
    var self = this;
    var hook = $('#gym_create');
    var form = hook.find('form');

    //format the data for the autocomplete dropdown
    self.gyms = $.map($.makeArray(data.gyms), function (item) {
        return { label: item.gymName + " from " + item.location, id: item.gymId, match: item.gymName };
    });

    self.post = {
        gymName: ko.observable(),
        location: ko.observable(),
        isSmallGym: ko.observable()
    };

    self.shouldShowForm = ko.observable(false);
    self.focusName = ko.observable(true);
    self.shouldAllowCreate = ko.observable(true);
    self.nameIsAvailable = ko.observable(false);
    self.nameIsTaken = ko.observable(false);

    self.gymNameMatched = function (result) {
        if (result.isMatch) {
            self.shouldAllowCreate(false);
            self.nameIsAvailable(false);
            self.nameIsTaken(true);
            $.publish('/gym/name/taken', result.id);
        }
        else {
            self.shouldAllowCreate(true);
            self.nameIsAvailable(result.inputHasValue);
            self.nameIsTaken(false);
            $.publish('/gym/name/available');
        }
    };

    self.cancelCreation = function () {
        self.reset();
        self.toggleFormVisibility();
    };

    self.toggleFormVisibility = function () {
        self.shouldShowForm(!self.shouldShowForm());
        self.focusName(self.shouldShowForm());
    };

    self.reset = function () {
        self.post.gymName('');
        self.post.location('');
        self.post.isSmallGym(true);
        form.find('.validation-summary-errors').empty();
        self.focusName(self.shouldShowForm());
        self.shouldAllowCreate(true);
        self.nameIsAvailable(false);
        self.nameIsTaken(false);
    };

    self.create = function () {
        //console.log(ko.toJSON(self.post));
        form.ajaxPost({
            data: ko.toJSON(self.post),
            success: function (result) {
                //console.log(ko.toJSON(result));
                var gym = ko.toJS(result);
                $.publish('/gym/created', gym.id);
                self.reset();
            }
        });
    };

    self.setup = function () {
        form.validate({
            debug: true,
            submitHandler: function () {
                self.create(this.currentForm);
            }
        });
    };

    self.reset();
    self.setup();
    return self;
});