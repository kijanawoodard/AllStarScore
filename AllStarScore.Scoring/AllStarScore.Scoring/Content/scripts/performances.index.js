﻿AllStarScore.PerformanceViewModel = (function () {
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
} ()); //by self execucting these, function are available without going specifying model