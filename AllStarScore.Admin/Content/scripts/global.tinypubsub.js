﻿/* jQuery Tiny Pub/Sub - v0.7 - 10/27/2011
* http://benalman.com/
* https://gist.github.com/661855
* Copyright (c) 2011 "Cowboy" Ben Alman; Licensed MIT, GPL */
(function ($) {
    var o = $({});
    $.subscribe = function () {
        o.on.apply(o, arguments);
    };

    $.unsubscribe = function () {
        o.off.apply(o, arguments);
    };

    $.publish = function () {
        o.trigger.apply(o, arguments);
        //console.log('publish');
        //console.dir(arguments);
    };
} (jQuery));