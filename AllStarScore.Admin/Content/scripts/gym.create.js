$(document).ready(function () {
    var viewModel = new CreateGymViewModel(window.gymCreateData);
    ko.applyBindings(viewModel, document.getElementById('gym_create'));

    $.subscribe('/gym/create/cancelled', function () {
        viewModel.cancelCreation();
    });
});

ko.bindingHandlers.autocomplete = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var options = valueAccessor() || {};
        var self = $(element);
        self
            .autocomplete({
                source: options.source,
                minLength: 1,
                select: function (event, ui) {
                    self
                        .val(ui.item.label)
                        .change()
                        .valid();

                    options.onMatchEval({
                        isMatch: true,
                        id: ui.item.id,
                        inputHasValue: true
                    });

                    return false;
                }
            })
            .focus(function () {
                self
                    .autocomplete('search', '') //make the list drop down when you enter the field
                    .select();
            })
            .on('input propertychange', function () {
                
                var result = $.grep(options.source, function (item) {
                    return item.label.toLowerCase() == self.val().toLowerCase()
                        || item.match.toLowerCase() == self.val().toLowerCase();
                });

                options.onMatchEval({
                    isMatch: result.length,
                    id: result.length > 0 ? result[0].id : 0,
                    inputHasValue: self.val().length
                });
            })
            .data("autocomplete")._renderItem = function (ul, item) {
                //http://stackoverflow.com/a/3457773/214073
                var t = item.label.replace(new RegExp("(?![^&;]+;)(?!<[^<>]*)(" + $.ui.autocomplete.escapeRegex(this.term) + ")(?![^<>]*>)(?![^&;]+;)", "gi"), "<strong>$1</strong>");
                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<a>" + t + "</a>")
                    .appendTo(ul);
            };
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var options = valueAccessor() || {};
        $(element).autocomplete("option", "source", options.source);
    }
};

//http://www.joezimjs.com/javascript/javascript-closures-and-the-module-pattern/
var CreateGymViewModel = (function (data) {
    var self = this;
    var hook = $('#gym_create');
    var form = hook.find('form');

    //format the data for the autocomplete dropdown
    self.gyms = $.map($.makeArray(data.gyms), function (item) {
        return { label: item.gymName + " from " + item.location, id: item.gymId, match: item.gymName };
    });

    self.post = {
        gymName: ko.observable(),
        location: ko.observable(),
        isSmallGym: ko.observable()
    };

    self.shouldShowForm = ko.observable(false);
    self.focusName = ko.observable(true);
    self.shouldAllowCreate = ko.observable(true);
    self.nameIsAvailable = ko.observable(false);
    self.nameIsTaken = ko.observable(false);

    self.gymNameMatched = function (result) {
        if (result.isMatch) {
            self.shouldAllowCreate(false);
            self.nameIsAvailable(false);
            self.nameIsTaken(true);
            $.publish('/gym/name/taken', result.id);
        }
        else {
            self.shouldAllowCreate(true);
            self.nameIsAvailable(result.inputHasValue);
            self.nameIsTaken(false);
            $.publish('/gym/name/available');
        }
    };

    self.cancelCreation = function () {
        self.reset();
        self.toggleFormVisibility();
    };

    self.toggleFormVisibility = function () {
        self.shouldShowForm(!self.shouldShowForm());
        self.focusName(self.shouldShowForm());
    };

    self.reset = function () {
        self.post.gymName('');
        self.post.location('');
        self.post.isSmallGym(true);
        form.find('.validation-summary-errors').empty();
        self.focusName(self.shouldShowForm());
        self.shouldAllowCreate(true);
        self.nameIsAvailable(false);
        self.nameIsTaken(false);
    };

    self.create = function () {
        //console.log(ko.toJSON(self.post));
        form.ajaxPost({
            data: ko.toJS(self.post),
            success: function (result) {
                //console.log(ko.toJSON(result));
                var gym = ko.toJS(result);
                $.publish('/gym/created', gym.id);
                self.reset();
            }
        });
    };

    self.setup = function () {
        form.validate({
            debug: true,
            submitHandler: function () {
                self.create(this.currentForm);
            }
        });
    };

    self.reset();
    self.setup();
    return self;
});
//
//$(document).ready(function () {
//    GymModule.onLoad();
//});
//
////http://www.joezimjs.com/javascript/javascript-closures-and-the-module-pattern/
//var GymModule = (function () {
//    var form = $('#gym_create_display form');
//
//    var bindName = function (name) {
//        $('#Name')
//            .focus()
//            .autocomplete({
//                source: function (request, response) {
//                    var url = $('.gym_search').attr('href');
//                    $.ajax({
//                        url: url,
//                        type: "GET",
//                        dataType: "json",
//
//                        data: { query: request.term },
//                        success: function (data) {
//                            response($.map(data, function (item) {
//                                return { label: item.Name + ' from ' + item.Location, value: item.Name, id: item.Id };
//                                //return { label: item, value: item };
//                            }));
//                        }
//                    });
//                },
//                minLength: 1,
//                select: function (event, ui) {
//                    console.log(ui.item ?
//                        "Selected: " + ui.item.value + " aka " + ui.item.id :
//                        "Nothing selected, input was " + this.value);
//                    checkname(ui.item.value);
//                }
//            })
//            .on('input propertychange', function () {
//                checkname($(this).val());
//            });
//    };
//
//    var checkname = function (name) {
//        console.log(name);
//
//        var url = $('.gym_check').attr('href');
//        $.ajax({
//            url: url,
//            type: "GET",
//            dataType: "text",
//
//            data: { name: name },
//            success: function (result) {
//                $('.gym_create .display .availability_check').html(result);
//            }
//        });
//    };
//
//    var bindCancel = function () {
//        $('#gym_create_display a.cancel').on('click', function (event) {
//            event.preventDefault();
//            $.publish('/gym/create/cancelled');
//        });
//
//        $.subscribe('/gym/create/cancelled', function () {
//            form[0].reset();
//            turnOffTaken();
//        });
//    };
//
//    var turnOffTaken = function () {
//        $('.gym_create .display').toggleClass('gym_name_taken', false);
//    };
//    
//    var onLoad = function () {
//        bindName();
//        bindCancel();
//        form[0].reset();
//        //$.validator.unobtrusive.parse(form);
//    };
//
//    var publishGymNameAvailable = function () {
//        turnOffTaken();
//        $.publish('/gym/name/available');
//    };
//
//    var publishGymNameTaken = function (gymid) {
//        $('.gym_create .display').toggleClass('gym_name_taken', true);
//        $.publish('/gym/name/taken', gymid);
//    };
//
//    var publishGymCreated = function (gymid) {
//        $.publish('/gym/created', gymid);
//    };
//
//    var publishGymEdited = function (gymid) {
//        $.publish('/gym/edited', gymid);
//    };
//    
//    // Return the object that is assigned to Module
//    return {
//        onLoad: onLoad,
//        publishGymNameAvailable: publishGymNameAvailable,
//        publishGymNameTaken: publishGymNameTaken,
//        publishGymCreated: publishGymCreated,
//        publishGymEdited: publishGymEdited
//    };
//} ());