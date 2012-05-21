$(document).ready(function () {
    $('a.gym_create')
        .onClickAjaxInto('.gym_create .display')
        .click();
});


$.subscribe('/gym/create/form/loaded', function () {

    var form =  $('.gym_create .display form');
    
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
                    }));
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            console.log(ui.item ?
                    "Selected: " + ui.item.value + " aka " + ui.item.id :
                    "Nothing selected, input was " + this.value);
        }
    });

    $('#Name').bind('input propertychange', function () {
        console.log($(this).val());
    });
});
