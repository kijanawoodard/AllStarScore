var TeamRegistrationModel = function (data) {
    var self = this;
    var form = $('#team_registration form.create');
    var editForm = $('#team_registration form.teams');

    self.data = data;
    self.divisions = data.divisions;
    self.teams = ko.observableArray();

    self.getDivisionId = function (name) {
        var result = $.grep(self.divisions, function (item) {
            return item.label == name;
        });

        if (result.length > 0) {
            return result[0].id;
        } else {
            return undefined;
        }
    };

    self.getDivisionName = function (id) {
        var result = $.grep(self.divisions, function (item) {
            return item.id == id;
        });

        if (result.length > 0) {
            return result[0].label;
        } else {
            return undefined;
        }
    };

    self.editing = ko.observable(false);
    var saveOriginalTeam = undefined;

    self.addTeam = function (team) {
        team.divisionName = self.getDivisionName(team.divisionId);
        team.editing = ko.observable(false);
        //console.log(team.divisionName);
        //console.log(ko.toJSON(team));
        self.teams.push(team);
    };

    $.each(self.data.teams, function (index, team) {
        self.addTeam(team);
    });

    self.editTeam = function (team) {
        saveOriginalTeam = ko.toJSON(team);
        team.editing(true);
        self.editing(true);
    };

    self.cancelEdit = function (team) {

        console.log(ko.toJSON(team));
        var k = ko.mapping.fromJSON(saveOriginalTeam);
        team.divisionName(k.divisionName);
        console.log(ko.toJSON(team));

        team.editing(false);
        self.editing(false); //compute?


        editForm.data('validator').resetForm();
    };

    self.removeTeam = function (team) {
        self.teams.remove(team);
    };

    self.createNew = {
        divisionName: ko.observable(),
        teamName: ko.observable(),
        participantCount: ko.observable(),
        isShowTeam: ko.observable()
    };

    self.resetCreateForm = function () {
        self.createNew.divisionName('');
        self.createNew.teamName('');
        self.createNew.participantCount('');
        self.createNew.isShowTeam(false);
        form.data('validator').resetForm(); //http://stackoverflow.com/a/2060530/214073
    };

    self.save = function (team) {
        var ok = editForm.valid();
        if (!ok) return;

        console.log(ko.toJSON(team));
        team.editing(false);
        self.editing(false); //compute?
    };

    self.create = function () {
        self.createNew.divisionId = self.getDivisionId(self.createNew.divisionName());
        self.createNew.competitionId = self.data.competitionId;
        self.createNew.gymId = self.data.gymId;

        form.ajaxPost({
            data: ko.toJS(self.createNew),
            success: function (result) {
                //console.log(ko.toJSON(result));
                self.addTeam(ko.toJS(result));
                self.resetCreateForm();
            }
        });
    };
};

var autoComplete = function (element, source) {
    
    $(element).autocomplete({
        source: source,
        minLength: 0,
        select: function (event, ui) {
            $(this)
                .val(ui.item.label)
                .change()
                .valid();
            return false;
        }
    })
    .focus(function () {
        $(this)
            .autocomplete('search', '') //make the list drop down when you enter the field
            .select();

    })
    .data("autocomplete")._renderItem = function (ul, item) {
        //http://stackoverflow.com/a/3457773/214073
        var t = item.label.replace(new RegExp("(?![^&;]+;)(?!<[^<>]*)(" + $.ui.autocomplete.escapeRegex(this.term) + ")(?![^<>]*>)(?![^&;]+;)", "gi"), "<strong>$1</strong>");
        return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<a>" + t + "</a>")
                    .appendTo(ul);
    };
};

var autoCompleteUpdate = function (element, source) {
    $(element).autocomplete("option", "source", source);
};

$(document).ready(function () {
    var data = window.registrationTeamsData;
    
    //format the data for the autocomplete dropdown
    data.divisions = $.map($.makeArray(data.divisions), function (item) {
        return { label: item.level + " - " + item.division, id: item.divisionId };
    });

    var viewModel = new TeamRegistrationModel(data);

    //check division field to make sure it's valid
    $.validator.addMethod("validateDivision", function (value, element) {
        return viewModel.getDivisionId(value);
    }, "Please choose a valid division");

    //http://stackoverflow.com/a/1090408/214073
    $.validator.addMethod('positiveNumber', function (value) {
        return Number(value) > 0;
    }, 'Enter a positive number.');

    $('form.create').validate({
        submitHandler: function () {
            viewModel.create(this.currentForm);
        },
        rules: {
            division: { validateDivision: true },
            participantCount: { positiveNumber: true }
        }
    });

    $('form.teams').validate({
        rules: {
            division: { validateDivision: true },
            participantCount: { positiveNumber: true }
        }
    });

    console.log(data.teams);

    ko.bindingHandlers.ko_autocomplete = {
        init: function (element, params) {
            autoComplete(element, params().source);
        },
        update: function (element, params) {
            $(element).autocomplete("option", "source", params().source);
        }
    };

    ko.applyBindings(viewModel, document.getElementById("team_registration"));
});
