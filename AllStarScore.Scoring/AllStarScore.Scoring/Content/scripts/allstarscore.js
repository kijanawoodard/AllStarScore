window.AllStarScore = window.AllStarScore || {};
window.Input = window.Input || { };

AllStarScore.Utilities = {
    asArray: function (obj) {
        var result = [];
        for (var key in obj) {
            var node = { key: key, value: obj[key] };
            result.push(node);
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