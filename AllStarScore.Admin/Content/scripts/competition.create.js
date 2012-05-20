$(document).ready(function () {
    $('#createlink').onClickAjaxInto('#create');

    $.subscribe("/create/form/loaded", function (event, form) {
        console.log(form);
        $(form).onSubmitAjaxInto('#create');
    });
});