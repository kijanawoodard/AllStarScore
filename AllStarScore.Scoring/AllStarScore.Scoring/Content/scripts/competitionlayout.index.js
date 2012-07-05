$(document).ready(function () {

    var mapping = {
        'copy': ["scoringMap"],
        'info': {
            create: function (options) {
                return new competitionLayoutIndexViewModel(options.data);
            }
        }
    };

    var utilities = {
        asArray: function (obj) {
            var result = [];
            for (var key in obj) {
                result.push({ key: key, value: obj[key] });
            }

            return result;
        },
        asObject: function (array, keyFunc) {
            keyFunc = keyFunc || function (item) {
                return item.id;
            };

            var result = {};
            _.each(array, function (item, index) {
                result[keyFunc(item)] = item;
            });

            return result;
        }
    };

    var competitionLayoutIndexViewModel = function (data) {
        var self = this;
        $.extend(self, data);

        self.divisions = utilities.asObject(data.divisions);
        self.levels = utilities.asObject(data.levels);
        self.registrations = utilities.asObject(data.registrations);
        self.performances = utilities.asObject(data.performances);
    };

    var info = ko.mapping.fromJS(window.competitionLayoutIndexData, mapping);
    _.extend(window.viewModel, info, { utilities: utilities });
});

