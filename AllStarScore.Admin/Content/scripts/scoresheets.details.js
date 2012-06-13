var viewModel;

$(document).ready(function () {
    viewModel = ko.mapping.fromJS(window.detailsScoreSheetsData, mapping);
    ko.applyBindings(viewModel, document.getElementById('scoresheets_details'));
    viewModel.schedule.loadVisibilityMatrix();
});

var mapping = {
    'schedule': {
        create: function (options) {
            return new ScheduleModel(options.data);
        }
    },
    'days': {
        create: function (options) {
            return new DayModel(options.data);
        }
    },
    'entries': {
        create: function (options) {
            return new EntryModel(options.data);
        }
    },
    'divisionPanels': {
        create: function (options) {
            return options.data;
        }
    },
    'registrations': {
        create: function (options) {
            return options.data;
        }
    }
};

var ScheduleModel = function (data) {
    var self = this;

    ko.mapping.fromJS(data, mapping, self);

    self.panels = ko.computed(function () {
        return _.map(_.range(self.numberOfPanels()), function (i) {
            return String.fromCharCode(65 + i);
        });
    }, self);

    self.visibilityMatrix = ko.observableArray();
    self.competitionDays = ko.observableArray();
    self.levels = ko.observableArray();

    self.loadVisibilityMatrix = function () {
        //created this function to have access to viewModel after mapping
        _.each(self.panels(), function (panel) {
            self.visibilityMatrix.push(panel);
            _.each(window.viewModel.judgePanel.judges(), function (judge) {
                self.visibilityMatrix.push(panel + judge.designator());
            });
        });

        _.each(self.days(), function (node) {
            //this is outside of the function 'load' function to get the competitionDays 
            //what are we doing here? normalizing the date to a string for the checkbox compare; 
            //the value of the checkbox has to be a string not a date object;
            //we're also divorcing what gets attached the checkbox so the formmatted value doesn't change our real day object
            var formatted = node.day().toString('ddd MM/dd/yyyy');
            self.visibilityMatrix.push(formatted);
            self.competitionDays.push(formatted);

            _.each(node.entries(), function (entry) {
                var registration = entry.registration();
                if (registration) {
                    if (_.indexOf(self.visibilityMatrix(), registration.levelId) == -1) {
                        self.visibilityMatrix.push(registration.levelId);
                        self.levels.push({ id: registration.levelId, name: registration.levelName });
                    }
                }
            });
        });
    };

    self.shouldShow = function (entry, parents) {
        var panel = parents[2];
        var day = parents[0].day().toString('ddd MM/dd/yyyy');
        var judge = panel + parents[1].designator();
        var level = entry.registration().levelId;
        
        var result = _.indexOf(self.visibilityMatrix(), panel) > -1 &&
                     _.indexOf(self.visibilityMatrix(), day) > -1 &&
                     _.indexOf(self.visibilityMatrix(), judge) > -1 &&
                    _.indexOf(self.visibilityMatrix(), level) > -1;
        return result;
    };
};

var DayModel = function (data) {
    //console.log(data.schedule);
    data.day = new Date(data.day);
    ko.mapping.fromJS(data, mapping, this);
};

var getRegistrationId = function (id) {
    return id.charAt(0).toLowerCase() + id.slice(1); //might change name of registration class in raven to avoid this
};

var EntryModel = function (data) {
    var self = this;

    data.time = new Date(data.time);
    ko.mapping.fromJS(data, {}, self);

    self.registration = ko.computed(function () {
        if (!this.registrationId)
            return undefined;

        var id = getRegistrationId(this.registrationId());
        return viewModel.registrations[id];
    }, self);

    self.panel = ko.computed(function () {
        if (!self.registration())
            return '';

        return viewModel.schedule.divisionPanels[self.registration().divisionId];
    }, self);

    self.isMyPanel = function (panel) {
        return self.panel() == panel;
    };

    self.getTemplate = function (judge) {
        var division = self.registration().divisionId;
        var level = self.registration().levelId;
        var map = viewModel.scoringMap[judge] || viewModel.scoringMap[division] || viewModel.scoringMap[level];
        return map();
    };
};