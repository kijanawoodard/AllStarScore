$.fn.onClickAjaxInto = function (target) {
    this.click(function (event) {
        event.preventDefault();
        $.ajax({
            url: this.href,
            type: 'get',
            success: function (result) {
                $(target).html(result);
            }
        });
        return false;
    });
};

$.fn.onSubmitAjaxInto = function (target) {
    this.submit(function (event) {
        event.preventDefault();
        if ($(this).valid()) {
            $.ajax({
                url: this.action,
                type: this.method,
                data: $(this).serialize(),
                success: function (result) {
                    $(target).html(result);
                }
            });
        }
        return false;
    });
};

$.fn.pickDateBefore = function (last) {
    var first = this;
    first.datepicker({
        numberOfMonths: 2,
        onSelect: function (selected) {
            $(last).datepicker("option", "minDate", selected);
        }
    });
    $(last).datepicker({
        numberOfMonths: 2,
        onSelect: function (selected) {
            first.datepicker("option", "maxDate", selected);
        }
    });
};