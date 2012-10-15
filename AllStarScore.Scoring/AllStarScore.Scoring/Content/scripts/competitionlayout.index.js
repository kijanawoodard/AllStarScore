$(document).ready(function () {
    AllStarScore.CompetitionData = new window.AllStarScore.CompetitionData(Input.CompetitionData);
    AllStarScore.ScoringMap = new AllStarScore.ScoringMap(Input.CompetitionData.scoringMap);
});

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

AllStarScore.CompetitionData = function (data) {
    var self = this;
    var utilities = window.AllStarScore.Utilities;

    self.raw = {};
    //self.raw = data;

    self.raw.levels = data.info.levels;
    self.raw.divisions = data.info.divisions;
    self.raw.judges = data.judgePanel.judges;

    self.company = data.info.company;
    self.competition = data.info.competition;
    self.schedule = data.info.schedule;
    self.levels = utilities.asObject(data.info.levels);
    self.divisions = utilities.asObject(data.info.divisions);
    self.gyms = utilities.asObject(data.info.gyms);
    self.judges = utilities.asObject(data.judgePanel.judges);
    self.panelJudges = _.pluck(_.where(self.raw.judges, { "responsibility": "judges-panel" }), "id");
    self.registrations = utilities.asObject(data.info.registrations);
    self.performances = utilities.asObject(data.performances);
    self.securityContext = data.securityContext;

    _.each(self.performances, function (performance) {
        var division = self.divisions[performance.divisionId];
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

    self.competition.days =
        _.map(self.competition.days, function (day) {
            return new Date(day);
        });

    return self;
};

