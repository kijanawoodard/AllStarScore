$(document).ready(function () {
    $('#competition_list_refresh').onClickAjaxInto('#competition_list');

    $.subscribe('/competition/created', function (event) {
        $('#competition_list_refresh').click();
    });
});