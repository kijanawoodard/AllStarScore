var GiftModel = function (gifts) {
    var self = this;
    self.gifts = ko.observableArray(gifts);

    self.addGift = function () {
        self.gifts.push({
            name: "",
            price: ""
        });
    };

    self.removeGift = function (gift) {
        self.gifts.remove(gift);
    };

    self.save = function (form) {
        alert("Could now transmit to server: " + ko.utils.stringifyJson(self.gifts));
        // To actually transmit to server as a regular form post, write this: ko.utils.postJson($("form")[0], self.gifts);
    };
};

var viewModel = new GiftModel([
    { name: "Tall Hat", price: "39.95" },
    { name: "Long Cloak", price: "120.00" }
]);


var TeamRegistrationModel = function (teams) {
    var self = this;
    self.teams = ko.observableArray(teams);

    self.addTeam = function (team) {
        self.teams.push(team);
    };

    self.removeTeam = function (team) {
        self.teams.remove(team);
    };

    self.save = function (form) {
        alert("Could now transmit to server: " + ko.utils.stringifyJson(self.teams));
        // To actually transmit to server as a regular form post, write this: ko.utils.postJson($("form")[0], self.gifts);
    };
};

var viewModel2 = new TeamRegistrationModel(teams);

    $(document).ready(function () {
        // Activate jQuery Validation
        $("form").validate({ submitHandler: function () { viewModel.save(); } });
        console.log(teams);
        ko.applyBindings(viewModel, document.getElementById("foo"));
        ko.applyBindings(viewModel2, document.getElementById("bar"));
    });