var viewModel = {};

$(document).ready(function () {

    var mapping = {
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

        data.divisions = utilities.asObject(data.divisions);
        data.levels = utilities.asObject(data.levels);

        ko.mapping.fromJS(data, mapping, self);
    };

    window.viewModel.lookup = ko.mapping.fromJS(window.competitionLayoutIndexData, mapping);
    window.viewModel.utilities = utilities;
});

