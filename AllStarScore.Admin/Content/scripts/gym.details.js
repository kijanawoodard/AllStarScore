$(document).ready(function () {
    GymDetailsModule.onLoad();
});

$.subscribe('/gym/edit/complete', function (event, gymid) {
    GymDetailsModule.reset();
});

$.subscribe('/gym/edit/cancelled', function () {
    GymDetailsModule.reset();
});

//http://www.joezimjs.com/javascript/javascript-closures-and-the-module-pattern/
var GymDetailsModule = (function () {

    var bindEdit = function () {
        $('.gym_details').on('click', '.display a.edit', function (event) {
            event.preventDefault();
            $('.gym_details .control_links a.edit').click();
        });
    };

    var reset = function () {
        $('.gym_details .control_links a.details').click();
    };

    var onLoad = function () {
        bindEdit();
    };

    // Return the object that is assigned to Module
    return {
        onLoad: onLoad,
        reset: reset
    };
} ());