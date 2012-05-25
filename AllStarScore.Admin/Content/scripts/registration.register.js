$.subscribe('/gym/selected', function (event, gymid) {
    var target = $('.team_registration_harness .control_links .form_get');
    var value = target.attr('href').replace('__gymid__', gymid);
    target.attr('href', value);
    target.click();
});
