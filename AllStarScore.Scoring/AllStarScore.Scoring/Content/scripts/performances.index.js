$(document).ready(function () {
    AllStarScore.PerformanceViewModel = new AllStarScore.PerformanceViewModel();
});

AllStarScore.PerformanceViewModel = function () {
    var self = this;

    self.templateToUse = function (entry) {
        return entry.performanceId ? 'performance-template' : 'block-template';
    };

    self.dataToUse = function (entry) {
        if (entry.performanceId) {
            entry = $.extend(entry, AllStarScore.CompetitionData.performances[entry.performanceId]);

        } else {
            entry.display = entry.text || entry.type;
        }

        return entry;
    };

    self.showPerformance = function (entry) {

        var context = AllStarScore.CompetitionData.securityContext;
        if (context.isTabulator) {
            return true;
        }

        return context.panel == entry.panel;
    };

    return self;
}; 