$(document).ready(function () {
    window.gymEditData.gyms = window.gymCreateData.gyms; //combine

    var viewModel = new EditGymViewModel(window.gymEditData);
    ko.applyBindings(viewModel, document.getElementById('gym_edit'));    
});

//http://www.joezimjs.com/javascript/javascript-closures-and-the-module-pattern/
var EditGymViewModel = (function (data) {
    var self = this;
    var hook = $('#gym_edit');
    var form = hook.find('form');

    self.data = data;

    //format the data for the autocomplete dropdown
    self.gyms = $.map($.makeArray(data.gyms), function (item) {
        return { label: item.gymName + " from " + item.location, id: item.gymId, match: item.gymName };
    });

    self.post = ko.mapping.fromJSON(self.data.gym);

    self.editing = ko.observable(false);
    this.edit = function () {
        this.editing(true);
    };

    self.shouldShowForm = ko.observable(false);
    self.focusName = ko.observable(true);
    self.shouldAllowName = ko.observable(true);
    self.nameIsAvailable = ko.observable(false);
    self.nameIsTaken = ko.observable(false);

    self.gymNameMatched = function (result) {
        if (result.isMatch && result.id != self.post.gymId()) {
            self.shouldAllowName(false);
            self.nameIsAvailable(false);
            self.nameIsTaken(true);
            $.publish('/gym/name/taken', result.id);
        }
        else {
            self.shouldAllowName(true);
            self.nameIsAvailable(result.inputHasValue);
            self.nameIsTaken(false);
            $.publish('/gym/name/available');
        }
    };

    self.cancel = function () {
        self.reset();
    };

    self.reset = function () {
        ko.mapping.fromJSON(self.data.gym, self.post);

        form.find('.validation-summary-errors').empty();
        self.focusName(false);
        self.editing(false);
        self.shouldAllowName(true);
        self.nameIsAvailable(false);
        self.nameIsTaken(false);
    };

    self.save = function () {
        //console.log(ko.toJSON(self.post));
        form.ajaxPost({
            data: ko.toJS(self.post),
            success: function (result) {
                //console.log(ko.toJSON(result));
                self.data.gym = ko.toJSON(result); //reset our original data with what we just stored; reset will make sure the form is up to date; string trimming is done server side
                self.reset();
            }
        });
    };

    self.setup = function () {
        form.validate({
            debug: true,
            submitHandler: function () {
                self.save(this.currentForm);
            }
        });

        self.shouldShowForm(true);
    };

    self.reset();
    self.setup();
    return self;
});