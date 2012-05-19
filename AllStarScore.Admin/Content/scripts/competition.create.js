$(document).ready(function () {
    $('#createlink').onClickAjaxInto('#create');
    
    $.subscribe("/create/form/loaded", function (event) {
        $('#create form').onSubmitAjaxInto('#create');
    });
});