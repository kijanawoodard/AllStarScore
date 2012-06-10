$(document).ready(function () {
    var viewModel = new CreateCompetitionViewModel();
    ko.applyBindings(viewModel, document.getElementById('competition_create'));
});

var CreateCompetitionViewModel = (function () {
    var self = this;
    var hook = $('#competition_create');
    var form = hook.find('form');

    self.post = {
        competitionName: ko.observable(),
        description: ko.observable(),
        firstDay: ko.observable(),
        lastDay: ko.observable()
    };

    self.shouldShowForm = ko.observable(false);
    self.focusName = ko.observable(true);

    self.cancelCreation = function () {
        self.reset();
        self.toggleFormVisibility();
    };

    self.toggleFormVisibility = function () {
        self.shouldShowForm(!self.shouldShowForm());
        self.focusName(true);
    };

    self.reset = function () {
        self.post.competitionName('');
        self.post.description('');
        self.post.firstDay($.datepicker.formatDate('mm/dd/yy', new Date()));
        self.post.lastDay($.datepicker.formatDate('mm/dd/yy', new Date()));
        form.find('.validation-summary-errors').empty();
        self.focusName(true);
    };

    self.create = function (formToPost) {
        //console.log(ko.toJSON(self.post));
        form.ajaxPost({
            data: ko.toJSON(self.post),
            success: function (result) {
                //console.log(ko.toJSON(result));
                $.publish('/competition/created');
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

        form.find('input[name=firstDay]').pickDateBefore('input[name=lastDay]');
    };

    self.reset();
    self.setup();
    return self;
});