$(document).ready(function () {
    $('a.gym_create')
        .onClickAjaxInto('.gym_create .display')
        .click();
});


$.subscribe('/gym/create/form/loaded', function () {

    var form = $('.gym_create .display form');

    form.onSubmitAjaxInto('.gym_create .display');
    $.validator.unobtrusive.parse(form);

    $('#Name').focus();

    $('#Name').autocomplete({
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
    });

    $('#Name').bind('input propertychange', function () {
        checkname($(this).val());
    });

    function checkname(name) {
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
    }
    
    $('.gym_create .display .for_existing')
        .live('click', function (event) {
            event.preventDefault();
            $.publish('/gym/selected', $(this).attr('data-val'));
        });
});

$.subscribe('/gym/name/available', function () {
    $('.gym_create .display').toggleClass('gym_name_taken', false);
});

$.subscribe('/gym/name/taken', function () {
    $('.gym_create .display').toggleClass('gym_name_taken', true);
});
