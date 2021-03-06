﻿AllStarScore.ScoreSheetsStart = function () {
    var viewModel = new AllStarScore.ScoreSheets();
    ko.applyBindings(viewModel, document.getElementById('scoresheets_index'));
};

AllStarScore.ScoreSheets = function () {
    var self = this;

    self.competitionData = AllStarScore.CompetitionData;
    self.competitionData.company = { name: 'Spirit Celebration' }; //HACK: need to load company from server
    
    self.levels = self.competitionData.raw.levels;
    self.judges = [{ "responsibility": "judges-panel", "id": "1" }, { "responsibility": "judges-panel", "id": "2" }, { "responsibility": "judges-panel", "id": "3" }, { "responsibility": "judges-deductions", "id": "D" }, { "responsibility": "judges-legalities", "id": "L"}]; // self.competitionData.raw.judges; - HACK: needed judge info from scoring project, but it's too entagled with that project to move into shared models library. Hmmmm.
    self.schedule = self.competitionData.schedule;

    self.panels = ko.computed(function () {
        return _.map(_.range(self.schedule.numberOfPanels), function (i) {
            return String.fromCharCode(65 + i);
        });
    }, self);

    self.visibilityMatrix = ko.observableArray();
    self.competitionDays = ko.observableArray();

    _.each(self.panels(), function (panel) {
        self.visibilityMatrix.push(panel);
        _.each(self.judges, function (judge) {
            //http://stackoverflow.com/a/175787/214073
            if (!isNaN(+judge.id)) {
                self.visibilityMatrix.push(panel + judge.id);
            }
        });
    });

    //    self.visibilityMatrix.push('A1');
    //    self.visibilityMatrix.push('B1');
    //console.log(_.keys(self.competitionData.performances).length);

    _.each(self.schedule.days, function (day) {
        //what are we doing here? normalizing the date to a string for the checkbox compare; 
        //the value of the checkbox has to be a string not a date object;
        //we're also divorcing what gets attached the checkbox so the formmatted value doesn't change our real day object
        var formatted = day.day.toString('ddd MM/dd/yyyy');
        self.visibilityMatrix.push(formatted);
        self.competitionDays.push(formatted);
        //console.log(day.entries.length);
    });

    _.each(self.levels, function (level) {
        self.visibilityMatrix.push(level.id);
    });

    self.toPerformance = function (performanceId) {
        //console.log(self.competitionData.performances[performanceId]);
        return self.competitionData.performances[performanceId];
    };

    self.shouldShow = function (performance, parents) {
        var panel = parents[3];
        var day = parents[1].day.toString('ddd MM/dd/yyyy');
        var judge = panel + parents[2].id;
        var level = performance.levelId;
        //console.log(panel + " " + day + " " + judge + " " + level);
        var result = _.indexOf(self.visibilityMatrix(), panel) > -1 &&
                     _.indexOf(self.visibilityMatrix(), day) > -1 &&
                     _.indexOf(self.visibilityMatrix(), judge) > -1 &&
                    _.indexOf(self.visibilityMatrix(), level) > -1;

        if (!result) {
            //console.log(panel + " " + day + " " + judge + " " + level);
        }
        return result;
    };

    self.getTemplate = function (performance, judge) {
        var maps = AllStarScore.ScoringMap.getMaps(performance, judge);
        return maps.scoreSheet;
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

        if (!result) {
            //console.log(panel + " " + day + " " + judge + " " + level);
        }
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