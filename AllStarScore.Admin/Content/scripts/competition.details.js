$(document).ready(function () {
    $('.register .existing a.cancel').on('click', function (event) {
        event.preventDefault();
        $.publish('/gym/create/cancelled');
    });
});

$.subscribe('/gym/name/taken', function (event, gymid) {
    $('#GymId').val(gymid);
    $('.register').addClass('taken');
});

$.subscribe('/gym/created', function (event, gymid) {
    $('#GymId').val(gymid);
    $('.register .existing form').submit();
});

$.subscribe('/gym/name/available', function () {
    $('.register').removeClass('taken');
});

$.subscribe('/gym/create/cancelled', function () {
    $('.register')
        .removeClass('taken');
});