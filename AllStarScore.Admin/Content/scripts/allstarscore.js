window.AllStarScore = {};

AllStarScore.Utilities = {
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
        _.each(array, function (item) {
            result[keyFunc(item)] = item;
        });

        return result;
    }
};