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
//        description: ko.observable(),
        firstDay: ko.observable(),
//        lastDay: ko.observable(),
        numberOfDays: ko.observable(),
        competitionStyle: ko.observable(),
        numberOfPanels: ko.observable()
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
//        self.post.description('');
        self.post.firstDay($.datepicker.formatDate('mm/dd/yy', new Date()));
//        self.post.lastDay($.datepicker.formatDate('mm/dd/yy', new Date()));
        self.post.numberOfDays(1);
        self.post.competitionStyle(1);
        self.post.numberOfPanels(1);

        self.post.numberOfPerformances = ko.computed(function () {
            if (self.post.competitionStyle() == 2) {
                return 2;
            } else {
                return 1;
            }
        });

        self.post.isWorldsCompetition = ko.computed(function () {
            return self.post.competitionStyle() == 3;
        });

        self.post.panels = ko.computed(function () {
            return _.map(_.range(self.post.numberOfPanels()), function (i) {
                return String.fromCharCode(65 + i);
            });
        });

        form.find('.validation-summary-errors').empty();
        self.focusName(true);
    };

    self.create = function (formToPost) {
//        console.log(ko.toJSON(self.post));
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
        
        form.find('input[name=firstDay]')
            .datepicker({
                numberOfMonths: 2
            });
    };

    self.reset();
    self.setup();
    return self;
});