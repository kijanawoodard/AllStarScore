var viewModel;

$(document).ready(function () {
    viewModel = ko.mapping.fromJS(window.detailsScoreSheetsData, mapping);
    ko.applyBindings(viewModel, document.getElementById('scoresheets_details'));
});

var mapping = {
    'viewModel': {
        create: function (options) {
            return new ScoreSheetsDetailsViewModel(options.data);
        }
    },
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

var ScoreSheetsDetailsViewModel = (function (data) {
    ko.mapping.fromJS(data, mapping, this);
});

var ScheduleModel = function (data) {
    var self = this;
    
    ko.mapping.fromJS(data, mapping, self);

    self.panels = ko.computed(function () {
        return _.map(_.range(self.numberOfPanels()), function (i) {
            return String.fromCharCode(65 + i);
        });
    }, self);
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

    var getScoringMap = function (judge) {
        var division = self.registration().divisionId;
        var level = self.registration().levelId;
        var map = viewModel.scoringMap[judge] || viewModel.scoringMap[division] || viewModel.scoringMap[level];
        return map;
    };
    self.getTemplate = function (judge) {
        var map = getScoringMap(judge);
        return map['template']();
    };

    self.getTemplateData = function (judge) {
        var map = getScoringMap(judge);
        return map['scoringDefinition'];
    };
};