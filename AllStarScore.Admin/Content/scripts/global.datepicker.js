$.validator.addMethod(
    "greaterThanOrEqualTo",
    function (value, element, params) {
        var target = params.val();
        var isValueNumeric = !isNaN(parseFloat(value)) && isFinite(value);
        var isTargetNumeric = !isNaN(parseFloat(target)) && isFinite(target);
        if (isValueNumeric && isTargetNumeric) {
            return Number(value) >= Number(target);
        }

        if (!/Invalid|NaN/.test(new Date(value))) {
            return new Date(value) >= new Date(target);
        }

        return false;
    },
    'Must be greater than or equal to the start date.');

(function ($) {
    $.fn.pickDateBefore = function (endElement) {
        return this.each(function () {
            var start = $(this);
            var end = start.closest('form').find(endElement);

            end.rules('add', { greaterThanOrEqualTo: start });

            start.datepicker({
                numberOfMonths: 2,
                onSelect: function (selected) {
                    end.datepicker("option", "minDate", selected);
                    start.change();
                    end.change();
                }
            });
            end.datepicker({
                numberOfMonths: 2,
                onSelect: function (selected) {
                    start.datepicker("option", "maxDate", selected);
                    start.change();
                    end.change();
                }
            });
        });
    };
})(jQuery);