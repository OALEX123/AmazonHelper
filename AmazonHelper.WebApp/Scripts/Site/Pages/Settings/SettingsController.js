(function () {
    'use strict';

    angular.module('settingsManagementApp').controller('SettingsController', SettingsController);

    SettingsController.$inject = ['$scope', '$timeout', 'settingsService'];

    function SettingsController($scope, $timeout, settingsService) {
        var vm = this;
        vm.settings = {};
        
        vm.saveSettings = saveSettings;

        // loading settings
        getSettings();

        function saveSettings() {
            settingsService.saveSettings(vm.settings)
                .then(function (response) {
                    if (response.success) {
                        alert('Saved');
                    }
                    else {
                        alert(response.message);
                    }
                });
        }

        function getSettings() {
            settingsService.getSettings()
                .then(function (response) {
                    if (response.success) {
                        vm.settings = response.data;
                        return vm.settings;
                    }
                    else {
                        alert(response.message);
                    }
                });
        }
    }

})();