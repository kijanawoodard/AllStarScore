$(document).ready(function () {
    $('#Name').autocomplete({
        source: function (request, response) {
            // define a function to call your Action (assuming UserController)
            var url = $('#gym_search').attr('href');
            $.ajax({
                url: url,
                type: "GET",
                dataType: "json",

                // query will be the param used by your action method
                data: { query: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.Name + ' from ' + item.Location, value: item.Name, id: item.Id };
                    }));
                }
            });
        },
        minLength: 1,
        change: function (event, ui) {
            console.log(ui.item ?
					"Selected: " + ui.item.value + " aka " + ui.item.id :
					"Nothing selected, input was " + this.value);
        }
    });
});    