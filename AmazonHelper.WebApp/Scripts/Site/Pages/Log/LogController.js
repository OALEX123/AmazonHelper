(function () {
    'use strict';

    angular.module('logManagementApp').controller('LogController', LogController);

    LogController.$inject = ['$scope', '$timeout', '$interval', 'logService'];

    function LogController($scope, $timeout, $interval, logService) {
        var vm = this;
        // array to store stats
        vm.logs = [];
        vm.loading = false;
        // stats paging settings
        vm.logPageNum = 1;
        vm.logPageSize = '100';
        vm.logTotalCount = 0;
        vm.logPagingOptions = ['100'];
        vm.logFilter = {
            asin: ''
        };

        vm.customActions = {
            removeForLastWeek: 1,
            removeAll: 2
        };

        // pageable notifies page is changed
        vm.onLogsPageChanged = onLogsPageChanged;

        vm.reloadLogs = reloadLogs;

        vm.triggerLogsLoading = triggerLogsLoading;

        // loading stats on page load
        triggerLogsLoading();


        function triggerLogsLoading() {
            getLogsPaged(vm.logPageNum, vm.logPageSize, vm.logFilter);
        }


        // watching store page num is changed
        $scope.$watch(function () {
            return vm.logPageNum;
        }, function (current, original) {
            if (current !== original) {
                triggerLogsLoading();
            }
        });

        // watching store page num is changed
        $scope.$watch(function () {
            return vm.logPageSize;
        }, function (current, original) {
            if (current !== original) {
                triggerLogsLoading();
            }
        });

        // pageable notifies that page is changed
        function onLogsPageChanged(pageNum) {
            vm.logPageNum = pageNum;
        }

        function reloadLogs() {
            if (vm.logPageNum === 1) {
                triggerLogsLoading();
            }
            else {
                vm.logPageNum = 1;
            }
        }

        function getLogsPaged(pageNum, pageSize, filter) {

            logService.getLogs(pageNum, pageSize, filter)
                .then(function (response) {
                    if (response.success) {
                        vm.logs = response.data.logs;
                        vm.logsTotalCount = response.data.logsTotalCount;
                        return vm.stats;
                    }
                    else {
                        alert(response.message);
                    }
                });
        }
    }
})();