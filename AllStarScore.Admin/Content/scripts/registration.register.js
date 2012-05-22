$.subscribe('/gym/name/taken', function (event, gymid) {
    console.log(gymid);
    $('.team_registration #GymId').val(gymid);
});
