var TeamRegistrationModel = function (data) {
    var self = this;
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

    self.addTeam = function (team) {
        team.divisionName = self.getDivisionName(team.divisionId);
        console.log(team.divisionName);
        //console.log(ko.toJSON(team));
        self.teams.push(team);
    };

    $.each(self.data.teams, function (index, team) {
        self.addTeam(team);
    });

    self.removeTeam = function (team) {
        self.teams.remove(team);
    };

    self.resetCreateForm = function () {
        $('#team_registration form.create')[0].reset();
        //$('#team_registration form.create .division').focus();
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
                if (result.errors && result.errors.length > 0) {
                    alert(result.errors[0].Value);
                } else {
                    console.log(ko.toJSON(result));
                    self.addTeam(ko.toJS(result));
                }
            },
            error: function (result) {
                alert('unknown error');
            }
        });
    };
};

$(document).ready(function () {
    //format the data for the autocomplete dropdown
    data.divisions = $.map($.makeArray(data.divisions), function (item) {
        return { label: item.level + " - " + item.division, id: item.divisionId };
    });
    
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

    console.log(data.teams);

    ko.applyBindings(viewModel, document.getElementById("team_registration"));

    $("input.division").autocomplete({
        source: data.divisions,
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