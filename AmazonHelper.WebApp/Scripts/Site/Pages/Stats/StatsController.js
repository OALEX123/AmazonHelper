(function () {
    'use strict';

    angular.module('statsManagementApp').controller('StatsController', StatsController);

    StatsController.$inject = ['$scope', '$timeout', '$interval', 'statsService'];

    function StatsController($scope, $timeout, $interval, statsService) {
        var vm = this;
        // array to store stats
        vm.stats = [];
        vm.loading = false;
        // stats paging settings
        vm.statsPageNum = 1;
        vm.statsPageSize = '20';
        vm.statsTotalCount = 0;
        vm.statsPagingOptions = ['10', '20', '50'];
        vm.statsFilter = {
            asin: ''
        };
        vm.timerLabel = '';

        var updatingInterval = 20;
        var timerCounter = updatingInterval;

        vm.customActions = {
            removeForLastWeek: 1,
            removeAll: 2
        };

        // pageable notifies page is changed
        //vm.onStatsPageChanged = onStatsPageChanged;

        vm.reloadStats = reloadStats;

        vm.triggerStatsLoading = triggerStatsLoading;
        vm.performCustomAction = performCustomAction;

        // loading stats on page load
        triggerStatsLoading();

        $interval(triggerStatsLoading, updatingInterval * 1000);

        $interval(updateTimerLabel, 1000);

        function triggerStatsLoading() {
            console.log('getProductsPaged(' + 'productsPageNum: ' + vm.statsPageNum + ' productsPageSize: ' + vm.statsPageSize + ')');
            getStatsPaged(vm.statsPageNum, vm.statsPageSize, vm.statsFilter);
        }

        function updateTimerLabel() {
            vm.timerLabel = --timerCounter;
            if (timerCounter === 0) {
                timerCounter = updatingInterval;
            }
        }

        /*
        * @name setLoading
        * @desc manages loading message
        * @param {Bool} loading state
         */
        function setLoading(isLoading) {
            if (isLoading) {
                vm.loading = true;
            }
            else {
                $timeout(function () { vm.loading = false; }, 700);
            }
        }

        // watching store page num is changed
        $scope.$watch(function () {
            return vm.statsPageNum;
        }, function (current, original) {
            if (current !== original) {
                triggerStatsLoading();
            }
        });

        // watching store page num is changed
        $scope.$watch(function () {
            return vm.statsPageSize;
        }, function (current, original) {
            if (current !== original) {
                triggerStatsLoading();
            }
        });

        function reloadStats() {
            if (vm.statsPageNum === 1) {
                triggerStatsLoading();
            }
            else {
                vm.statsPageNum = 1;
            }
        }

        function performCustomAction(action) {
            if (confirm('You sure you want to remove statistics?')) {
                customAction(action).then(function (response) {
                    if (response.success) {
                        reloadStats();
                    }
                    else {
                        alert(response.message);
                    }
                });
            }
        }

        function customAction(action) {
            return statsService.customAction(action)
            .then(function (response) {
                return response;
            });
        }

        function getStatsPaged(pageNum, pageSize, filter) {

            statsService.getStats(pageNum, pageSize, filter)
                .then(function (response) {
                    if (response.success) {
                        vm.stats = response.data.stats;
                        vm.statsTotalCount = response.data.statsTotalCount;
                        return vm.stats;
                    }
                    else {
                        alert(response.message);
                    }
                });
        }
    }
})();