AllStarScore.Scheduling = {};

AllStarScore.Scheduling.Print = {
    Start: function () {
        var viewModel = new AllStarScore.Scheduling.PrintViewModel();
        ko.applyBindings(viewModel, document.getElementById('scheduling_print'));
    }
};


AllStarScore.Scheduling.PrintViewModel = function () {
    var self = this;

    var data = AllStarScore.CompetitionData;
    self.performances = data.performances;
    self.schedule = data.schedule;

    self.entryTypes = {
        'Performance': { duration: self.schedule.defaultDuration, template: 'performance-template' },
        'Open': { duration: self.schedule.defaultDuration, template: 'block-template' },
        'Break': { duration: self.schedule.defaultBreakDuration, template: 'block-template' },
        'Awards': { duration: self.schedule.defaultAwardsDuration, template: 'block-template' }
    };

    self.toPerformance = function (entry) {
        if (!entry.performanceId) return entry;
        return $.extend(entry, self.performances[entry.performanceId]);
    };

    self.display = function (entry) {
        return entry.text || entry.type;
    };

    self.calculateWarmup = function (entry) {
        var warmup = entry.warmupTime || self.schedule.defaultWarmupTime;
        var result = new Date(entry.time.getTime() - warmup * 60 * 1000);
        return result;
    };

    $('#scheduling_print').on('submit', '.excel form', function (e) {
        var html = $('#scheduling_print .main').html();
        $('#excel-data').attr('value', html);
    });

    return self;
};