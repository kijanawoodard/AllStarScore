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

//http://www.aaronkjackson.com/2012/04/formatting-dates-with-knockoutjs/
ko.bindingHandlers.dateString = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor(),
            allBindings = allBindingsAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);
        var pattern = allBindings.datePattern || 'MM/dd/yyyy';
        $(element).text(valueUnwrapped.toString(pattern));
    }
};

//http://stackoverflow.com/a/7986507/214073
//http://jsfiddle.net/QCmJt/32/
ko.bindingHandlers.selectableItem = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var selectable = $(element).parent();

        selectable.bind('selectableselected', function (event, ui) {
            if (ui.selected === element) {
                var value = valueAccessor();

                value(true);
            }
        });

        selectable.bind('selectableunselected', function (event, ui) {
            if (ui.unselected === element) {
                var value = valueAccessor();

                value(false);
            }
        });
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var el = $(element);

        if (valueAccessor()()) {
            el.addClass('ui-selected');
        } else {
            el.removeClass('ui-selected');
        }
    }
};

ko.bindingHandlers.ko_slider = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var slider = $(element);
        var value = valueAccessor().source;
        var boundOptions = valueAccessor().options;
        var options = {
            min: 1,
            max: 100,
            value: value(),
            slide: function (event, ui) {
                value(ui.value);
            }
        };

        $.extend(options, boundOptions);

        slider.slider(options);
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {

    }
};