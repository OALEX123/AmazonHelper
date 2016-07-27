(function () {
    'use strict';

    angular.module('productManagementApp', ['ui-directives', 'commomServices', 'ui.bootstrap', 'angular-loading-bar'])
        .config(['cfpLoadingBarProvider', function(cfpLoadingBarProvider) {
            cfpLoadingBarProvider.includeSpinner = false;
            cfpLoadingBarProvider.latencyThreshold = 500;
        }]);

})();
