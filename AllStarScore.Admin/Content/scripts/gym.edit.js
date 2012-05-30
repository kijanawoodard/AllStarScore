$(document).ready(function () {
    GymEditModule.onLoad();
});

//http://www.joezimjs.com/javascript/javascript-closures-and-the-module-pattern/
var GymEditModule = (function () {

    var bindCancel = function () {
        $('.gym_details .display').on('click', 'form.edit a.cancel', function (event) {
            event.preventDefault();
            $.publish('/gym/edit/cancelled');
        });
    };

    var onLoad = function () {
        bindCancel();
        parseForm();
    };

    var parseForm = function () {
        var form = $('.gym_details form.edit');
        //$.validator.unobtrusive.parse(form);
    };

    var publishGymEdited = function (gymid) {
        $.publish('/gym/edit/complete', gymid);
    };

    // Return the object that is assigned to Module
    return {
        onLoad: onLoad,
        publishGymEdited: publishGymEdited,
        parseForm: parseForm
    };
} ());