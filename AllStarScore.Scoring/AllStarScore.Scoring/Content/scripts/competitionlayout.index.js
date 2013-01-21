$(document).ready(function () {
    AllStarScore.CompetitionData = new window.AllStarScore.CompetitionData(Input.CompetitionData);
    AllStarScore.ReportOrder = new AllStarScore.ReportOrder(Input.CompetitionData);
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

AllStarScore.ReportOrder = function (data) {
    var self = this;

    self.levels = _.pluck(data.info.competitionDivisions.awardsLevels, 'id');
    self.divisions = _.pluck(data.info.competitionDivisions.divisions, 'id');

//    self.levels = [
//        "awards/level/individual",
//        "awards/level/hiphopprep",
//        "awards/level/danceprep",
//        "awards/level/crew",
//        "awards/level/hiphop",
//        "awards/level/dance",
//        "awards/level/school",
//        "awards/level/highschool",
//        "awards/level/cheerprep",
//        "awards/level/clubcheernovice",
//        "awards/level/clubcheerintermediate",
//        "awards/level/clubcheeradvanced",
//        "awards/level/gamerec",
//        "awards/level/1",
//        "awards/level/2",
//        "awards/level/3",
//        "awards/level/4",
//        "awards/level/5",
//        "awards/level/6",
//        "awards/level/specialneeds"
//    ];
//
//    self.divisions = [
//        "company/1/divisions/level/hiphopprep/division/tiny",
//        "company/1/divisions/level/hiphopprep/division/tiny-coed",
//        "company/1/divisions/level/hiphopprep/division/mini",
//        "company/1/divisions/level/hiphopprep/division/mini-coed",
//        "company/1/divisions/level/hiphopprep/division/youth",
//        "company/1/divisions/level/hiphopprep/division/youth-coed",
//        "company/1/divisions/level/hiphopprep/division/junior",
//        "company/1/divisions/level/hiphopprep/division/junior-coed",
//        "company/1/divisions/level/danceprep/division/tiny",
//        "company/1/divisions/level/danceprep/division/tiny-coed",
//        "company/1/divisions/level/danceprep/division/mini",
//        "company/1/divisions/level/danceprep/division/mini-coed",
//        "company/1/divisions/level/danceprep/division/youth",
//        "company/1/divisions/level/danceprep/division/youth-coed",
//        "company/1/divisions/level/danceprep/division/junior",
//        "company/1/divisions/level/danceprep/division/junior-coed",
//        "company/1/divisions/level/crew/division/tiny",
//        "company/1/divisions/level/crew/division/tiny-coed",
//        "company/1/divisions/level/crew/division/mini",
//        "company/1/divisions/level/crew/division/mini-coed",
//        "company/1/divisions/level/crew/division/youth",
//        "company/1/divisions/level/crew/division/youth-coed",
//        "company/1/divisions/level/crew/division/junior",
//        "company/1/divisions/level/crew/division/junior-coed",
//        "company/1/divisions/level/crew/division/senior",
//        "company/1/divisions/level/crew/division/senior-coed",
//        "company/1/divisions/level/crew/division/open",
//        "company/1/divisions/level/crew/division/open-coed",
//        "company/1/divisions/level/hiphop/division/tiny",
//        "company/1/divisions/level/hiphop/division/tiny-coed",
//        "company/1/divisions/level/hiphop/division/mini",
//        "company/1/divisions/level/hiphop/division/mini-coed",
//        "company/1/divisions/level/hiphop/division/youth",
//        "company/1/divisions/level/hiphop/division/youth-coed",
//        "company/1/divisions/level/hiphop/division/junior",
//        "company/1/divisions/level/hiphop/division/junior-coed",
//        "company/1/divisions/level/hiphop/division/senior",
//        "company/1/divisions/level/hiphop/division/senior-coed",
//        "company/1/divisions/level/hiphop/division/open",
//        "company/1/divisions/level/hiphop/division/open-coed",
//        "company/1/divisions/level/dance/division/tiny",
//        "company/1/divisions/level/dance/division/tiny-coed",
//        "company/1/divisions/level/dance/division/mini",
//        "company/1/divisions/level/dance/division/mini-coed",
//        "company/1/divisions/level/dance/division/youth",
//        "company/1/divisions/level/dance/division/youth-coed",
//        "company/1/divisions/level/dance/division/junior",
//        "company/1/divisions/level/dance/division/junior-coed",
//        "company/1/divisions/level/dance/division/senior",
//        "company/1/divisions/level/dance/division/senior-coed",
//        "company/1/divisions/level/dance/division/open",
//        "company/1/divisions/level/dance/division/open-coed",
//        "company/1/divisions/level/school/division/elementary",
//        "company/1/divisions/level/school/division/junior-high",
//        "company/1/divisions/level/highschool/division/novice",
//        "company/1/divisions/level/highschool/division/intermediate",
//        "company/1/divisions/level/highschool/division/advanced",
//        "company/1/divisions/level/cheerprep/division/tiny",
//        "company/1/divisions/level/cheerprep/division/tiny-small",
//        "company/1/divisions/level/cheerprep/division/tiny-large",
//        "company/1/divisions/level/cheerprep/division/mini",
//        "company/1/divisions/level/cheerprep/division/mini-small",
//        "company/1/divisions/level/cheerprep/division/mini-large",
//        "company/1/divisions/level/cheerprep/division/youth-level-1",
//        "company/1/divisions/level/cheerprep/division/youth-level-2",
//        "company/1/divisions/level/cheerprep/division/youth-level-3",
//        "company/1/divisions/level/cheerprep/division/junior-level-1",
//        "company/1/divisions/level/cheerprep/division/junior-level-2",
//        "company/1/divisions/level/cheerprep/division/junior-level-3",
//        "company/1/divisions/level/cheerprep/division/senior-level-1",
//        "company/1/divisions/level/cheerprep/division/senior-level-2",
//        "company/1/divisions/level/cheerprep/division/senior-level-3",
//        "company/1/divisions/level/clubcheernovice/division/tiny",
//        "company/1/divisions/level/clubcheernovice/division/mini",
//        "company/1/divisions/level/clubcheernovice/division/youth",
//        "company/1/divisions/level/clubcheernovice/division/youth-small",
//        "company/1/divisions/level/clubcheernovice/division/youth-large",
//        "company/1/divisions/level/clubcheernovice/division/junior",
//        "company/1/divisions/level/clubcheernovice/division/junior-small",
//        "company/1/divisions/level/clubcheernovice/division/junior-large",
//        "company/1/divisions/level/clubcheernovice/division/senior",
//        "company/1/divisions/level/clubcheerintermediate/division/tiny",
//        "company/1/divisions/level/clubcheerintermediate/division/mini",
//        "company/1/divisions/level/clubcheerintermediate/division/youth",
//        "company/1/divisions/level/clubcheerintermediate/division/junior",
//        "company/1/divisions/level/clubcheerintermediate/division/senior",
//        "company/1/divisions/level/clubcheeradvanced/division/tiny",
//        "company/1/divisions/level/clubcheeradvanced/division/mini",
//        "company/1/divisions/level/clubcheeradvanced/division/youth",
//        "company/1/divisions/level/clubcheeradvanced/division/junior",
//        "company/1/divisions/level/clubcheeradvanced/division/senior",
//        "company/1/divisions/level/gamerecnovice/division/tiny",
//        "company/1/divisions/level/gamerecnovice/division/mini",
//        "company/1/divisions/level/gamerecnovice/division/youth",
//        "company/1/divisions/level/gamerecnovice/division/junior",
//        "company/1/divisions/level/gamerecnovice/division/senior",
//        "company/1/divisions/level/gamerecintermediate/division/tiny",
//        "company/1/divisions/level/gamerecintermediate/division/mini",
//        "company/1/divisions/level/gamerecintermediate/division/youth",
//        "company/1/divisions/level/gamerecintermediate/division/junior",
//        "company/1/divisions/level/gamerecintermediate/division/senior",
//        "company/1/divisions/level/gamerecadvanced/division/tiny",
//        "company/1/divisions/level/gamerecadvanced/division/mini",
//        "company/1/divisions/level/gamerecadvanced/division/youth",
//        "company/1/divisions/level/gamerecadvanced/division/junior",
//        "company/1/divisions/level/gamerecadvanced/division/senior",
//        "company/1/divisions/level/1/division/tiny",
//        "company/1/divisions/level/1/division/tiny-small",
//        "company/1/divisions/level/1/division/tiny-large",
//        "company/1/divisions/level/1/division/mini",
//        "company/1/divisions/level/1/division/mini-small",
//        "company/1/divisions/level/1/division/mini-large",
//        "company/1/divisions/level/1/division/youth",
//        "company/1/divisions/level/1/division/youth-small",
//        "company/1/divisions/level/1/division/youth-large",
//        "company/1/divisions/level/1/division/junior",
//        "company/1/divisions/level/1/division/junior-small",
//        "company/1/divisions/level/1/division/junior-large",
//        "company/1/divisions/level/1/division/senior",
//        "company/1/divisions/level/1/division/senior-small",
//        "company/1/divisions/level/1/division/senior-large",
//        "company/1/divisions/level/2/division/mini",
//        "company/1/divisions/level/2/division/mini-small",
//        "company/1/divisions/level/2/division/mini-large",
//        "company/1/divisions/level/2/division/youth",
//        "company/1/divisions/level/2/division/youth-small",
//        "company/1/divisions/level/2/division/youth-large",
//        "company/1/divisions/level/2/division/junior",
//        "company/1/divisions/level/2/division/junior-small",
//        "company/1/divisions/level/2/division/junior-large",
//        "company/1/divisions/level/2/division/senior",
//        "company/1/divisions/level/2/division/senior-small",
//        "company/1/divisions/level/2/division/senior-large",
//        "company/1/divisions/level/3/division/youth",
//        "company/1/divisions/level/3/division/youth-small",
//        "company/1/divisions/level/3/division/youth-large",
//        "company/1/divisions/level/3/division/junior",
//        "company/1/divisions/level/3/division/junior-small",
//        "company/1/divisions/level/3/division/junior-large",
//        "company/1/divisions/level/3/division/senior",
//        "company/1/divisions/level/3/division/senior-coed",
//        "company/1/divisions/level/3/division/senior-coed-small",
//        "company/1/divisions/level/3/division/senior-coed-large",
//        "company/1/divisions/level/3/division/senior-small",
//        "company/1/divisions/level/3/division/senior-large",
//        "company/1/divisions/level/3/division/open",
//        "company/1/divisions/level/3/division/open-small",
//        "company/1/divisions/level/3/division/open-large",
//        "company/1/divisions/level/4/division/youth",
//        "company/1/divisions/level/4/division/youth-small",
//        "company/1/divisions/level/4/division/youth-large",
//        "company/1/divisions/level/4/division/junior",
//        "company/1/divisions/level/4/division/junior-small",
//        "company/1/divisions/level/4/division/junior-large",
//        "company/1/divisions/level/4/division/senior",
//        "company/1/divisions/level/4/division/senior-4-2",
//        "company/1/divisions/level/4/division/senior-4-2-large",
//        "company/1/divisions/level/4/division/senior-4-2-small",
//        "company/1/divisions/level/4/division/senior-coed",
//        "company/1/divisions/level/4/division/senior-coed-small",
//        "company/1/divisions/level/4/division/senior-coed-large",
//        "company/1/divisions/level/4/division/senior-small",
//        "company/1/divisions/level/4/division/senior-large",
//        "company/1/divisions/level/4/division/open",
//        "company/1/divisions/level/4/division/open-small",
//        "company/1/divisions/level/4/division/open-large",
//        "company/1/divisions/level/5/division/worlds",
//        "company/1/divisions/level/5/division/youth",
//        "company/1/divisions/level/5/division/youth-small",
//        "company/1/divisions/level/5/division/youth-large",
//        "company/1/divisions/level/5/division/junior",
//        "company/1/divisions/level/5/division/junior-small",
//        "company/1/divisions/level/5/division/junior-large",
//        "company/1/divisions/level/5/division/senior-restricted",
//        "company/1/divisions/level/5/division/senior-restricted-small",
//        "company/1/divisions/level/5/division/senior-restricted-large",
//        "company/1/divisions/level/5/division/senior",
//        "company/1/divisions/level/5/division/senior-small",
//        "company/1/divisions/level/5/division/senior-medium",
//        "company/1/divisions/level/5/division/senior-large",
//        "company/1/divisions/level/5/division/senior-coed",
//        "company/1/divisions/level/5/division/senior-coed-small",
//        "company/1/divisions/level/5/division/senior-coed-medium",
//        "company/1/divisions/level/5/division/senior-coed-large",
//        "company/1/divisions/level/5/division/international-open",
//        "company/1/divisions/level/5/division/international-open-coed",
//        "company/1/divisions/level/5/division/open-coed",
//        "company/1/divisions/level/6/division/open",
//        "company/1/divisions/level/6/division/open-coed",
//        "company/1/divisions/level/specialneeds/division/special-needs"
//    ];
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
                //console.log(entry);
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

