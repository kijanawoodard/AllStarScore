$(document).ready(function () {
    $.subscribe('/competition/created', function (event) {
        $('#competition_list .control_links a').ajaxClick('#competition_list .display');
    });
});