(function () {
    'use strict';

    angular
        .module('logManagementApp')
        .factory('logService', logService);

    logService.$inject = ['$http', 'logger'];

    function logService($http, logger) {
        return {
            getLogs: getLogs
        };

        function getLogs(pageNum, pageSize, filter) {

            return $http.get(routings.getLogsUrl + '?pageNum=' + pageNum + '&pageSize=' + pageSize + '&asin=' + filter.asin)
                .then(function (response) {
                    return response.data;
                })
               .catch(function () {
                   logger.log('get stores request failed');
               });
        }
    }

})();
