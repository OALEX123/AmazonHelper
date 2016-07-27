(function () {
    'use strict';

    angular
        .module('statsManagementApp')
        .factory('statsService', statsService);

    statsService.$inject = ['$http', 'logger'];

    function statsService($http, logger) {
        return {
            getStats: getStats,
            customAction: customAction
        };


        function getStats(pageNum, pageSize, filter) {

            return $http.get(routings.getStatsUrl + '?pageNum=' + pageNum + '&pageSize=' + pageSize + '&asin=' + filter.asin)
                .then(function (response) {
                    return response.data;
                })
               .catch(function () {
                   logger.log('get stores request failed');
               });
        }

        function customAction(action) {
            return $http.post(routings.customActionUrl, { action: action })
                .then(function (response) {
                    return response.data;
                })
               .catch(function () {
                   logger.log('remove product request failed');
               });
        }
    }

})();
