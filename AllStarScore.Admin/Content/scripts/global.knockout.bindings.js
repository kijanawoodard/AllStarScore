//http://stackoverflow.com/questions/7855500/autocomplete-with-knockoutjs
ko.bindingHandlers.autocomplete = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var options = valueAccessor() || {};
        var self = $(element);
        self
            .autocomplete({
                source: options.source,
                minLength: 1,
                select: function (event, ui) {
                    self
                        .val(ui.item.label)
                        .change()
                        .valid();

                    options.onMatchEval({
                        isMatch: true,
                        id: ui.item.id,
                        inputHasValue: true
                    });

                    return false;
                }
            })
            .focus(function () {
                self
                    .autocomplete('search', '') //make the list drop down when you enter the field
                    .select();
            })
            .on('input propertychange', function () {

                var result = $.grep(options.source, function (item) {
                    return item.label.toLowerCase() == self.val().toLowerCase()
                        || item.match.toLowerCase() == self.val().toLowerCase();
                });

                options.onMatchEval({
                    isMatch: result.length,
                    id: result.length > 0 ? result[0].id : 0,
                    inputHasValue: self.val().length
                });
            })
            .data("autocomplete")._renderItem = function (ul, item) {
                //http://stackoverflow.com/a/3457773/214073
                var t = item.label.replace(new RegExp("(?![^&;]+;)(?!<[^<>]*)(" + $.ui.autocomplete.escapeRegex(this.term) + ")(?![^<>]*>)(?![^&;]+;)", "gi"), "<strong>$1</strong>");
                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<a>" + t + "</a>")
                    .appendTo(ul);
            };
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var options = valueAccessor() || {};
        $(element).autocomplete("option", "source", options.source);
    }
};