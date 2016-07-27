(function () {
    'use strict';

    angular
        .module('settingsManagementApp')
        .factory('settingsService', settingsService);

    settingsService.$inject = ['$http', 'logger'];

    function settingsService($http, logger) {
        return {
            getSettings: getSettings,
            saveSettings: saveSettings
        };

        function getSettings() {

            return $http.get(routings.getSettingsUrl)
                .then(function (response) {
                    return response.data;
                })
               .catch(function () {
                   logger.log('get stores request failed');
               });
        }

        function saveSettings(settings) {

            return $http.post(routings.saveSettingsUrl, settings)
                .then(function (response) {
                    return response.data;
                })
               .catch(function () {
                   logger.log('get stores request failed');
               });
        }
    }

})();