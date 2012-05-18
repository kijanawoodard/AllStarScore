var onSuccess = function (el) {
    // enable unobtrusive validation for the contents
    // that was injected into the <div id="create"></div> node
    $.validator.unobtrusive.parse(el.find('form'));
    el.find('.datepicker').datepicker();
};

$(document).ready(function () {
    $('#createlink').click(function (event) {
        event.preventDefault();
        $.ajax({
            url: this.href,
            type: 'get',
            success: function (result) {
                var el = $('#create');
                el.html(result);
                onSuccess(el);
            }
        });
        return false;
    });
});