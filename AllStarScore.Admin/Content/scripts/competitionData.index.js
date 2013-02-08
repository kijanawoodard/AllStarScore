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
        if (!division) {
            console.log("Division is null!");
            console.log(performance);
        }
        performance.division = division.name;
        performance.levelId = division.levelId;
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
        
        performance.orderId = performance.id.substr(performance.id.length - 1);
        performance.order = [, '1st', '2nd', '3rd', '4th', '5th'][performance.orderId];
    });

    _.each(self.schedule.days, function (day) {
        day.day = new Date(day.day);
        _.each(day.entries, function (entry) {
            entry.time = new Date(entry.time);
            var ok = !entry.performanceId || self.performances[entry.performanceId];
            if (!ok) {
                entry.remove = true;
            }

            if (ok && entry.performanceId) {
                console.log(entry);
                self.performances[entry.performanceId].time = entry.time;
            }
        });
        day.entries = _.reject(day.entries, function(entry) {
            return entry.remove;
        });
    });

    return self;
};

AllStarScore.ScoringMap = function (data) {
    var self = this;
    _.extend(self, data);

    self.getMaps = function (item, judge) {
        judge = judge || {}; //make sure we have 'something'
        item = item || {}; //make sure we have 'somehing'

        var divisionId = item.divisionId || item.id || item; //supports a division with an id property, anything with a divisionId, or the passed in value
        var division = AllStarScore.CompetitionData.divisions[divisionId] || {}; //now we've got the id; find the level
        var level = AllStarScore.CompetitionData.levels[division.levelId] || {};

        var result = {
            scoreSheet: self.scoreSheets[judge.responsibility] || self.scoreSheets[division.scoringDefinition] || self.scoreSheets[level.scoringDefinition],
            template: self.templates[judge.responsibility] || self.templates[division.scoringDefinition] || self.templates[level.scoringDefinition],
            categories: self.categories[judge.responsibility] || self.categories[division.scoringDefinition] || self.categories[level.scoringDefinition],
            level: level,
            division: division
        };

        return result;
    };

    return self;
};