﻿$(document).ready(function () {
    $('#createlink').onClickAjaxInto('#create');

    $.subscribe("/create/form/loaded", function (event, form) {
        $(form).onSubmitAjaxInto('#create');
    });
});