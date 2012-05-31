(function ($) {
    $.fn.onClickAjaxInto = function (target) {
        return this.each(function() {
            $(this).click(function(event) {
                event.preventDefault();
                $.ajax({
                    url: this.href,
                    type: 'get',
                    success: function(result) {
                        $(target).html(result);
                    }
                });
                return false;
            });
        });
    };
}(jQuery));

(function ($) {
    $.fn.onSubmitAjaxInto = function(target) {
        return this.each(function() {
            $(this).submit(function(event) {
                event.preventDefault();
                if ($(this).valid()) {
                    $.ajax({
                        url: this.action,
                        type: this.method,
                        data: $(this).serialize(),
                        success: function(result) {
                            $(target).html(result);
                        }
                    });
                }
                return false;
            });
        });
    };
}(jQuery));

(function ($) {
    $.fn.ajaxClick = function(target) {
        return this.each(function() {
            var link = this;
            $.ajax({
                url: link.href,
                type: 'get',
                success: function(result) {
                    $(target).html(result);
                }
            });
        });
    };
}(jQuery));

(function ($) {
    $.fn.ajaxPost = function(options) {
        return this.each(function() {
            var form = this;
            $.ajax({
                url: form.action,
                type: form.method,
                data: options.data,
                success: options.success,
                error: function (xhr) {
                    var errors = [{ "Key": "General", "Value": ["An Unknown Error Occurred"]}];
                    if (xhr.status == 400) {
                        errors = $.parseJSON(xhr.responseText).errors;
                    } else if (xhr.status == 302) {
                        alert("redirected");
                        console.log(xhr.responseText);
                    }
                    else {
                        console.log(xhr.responseText);
                    }

                    var summary = $(form).find('.validation-summary-errors');
                    if (summary.length == 0) {
                        console.log('To see errors on page add this within the form: <div class="validation-summary-errors"></div>');
                        console.log(xhr.responseText);
                    }
                    else {
                        summary.empty().append($('<ul>'));
                        $.each(errors, function (x, error) {
                            $.each(error.Value, function (y, value) {
                                summary.find('ul').append(
                                    $('<li>').append(error.Key + ": " + value)
                                );
                            });
                        });
                    }
                }
            });
        });
    };
}(jQuery));