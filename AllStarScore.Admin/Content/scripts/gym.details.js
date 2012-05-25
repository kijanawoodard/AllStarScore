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
        $('.gym_details').on('click', 'a.edit', function (event) {
            event.preventDefault();
            $.publish('/gym/edit/requested');
            $('.gym_details').addClass('editing_gym');
        });
    };

    var reset = function() {
        $('.gym_details .control_links a.gym_details_get').click();
    };
    
    var onLoad = function () {
        bindEdit();

        $('.gym_details').removeClass('editing_gym');
    };

    // Return the object that is assigned to Module
    return {
        onLoad: onLoad,
        reset: reset
    };
} ());