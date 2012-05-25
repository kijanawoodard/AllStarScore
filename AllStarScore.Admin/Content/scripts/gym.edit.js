$(document).ready(function () {
    GymEditModule.onLoad();
});

$.subscribe('/gym/edit/requested', function (event, gymid) {
    //$('.gym_edit .control_links a.get').click();
});

//http://www.joezimjs.com/javascript/javascript-closures-and-the-module-pattern/
var GymEditModule = (function () {
    var form = $('#gym_edit_display form');

    var bindCancel = function () {
        form.parent().on('click', 'form a.cancel', function (event) {
            event.preventDefault();
            $.publish('/gym/edit/cancelled');
        });
    };

    var onLoad = function () {
        bindCancel();
        $.validator.unobtrusive.parse(form);
    };

    var publishGymEdited = function (gymid) {
        $.publish('/gym/edit/complete', gymid);
    };

    // Return the object that is assigned to Module
    return {
        onLoad: onLoad,
        publishGymEdited: publishGymEdited
    };
} ());