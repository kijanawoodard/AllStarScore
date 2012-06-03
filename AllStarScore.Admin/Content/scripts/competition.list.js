$.subscribe('/competition/created', function (event) {
    $('#competition_list .control_links a').ajaxClick('#competition_list .display');
});

//$(document).ready(function () {
//    var viewModel = new ListCompetitionViewModel(window.competitionListData);
//    ko.applyBindings(viewModel, document.getElementById('competition_list'));
//});
//
//var ListCompetitionViewModel = (function (data) {
//    var self = this;
//
//    $.each($.makeArray(data.competitions), function (index, competition) {
//        competition.competitionFirstDay = new Date(competition.competitionFirstDay); //TODO: Blog
//    });
//
//    data.competitions.sort(function (a, b) {
//        return a.competitionFirstDay == b.competitionFirstDay ? 0 : a.competitionFirstDay < b.competitionFirstDay ? 1 : -1;
//    });
//
//    self.competitions = ko.mapping.fromJS(data.competitions);
//
//    return self;
//});