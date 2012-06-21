var infoViewModel;

$(document).ready(function () {

    var mapping = {
        'info': {
            create: function (options) {
                return new competitionLayoutIndexViewModel(options.data);
            }
        },
        'dsivisions': {
            create: function (options) {
                console.log(1);
                var result = {};

                return result;
            }
        }
    };

    var competitionLayoutIndexViewModel = function (data) {
        var self = this;

        self.asArray = function (obj) {
            var result = [];
            for (var key in obj) {
                result.push({ key: key, value: obj[key] });
            }

            return result;
        };

        self.asObject = function (array, keyFunc) {
            keyFunc = keyFunc || function (item) {
                return item.id;
            };

            var result = {};
            _.each(array, function (item, index) {
                result[keyFunc(item)] = item;
            });

            return result;
        };

        data.divisions = self.asObject(data.divisions);
        data.levels = self.asObject(data.levels);
        
        ko.mapping.fromJS(data, mapping, self);
    };

    infoViewModel = ko.mapping.fromJS(window.competitionLayoutIndexData, mapping, window.viewModel);
    ko.applyBindings(infoViewModel, document.getElementById('competitionlayout_index'));
});

