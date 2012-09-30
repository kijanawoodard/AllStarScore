AllStarScore.CompetitionData = { };

AllStarScore.CompetitionData.ViewModel = function (data) {
    var self = this;
    var utilities = window.AllStarScore.Utilities;

    self.raw = data;
    self.competition = data.competition;
    self.schedule = data.schedule;
    self.levels = utilities.asObject(data.levels);
    self.divisions = utilities.asObject(data.divisions);
    self.gyms = utilities.asObject(data.gyms);
    self.registrations = utilities.asObject(data.registrations);
    self.performances = utilities.asObject(data.performances);

    _.each(self.performances, function (performance) {
        var division = self.divisions[performance.divisionId];
        performance.division = division.name;
        performance.level = self.levels[division.levelId].name;

        var registration = self.registrations[performance.registrationId];
        performance.team = registration.teamName;
        performance.participants = registration.participantCount;
        performance.isShowTeam = registration.isShowTeam;

        var gym = self.gyms[registration.gymId];
        performance.gym = gym.name;
        performance.isSmallGym = gym.isSmallGym;
        performance.location = gym.location;

        performance.order = [, '1st', '2nd', '3rd', '4th', '5th'][performance.id.substr(performance.id.length - 1)];
    });

    _.each(self.schedule.days, function (day) {
        day.day = new Date(day.day);
        _.each(day.entries, function(entry) {
            entry.time = new Date(entry.time);
        });
    });

    return self;
};