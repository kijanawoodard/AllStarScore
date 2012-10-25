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
    self.levels = [
        "company/1/level/individual",
        "company/1/level/danceprep",
        "company/1/level/dance",
        "company/1/level/school",
        "company/1/level/cheerprep",
        "company/1/level/clubcheerbeginner",
        "company/1/level/clubcheerintermediate",
        "company/1/level/clubcheeradvanced",
        "company/1/level/gamerecbeginner",
        "company/1/level/gamerecintermediate",
        "company/1/level/gamerecadvanced",
        "company/1/level/1",
        "company/1/level/2",
        "company/1/level/3",
        "company/1/level/4",
        "company/1/level/5",
        "company/1/level/6",
        "company/1/level/specialneeds"
    ];

    self.divisions = [
        "company/1/divisions/level/danceprep/division/ballet_tiny",
        "company/1/divisions/level/danceprep/division/ballet_tiny_coed",
        "company/1/divisions/level/danceprep/division/ballet_mini",
        "company/1/divisions/level/danceprep/division/ballet_mini_coed",
        "company/1/divisions/level/danceprep/division/ballet_youth",
        "company/1/divisions/level/danceprep/division/ballet_youth_coed",
        "company/1/divisions/level/danceprep/division/ballet_junior",
        "company/1/divisions/level/danceprep/division/ballet_junior_coed",
        "company/1/divisions/level/danceprep/division/crew_tiny",
        "company/1/divisions/level/danceprep/division/crew_tiny_coed",
        "company/1/divisions/level/danceprep/division/crew_mini",
        "company/1/divisions/level/danceprep/division/crew_mini_coed",
        "company/1/divisions/level/danceprep/division/crew_youth",
        "company/1/divisions/level/danceprep/division/crew_youth_coed",
        "company/1/divisions/level/danceprep/division/crew_junior",
        "company/1/divisions/level/danceprep/division/crew_junior_coed",
        "company/1/divisions/level/danceprep/division/hip_hop_tiny",
        "company/1/divisions/level/danceprep/division/hip_hop_tiny_coed",
        "company/1/divisions/level/danceprep/division/hip_hop_mini",
        "company/1/divisions/level/danceprep/division/hip_hop_mini_coed",
        "company/1/divisions/level/danceprep/division/hip_hop_youth",
        "company/1/divisions/level/danceprep/division/hip_hop_youth_coed",
        "company/1/divisions/level/danceprep/division/hip_hop_junior",
        "company/1/divisions/level/danceprep/division/hip_hop_junior_coed",
        "company/1/divisions/level/danceprep/division/jazz_tiny",
        "company/1/divisions/level/danceprep/division/jazz_tiny_coed",
        "company/1/divisions/level/danceprep/division/jazz_mini",
        "company/1/divisions/level/danceprep/division/jazz_mini_coed",
        "company/1/divisions/level/danceprep/division/jazz_youth",
        "company/1/divisions/level/danceprep/division/jazz_youth_coed",
        "company/1/divisions/level/danceprep/division/jazz_junior",
        "company/1/divisions/level/danceprep/division/jazz_junior_coed",
        "company/1/divisions/level/danceprep/division/lyrical_tiny",
        "company/1/divisions/level/danceprep/division/lyrical_tiny_coed",
        "company/1/divisions/level/danceprep/division/lyrical_mini",
        "company/1/divisions/level/danceprep/division/lyrical_mini_coed",
        "company/1/divisions/level/danceprep/division/lyrical_youth",
        "company/1/divisions/level/danceprep/division/lyrical_youth_coed",
        "company/1/divisions/level/danceprep/division/lyrical_junior",
        "company/1/divisions/level/danceprep/division/lyrical_junior_coed",
        "company/1/divisions/level/danceprep/division/novelty_tiny",
        "company/1/divisions/level/danceprep/division/novelty_tiny_coed",
        "company/1/divisions/level/danceprep/division/novelty_mini",
        "company/1/divisions/level/danceprep/division/novelty_mini_coed",
        "company/1/divisions/level/danceprep/division/novelty_youth",
        "company/1/divisions/level/danceprep/division/novelty_youth_coed",
        "company/1/divisions/level/danceprep/division/novelty_junior",
        "company/1/divisions/level/danceprep/division/novelty_junior_coed",
        "company/1/divisions/level/danceprep/division/open_tiny",
        "company/1/divisions/level/danceprep/division/open_tiny_coed",
        "company/1/divisions/level/danceprep/division/open_mini",
        "company/1/divisions/level/danceprep/division/open_mini_coed",
        "company/1/divisions/level/danceprep/division/open_youth",
        "company/1/divisions/level/danceprep/division/open_youth_coed",
        "company/1/divisions/level/danceprep/division/open_junior",
        "company/1/divisions/level/danceprep/division/open_junior_coed",
        "company/1/divisions/level/danceprep/division/pom_tiny",
        "company/1/divisions/level/danceprep/division/pom_tiny_coed",
        "company/1/divisions/level/danceprep/division/pom_mini",
        "company/1/divisions/level/danceprep/division/pom_mini_coed",
        "company/1/divisions/level/danceprep/division/pom_youth",
        "company/1/divisions/level/danceprep/division/pom_youth_coed",
        "company/1/divisions/level/danceprep/division/pom_junior",
        "company/1/divisions/level/danceprep/division/pom_junior_coed",
        "company/1/divisions/level/danceprep/division/prop_tiny",
        "company/1/divisions/level/danceprep/division/prop_tiny_coed",
        "company/1/divisions/level/danceprep/division/prop_mini",
        "company/1/divisions/level/danceprep/division/prop_mini_coed",
        "company/1/divisions/level/danceprep/division/prop_youth",
        "company/1/divisions/level/danceprep/division/prop_youth_coed",
        "company/1/divisions/level/danceprep/division/prop_junior",
        "company/1/divisions/level/danceprep/division/prop_junior_coed",
        "company/1/divisions/level/dance/division/ballet_tiny",
        "company/1/divisions/level/dance/division/ballet_tiny_coed",
        "company/1/divisions/level/dance/division/ballet_mini",
        "company/1/divisions/level/dance/division/ballet_mini_coed",
        "company/1/divisions/level/dance/division/ballet_youth",
        "company/1/divisions/level/dance/division/ballet_youth_coed",
        "company/1/divisions/level/dance/division/ballet_junior",
        "company/1/divisions/level/dance/division/ballet_junior_coed",
        "company/1/divisions/level/dance/division/ballet_senior",
        "company/1/divisions/level/dance/division/ballet_senior_coed",
        "company/1/divisions/level/dance/division/ballet_open",
        "company/1/divisions/level/dance/division/ballet_open_coed",
        "company/1/divisions/level/dance/division/crew_tiny",
        "company/1/divisions/level/dance/division/crew_tiny_coed",
        "company/1/divisions/level/dance/division/crew_mini",
        "company/1/divisions/level/dance/division/crew_mini_coed",
        "company/1/divisions/level/dance/division/crew_youth",
        "company/1/divisions/level/dance/division/crew_youth_coed",
        "company/1/divisions/level/dance/division/crew_junior",
        "company/1/divisions/level/dance/division/crew_junior_coed",
        "company/1/divisions/level/dance/division/crew_senior",
        "company/1/divisions/level/dance/division/crew_senior_coed",
        "company/1/divisions/level/dance/division/crew_open",
        "company/1/divisions/level/dance/division/crew_open_coed",
        "company/1/divisions/level/dance/division/hip_hop_tiny",
        "company/1/divisions/level/dance/division/hip_hop_tiny_coed",
        "company/1/divisions/level/dance/division/hip_hop_mini",
        "company/1/divisions/level/dance/division/hip_hop_mini_coed",
        "company/1/divisions/level/dance/division/hip_hop_youth",
        "company/1/divisions/level/dance/division/hip_hop_youth_coed",
        "company/1/divisions/level/dance/division/hip_hop_junior",
        "company/1/divisions/level/dance/division/hip_hop_junior_coed",
        "company/1/divisions/level/dance/division/hip_hop_senior",
        "company/1/divisions/level/dance/division/hip_hop_senior_coed",
        "company/1/divisions/level/dance/division/hip_hop_open",
        "company/1/divisions/level/dance/division/hip_hop_open_coed",
        "company/1/divisions/level/dance/division/jazz_tiny",
        "company/1/divisions/level/dance/division/jazz_tiny_coed",
        "company/1/divisions/level/dance/division/jazz_mini",
        "company/1/divisions/level/dance/division/jazz_mini_coed",
        "company/1/divisions/level/dance/division/jazz_youth",
        "company/1/divisions/level/dance/division/jazz_youth_coed",
        "company/1/divisions/level/dance/division/jazz_junior",
        "company/1/divisions/level/dance/division/jazz_junior_coed",
        "company/1/divisions/level/dance/division/jazz_senior",
        "company/1/divisions/level/dance/division/jazz_senior_coed",
        "company/1/divisions/level/dance/division/jazz_open",
        "company/1/divisions/level/dance/division/jazz_open_coed",
        "company/1/divisions/level/dance/division/lyrical_tiny",
        "company/1/divisions/level/dance/division/lyrical_tiny_coed",
        "company/1/divisions/level/dance/division/lyrical_mini",
        "company/1/divisions/level/dance/division/lyrical_mini_coed",
        "company/1/divisions/level/dance/division/lyrical_youth",
        "company/1/divisions/level/dance/division/lyrical_youth_coed",
        "company/1/divisions/level/dance/division/lyrical_junior",
        "company/1/divisions/level/dance/division/lyrical_junior_coed",
        "company/1/divisions/level/dance/division/lyrical_senior",
        "company/1/divisions/level/dance/division/lyrical_senior_coed",
        "company/1/divisions/level/dance/division/lyrical_open",
        "company/1/divisions/level/dance/division/lyrical_open_coed",
        "company/1/divisions/level/dance/division/novelty_tiny",
        "company/1/divisions/level/dance/division/novelty_tiny_coed",
        "company/1/divisions/level/dance/division/novelty_mini",
        "company/1/divisions/level/dance/division/novelty_mini_coed",
        "company/1/divisions/level/dance/division/novelty_youth",
        "company/1/divisions/level/dance/division/novelty_youth_coed",
        "company/1/divisions/level/dance/division/novelty_junior",
        "company/1/divisions/level/dance/division/novelty_junior_coed",
        "company/1/divisions/level/dance/division/novelty_senior",
        "company/1/divisions/level/dance/division/novelty_senior_coed",
        "company/1/divisions/level/dance/division/novelty_open",
        "company/1/divisions/level/dance/division/novelty_open_coed",
        "company/1/divisions/level/dance/division/open_tiny",
        "company/1/divisions/level/dance/division/open_tiny_coed",
        "company/1/divisions/level/dance/division/open_mini",
        "company/1/divisions/level/dance/division/open_mini_coed",
        "company/1/divisions/level/dance/division/open_youth",
        "company/1/divisions/level/dance/division/open_youth_coed",
        "company/1/divisions/level/dance/division/open_junior",
        "company/1/divisions/level/dance/division/open_junior_coed",
        "company/1/divisions/level/dance/division/open_senior",
        "company/1/divisions/level/dance/division/open_senior_coed",
        "company/1/divisions/level/dance/division/open_open",
        "company/1/divisions/level/dance/division/open_open_coed",
        "company/1/divisions/level/dance/division/pom_tiny",
        "company/1/divisions/level/dance/division/pom_tiny_coed",
        "company/1/divisions/level/dance/division/pom_mini",
        "company/1/divisions/level/dance/division/pom_mini_coed",
        "company/1/divisions/level/dance/division/pom_youth",
        "company/1/divisions/level/dance/division/pom_youth_coed",
        "company/1/divisions/level/dance/division/pom_junior",
        "company/1/divisions/level/dance/division/pom_junior_coed",
        "company/1/divisions/level/dance/division/pom_senior",
        "company/1/divisions/level/dance/division/pom_senior_coed",
        "company/1/divisions/level/dance/division/pom_open",
        "company/1/divisions/level/dance/division/pom_open_coed",
        "company/1/divisions/level/dance/division/prop_tiny",
        "company/1/divisions/level/dance/division/prop_tiny_coed",
        "company/1/divisions/level/dance/division/prop_mini",
        "company/1/divisions/level/dance/division/prop_mini_coed",
        "company/1/divisions/level/dance/division/prop_youth",
        "company/1/divisions/level/dance/division/prop_youth_coed",
        "company/1/divisions/level/dance/division/prop_junior",
        "company/1/divisions/level/dance/division/prop_junior_coed",
        "company/1/divisions/level/dance/division/prop_senior",
        "company/1/divisions/level/dance/division/prop_senior_coed",
        "company/1/divisions/level/dance/division/prop_open",
        "company/1/divisions/level/dance/division/prop_open_coed",
        "company/1/divisions/level/school/division/elementary_school",
        "company/1/divisions/level/school/division/junior_high",
        "company/1/divisions/level/school/division/high_school_novice",
        "company/1/divisions/level/school/division/high_school_intermediate",
        "company/1/divisions/level/school/division/high_school_advanced",
        "company/1/divisions/level/cheerprep/division/tiny_level_1",
        "company/1/divisions/level/cheerprep/division/tiny_level_2",
        "company/1/divisions/level/cheerprep/division/tiny_level_3",
        "company/1/divisions/level/cheerprep/division/mini_level_1",
        "company/1/divisions/level/cheerprep/division/mini_level_2",
        "company/1/divisions/level/cheerprep/division/mini_level_3",
        "company/1/divisions/level/cheerprep/division/youth_level_1",
        "company/1/divisions/level/cheerprep/division/youth_level_2",
        "company/1/divisions/level/cheerprep/division/youth_level_3",
        "company/1/divisions/level/cheerprep/division/junior_level_1",
        "company/1/divisions/level/cheerprep/division/junior_level_2",
        "company/1/divisions/level/cheerprep/division/junior_level_3",
        "company/1/divisions/level/cheerprep/division/senior_level_1",
        "company/1/divisions/level/cheerprep/division/senior_level_2",
        "company/1/divisions/level/cheerprep/division/senior_level_3",
        "company/1/divisions/level/clubcheerbeginner/division/tiny",
        "company/1/divisions/level/clubcheerbeginner/division/mini",
        "company/1/divisions/level/clubcheerbeginner/division/youth",
        "company/1/divisions/level/clubcheerbeginner/division/junior",
        "company/1/divisions/level/clubcheerbeginner/division/senior",
        "company/1/divisions/level/clubcheerintermediate/division/tiny",
        "company/1/divisions/level/clubcheerintermediate/division/mini",
        "company/1/divisions/level/clubcheerintermediate/division/youth",
        "company/1/divisions/level/clubcheerintermediate/division/junior",
        "company/1/divisions/level/clubcheerintermediate/division/senior",
        "company/1/divisions/level/clubcheeradvanced/division/tiny",
        "company/1/divisions/level/clubcheeradvanced/division/mini",
        "company/1/divisions/level/clubcheeradvanced/division/youth",
        "company/1/divisions/level/clubcheeradvanced/division/junior",
        "company/1/divisions/level/clubcheeradvanced/division/senior",
        "company/1/divisions/level/gamerecbeginner/division/tiny",
        "company/1/divisions/level/gamerecbeginner/division/mini",
        "company/1/divisions/level/gamerecbeginner/division/youth",
        "company/1/divisions/level/gamerecbeginner/division/junior",
        "company/1/divisions/level/gamerecbeginner/division/senior",
        "company/1/divisions/level/gamerecintermediate/division/tiny",
        "company/1/divisions/level/gamerecintermediate/division/mini",
        "company/1/divisions/level/gamerecintermediate/division/youth",
        "company/1/divisions/level/gamerecintermediate/division/junior",
        "company/1/divisions/level/gamerecintermediate/division/senior",
        "company/1/divisions/level/gamerecadvanced/division/tiny",
        "company/1/divisions/level/gamerecadvanced/division/mini",
        "company/1/divisions/level/gamerecadvanced/division/youth",
        "company/1/divisions/level/gamerecadvanced/division/junior",
        "company/1/divisions/level/gamerecadvanced/division/senior",
        "company/1/divisions/level/1/division/tiny",
        "company/1/divisions/level/1/division/tiny_large",
        "company/1/divisions/level/1/division/tiny_small",
        "company/1/divisions/level/1/division/mini",
        "company/1/divisions/level/1/division/mini_large",
        "company/1/divisions/level/1/division/mini_small",
        "company/1/divisions/level/1/division/youth",
        "company/1/divisions/level/1/division/youth_large",
        "company/1/divisions/level/1/division/youth_small",
        "company/1/divisions/level/1/division/junior",
        "company/1/divisions/level/1/division/junior_large",
        "company/1/divisions/level/1/division/junior_small",
        "company/1/divisions/level/1/division/senior",
        "company/1/divisions/level/1/division/senior_large",
        "company/1/divisions/level/1/division/senior_small",
        "company/1/divisions/level/2/division/mini",
        "company/1/divisions/level/2/division/mini_large",
        "company/1/divisions/level/2/division/mini_small",
        "company/1/divisions/level/2/division/youth",
        "company/1/divisions/level/2/division/youth_large",
        "company/1/divisions/level/2/division/youth_small",
        "company/1/divisions/level/2/division/junior",
        "company/1/divisions/level/2/division/junior_large",
        "company/1/divisions/level/2/division/junior_small",
        "company/1/divisions/level/2/division/senior",
        "company/1/divisions/level/2/division/senior_large",
        "company/1/divisions/level/2/division/senior_small",
        "company/1/divisions/level/3/division/youth",
        "company/1/divisions/level/3/division/youth_large",
        "company/1/divisions/level/3/division/youth_small",
        "company/1/divisions/level/3/division/junior",
        "company/1/divisions/level/3/division/junior_large",
        "company/1/divisions/level/3/division/junior_small",
        "company/1/divisions/level/3/division/senior",
        "company/1/divisions/level/3/division/senior_coed",
        "company/1/divisions/level/3/division/senior_coed_large",
        "company/1/divisions/level/3/division/senior_coed_small",
        "company/1/divisions/level/3/division/senior_large",
        "company/1/divisions/level/3/division/senior_small",
        "company/1/divisions/level/3/division/open",
        "company/1/divisions/level/3/division/open_large",
        "company/1/divisions/level/3/division/open_small",
        "company/1/divisions/level/4/division/youth",
        "company/1/divisions/level/4/division/youth_large",
        "company/1/divisions/level/4/division/youth_small",
        "company/1/divisions/level/4/division/junior",
        "company/1/divisions/level/4/division/junior_large",
        "company/1/divisions/level/4/division/junior_small",
        "company/1/divisions/level/4/division/senior",
        "company/1/divisions/level/4/division/senior_4.2",
        "company/1/divisions/level/4/division/senior_4.2_large",
        "company/1/divisions/level/4/division/senior_4.2_small",
        "company/1/divisions/level/4/division/senior_coed",
        "company/1/divisions/level/4/division/senior_coed_large",
        "company/1/divisions/level/4/division/senior_coed_small",
        "company/1/divisions/level/4/division/senior_large",
        "company/1/divisions/level/4/division/senior_small",
        "company/1/divisions/level/4/division/open",
        "company/1/divisions/level/4/division/open_large",
        "company/1/divisions/level/4/division/open_small",
        "company/1/divisions/level/5/division/worlds",
        "company/1/divisions/level/5/division/youth",
        "company/1/divisions/level/5/division/youth_large",
        "company/1/divisions/level/5/division/youth_small",
        "company/1/divisions/level/5/division/junior",
        "company/1/divisions/level/5/division/junior_large",
        "company/1/divisions/level/5/division/junior_small",
        "company/1/divisions/level/5/division/senior",
        "company/1/divisions/level/5/division/senior_coed",
        "company/1/divisions/level/5/division/senior_coed_small",
        "company/1/divisions/level/5/division/senior_coed_medium",
        "company/1/divisions/level/5/division/senior_coed_large",
        "company/1/divisions/level/5/division/senior_large",
        "company/1/divisions/level/5/division/senior_restricted",
        "company/1/divisions/level/5/division/senior_small",
        "company/1/divisions/level/5/division/international_open",
        "company/1/divisions/level/5/division/international_open_coed",
        "company/1/divisions/level/6/division/open",
        "company/1/divisions/level/6/division/open_coed",
        "company/1/divisions/level/specialneeds/division/special_needs"
    ];
    //
    //    
    //    self.go = _.sortBy(self.divisions, function (id) {
    //        for (var i = 0; i < list.length; i++) {
    //            if (id.indexOf(list[i]) != -1) {
    //                return i;
    //            }
    //        }
    //    });

    //    var list = ['tiny', 'mini', 'youth', 'junior', 'senior', 'open'];
    //    var divisions = _.sortBy(data.info.divisions, function (division) {
    //        var relative = 0;
    //        for (var i = 0; i < list.length; i++) {
    //            if (division.id.indexOf(list[i]) != -1) {
    //                relative = i;
    //                break;
    //            }
    //        }
    //
    //        var levelposition = _.indexOf(self.levels, division.levelId) * 1000;
    //        var result = levelposition + relative;
    //        console.log({ "q": levelposition,  "a": relative, "b": result });
    //        return result;
    //    });
    //    self.divisions = _.map(divisions, function (division) {
    //        return division.id;
    //    });
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

