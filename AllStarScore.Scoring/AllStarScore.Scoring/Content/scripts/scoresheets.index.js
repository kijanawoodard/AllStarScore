$(document).ready(function () {
    console.log(AllStarScore);
//    console.log(window.detailsScoreSheetsData);
    AllStarScore.ScoreSheets = new AllStarScore.ScoreSheetViewModel(window.detailsScoreSheetsData);
});

AllStarScore.ScoreSheetViewModel = function () {
    var self = this;

    var competitionData = AllStarScore.CompetitionData;
    self.levels = competitionData.raw.levels;
    self.judges = competitionData.raw.judges;
    var templates = AllStarScore.ScoreSheetMap.all;

    self.panels = ko.computed(function () {
        return _.map(_.range(competitionData.schedule.numberOfPanels), function (i) {
            return String.fromCharCode(65 + i);
        });
    }, self);

    self.visibilityMatrix = ko.observableArray();
    self.competitionDays = ko.observableArray();

    _.each(self.panels(), function (panel) {
        self.visibilityMatrix.push(panel);
        _.each(self.judges, function (judge) {
            self.visibilityMatrix.push(panel + judge.id);
        });
    });

    _.each(competitionData.competition.days, function (day) {
        //what are we doing here? normalizing the date to a string for the checkbox compare; 
        //the value of the checkbox has to be a string not a date object;
        //we're also divorcing what gets attached the checkbox so the formmatted value doesn't change our real day object
        var formatted = day.toString('ddd MM/dd/yyyy');
        self.visibilityMatrix.push(formatted);
        self.competitionDays.push(formatted);
    });

    _.each(self.levels, function (level) {
        self.visibilityMatrix.push(level.id);
    });

    self.toPerformance = function (performanceId) {
        return competitionData.performances[performanceId];
    };

    self.shouldShow = function (performance, parents) {
        var panel = parents[3];
        var day = parents[1].day.toString('ddd MM/dd/yyyy');
        var judge = panel + parents[2].id;
        var level = performance.levelId;

        var result = _.indexOf(self.visibilityMatrix(), panel) > -1 &&
                     _.indexOf(self.visibilityMatrix(), day) > -1 &&
                     _.indexOf(self.visibilityMatrix(), judge) > -1 &&
                    _.indexOf(self.visibilityMatrix(), level) > -1;
        
        return result;
    };

    self.getTemplate = function (performance, judge) {
        var division = performance.divisionIdWithoutCompanyId;
        var level = performance.levelIdWithoutCompanyId;
        var map = templates[judge.responsibility]
                        || templates[division]
                        || templates[level];

        return map;
    };

    return self;
};

var ScheduleModel = function (data) {
    var self = this;
    $.extend(self, data);

    self.days = _.map(self.days, function (day) {
        return new DayModel(day);
    });

    self.panels = ko.computed(function () {
        return _.map(_.range(self.numberOfPanels), function (i) {
            return String.fromCharCode(65 + i);
        });
    }, self);

    self.visibilityMatrix = ko.observableArray();
    self.competitionDays = ko.observableArray();
    self.levels = ko.observableArray();

    _.each(self.panels(), function (panel) {
        self.visibilityMatrix.push(panel);
        _.each(window.viewModel.judgePanel.judges, function (judge) {
            self.visibilityMatrix.push(panel + judge.id);
        });
    });

    _.each(self.days, function (node) {
        //what are we doing here? normalizing the date to a string for the checkbox compare; 
        //the value of the checkbox has to be a string not a date object;
        //we're also divorcing what gets attached the checkbox so the formmatted value doesn't change our real day object
        var formatted = node.day.toString('ddd MM/dd/yyyy');
        self.visibilityMatrix.push(formatted);
        self.competitionDays.push(formatted);   

        _.each(node.entries, function (entry) {
            var registration = entry.registration();
            if (registration) {
                if (_.indexOf(self.visibilityMatrix(), registration.levelId) == -1) {
                    self.visibilityMatrix.push(registration.levelId);
                    self.levels.push({ id: registration.levelId, name: registration.levelName });
                }
            }
        });
    });

    self.shouldShow = function (entry, parents) {
        var panel = parents[2];
        var day = parents[0].day.toString('ddd MM/dd/yyyy');
        var judge = panel + parents[1].id;
        var level = entry.registration().levelId;

        var result = _.indexOf(self.visibilityMatrix(), panel) > -1 &&
                     _.indexOf(self.visibilityMatrix(), day) > -1 &&
                     _.indexOf(self.visibilityMatrix(), judge) > -1 &&
                    _.indexOf(self.visibilityMatrix(), level) > -1;
        return result;
    };
};

var DayModel = function (data) {
    var self = this;
    $.extend(self, data);

    self.day = new Date(data.day);
    self.entries = _.map(self.entries, function (entry) {
        return new EntryModel(entry);
    });
};

var getRegistrationId = function (id) {
    return id.charAt(0).toLowerCase() + id.slice(1); //might change name of registration class in raven to avoid this
};

var EntryModel = function (data) {
    var self = this;
    
    $.extend(self, data); //http://stackoverflow.com/questions/7488208/am-i-over-using-the-knockout-mapping-plugin-by-always-using-it-to-do-my-viewmode

    self.time = new Date(data.time);

    self.registration = ko.computed(function () {
        if (!this.registrationId)
            return undefined;

        var id = getRegistrationId(this.registrationId);
        return window.viewModel.registrations[id];
    }, self);

    self.isMyPanel = function (panel) {
        return self.panel == panel;
    };

    self.getTemplate = function (judge) {
        var division = self.registration().divisionId;
        var level = self.registration().levelId;
        var map = window.viewModel.scoringMap[judge.responsibility]
                        || window.viewModel.scoringMap[division] 
                        || window.viewModel.scoringMap[level];
        return map;
    };
};