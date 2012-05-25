$(document).ready(function () {
    GymModule.onLoad();
});

//http://www.joezimjs.com/javascript/javascript-closures-and-the-module-pattern/
var GymModule = (function () {
    var form = $('#gym_create_display form');

    var bindName = function (name) {
        $('#Name')
            .focus()
            .autocomplete({
                source: function (request, response) {
                    var url = $('.gym_search').attr('href');
                    $.ajax({
                        url: url,
                        type: "GET",
                        dataType: "json",

                        data: { query: request.term },
                        success: function (data) {
                            response($.map(data, function (item) {
                                return { label: item.Name + ' from ' + item.Location, value: item.Name, id: item.Id };
                                //return { label: item, value: item };
                            }));
                        }
                    });
                },
                minLength: 1,
                select: function (event, ui) {
                    console.log(ui.item ?
                        "Selected: " + ui.item.value + " aka " + ui.item.id :
                        "Nothing selected, input was " + this.value);
                    checkname(ui.item.value);
                }
            })
            .on('input propertychange', function () {
                checkname($(this).val());
            });
    };

    var checkname = function (name) {
        console.log(name);

        var url = $('.gym_check').attr('href');
        $.ajax({
            url: url,
            type: "GET",
            dataType: "text",

            data: { name: name },
            success: function (result) {
                $('.gym_create .display .availability_check').html(result);
            }
        });
    };

    var bindCancel = function () {
        $('#gym_create_display a.cancel').on('click', function (event) {
            event.preventDefault();
            $.publish('/gym/create/cancelled');
        });

        $.subscribe('/gym/create/cancelled', function () {
            form[0].reset();
            turnOffTaken();
        });
    };

    var turnOffTaken = function () {
        $('.gym_create .display').toggleClass('gym_name_taken', false);
    };
    
    var onLoad = function () {
        bindName();
        bindCancel();
        form[0].reset();
        $.validator.unobtrusive.parse(form);
    };

    var publishGymNameAvailable = function () {
        turnOffTaken();
        $.publish('/gym/name/available');
    };

    var publishGymNameTaken = function (gymid) {
        $('.gym_create .display').toggleClass('gym_name_taken', true);
        $.publish('/gym/name/taken', gymid);
    };

    var publishGymCreated = function (gymid) {
        $.publish('/gym/created', gymid);
    };

    var publishGymEdited = function (gymid) {
        $.publish('/gym/edited', gymid);
    };
    
    // Return the object that is assigned to Module
    return {
        onLoad: onLoad,
        publishGymNameAvailable: publishGymNameAvailable,
        publishGymNameTaken: publishGymNameTaken,
        publishGymCreated: publishGymCreated,
        publishGymEdited: publishGymEdited
    };
} ());