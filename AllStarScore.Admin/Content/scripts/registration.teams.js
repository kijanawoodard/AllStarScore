var TeamRegistrationModel = function (data) {
    var self = this;
    self.data = data;
    self.teams = ko.observableArray(data.teams);
    self.divisions = self.data.divisions;

    self.addTeam = function (team) {
        self.teams.push(team);
    };

    self.removeTeam = function (team) {
        self.teams.remove(team);
    };

    self.resetCreateForm = function () {
        $('#team_registration form.create')[0].reset();
        //$('#team_registration form.create .division').focus();
    };

    self.getDivisionId = function (name) {
        for (var i = 0; self.divisions.length > i; i += 1) {
            if (self.divisions[i].label === name) {
                return divisions[i].id;
            }
        }

        return undefined;
    };

    self.createNew = {
        divisionName: ko.observable(),
        teamName: ko.observable(),
        isShowTeam: ko.observable()
    };

    self.save = function (form) {
        alert("Could now transmit to server: " + ko.utils.stringifyJson(self.teams));
        // To actually transmit to server as a regular form post, write this: ko.utils.postJson($("form")[0], self.gifts);
    };

    self.create = function (form) {
        self.createNew.divisionId = self.getDivisionId(self.createNew.divisionName());
        self.createNew.competitionId = self.data.competitionId;
        self.createNew.gymId = self.data.gymId;

        $.ajax({
            url: form.action,
            type: form.method,
            data: ko.toJS(self.createNew), //$(this).serialize(),
            success: function (result) {
                //$(target).html(result);
                alert(ko.toJSON(result));
            }
        });
    };
};

$(document).ready(function () {
    var viewModel = new TeamRegistrationModel(data);
    
    //check division field to make sure it's valid
    $.validator.addMethod("validateDivision", function (input, element) {
        return viewModel.getDivisionId(input);
    }, "Please choose a valid division");

    $('form.create').validate({
        debug: true,
        submitHandler: function () {
            viewModel.create(this.currentForm);
        },
        rules: {
            division: { validateDivision: true }
        }
    });

    console.log(teams);

    ko.applyBindings(viewModel, document.getElementById("team_registration"));

    $("input.division").autocomplete({
        source: divisions,
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
});