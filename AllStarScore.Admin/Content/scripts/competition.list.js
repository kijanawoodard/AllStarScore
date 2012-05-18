$(document).ready(function () {
    $('#createlink').ajaxLinkInto('#create', '/create/form/loaded');
    $.subscribe("/form/loaded", validateForm);
    $.subscribe("/create/form/loaded", competitionCreateFirstBeforeLast);

    $('#create a.cancel').live('click', function (event) {
        event.preventDefault();
        $('#create').empty();
    });
});

function competitionCreateFirstBeforeLast(event, target) {
    firstBeforeLast(target, '#FirstDay', '#LastDay');
}

function firstBeforeLast(target, first, last) {
    $(first).datepicker({
        numberOfMonths: 2,
        onSelect: function (selected) {
            $(last).datepicker("option", "minDate", selected);
        }
    });
    $(last).datepicker({
        numberOfMonths: 2,
        onSelect: function (selected) {
            $(first).datepicker("option", "maxDate", selected);
        }
    });
};

function validateForm(event, target) {
    $.validator.unobtrusive.parse($(target).find('form'));
}


$.fn.ajaxLinkInto = function (target, topic) {
    this.click(function (event) {
        event.preventDefault();
        $.ajax({
            url: this.href,
            type: 'get',
            success: function (result) {
                $(target).html(result);
                
                $.publish('/form/loaded', target);
                if (topic)
                    $.publish(topic, target);
            }
        });
        return false;
    });
};