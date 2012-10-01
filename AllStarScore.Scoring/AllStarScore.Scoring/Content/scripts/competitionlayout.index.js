$(document).ready(function () {
    AllStarScore.CompetitionData = new window.AllStarScore.CompetitionData.ViewModel(Input.CompetitionData);
    AllStarScore.ScoringMap = Input.CompetitionData.scoringMap;
});

AllStarScore.CompetitionData = {};

AllStarScore.CompetitionData.ViewModel = function (data) {
    var self = this;
    var utilities = window.AllStarScore.Utilities;

    //self.raw = data;

    self.company = data.info.company;
    self.competition = data.info.competition;
    self.schedule = data.info.schedule;
    self.levels = utilities.asObject(data.info.levels);
    self.divisions = utilities.asObject(data.info.divisions);
    self.gyms = utilities.asObject(data.info.gyms);
    self.registrations = utilities.asObject(data.info.registrations);
    self.performances = utilities.asObject(data.performances);

    _.each(self.performances, function (performance) {
        var division = self.divisions[performance.divisionId];
        performance.divisionIdWithoutCompany = division.id.replace(self.company.id + "/", "");
        performance.division = division.name;

        performance.levelId = division.levelId;
        performance.levelIdWithoutCompany = division.levelId.replace(self.company.id + "/", "");
        performance.level = self.levels[division.levelId].name;

        var registration = self.registrations[performance.registrationId];
        performance.team = registration.teamName;
        performance.participants = registration.participantCount;
        performance.isShowTeam = registration.isShowTeam;

        var gym = self.gyms[registration.gymId];
        performance.gym = gym.name;
        performance.isSmallGym = gym.isSmallGym;
        performance.location = gym.location;

        performance.panel = self.schedule.divisionPanels[division.id];
        
        performance.order = [, '1st', '2nd', '3rd', '4th', '5th'][performance.id.substr(performance.id.length - 1)];
    });

    _.each(self.schedule.days, function (day) {
        day.day = new Date(day.day);
        _.each(day.entries, function (entry) {
            entry.time = new Date(entry.time);

            if (entry.performanceId) {
                self.performances[entry.performanceId].time = entry.time;
            }
        });
    });

    return self;
};

