(function () {
    'use strict';

    angular.module('ui-directives', []);

    angular.module('ui-directives')
        .directive('storeList', storeList);

    angular.module('ui-directives')
        .directive('productList', productList);

    angular.module('ui-directives')
        .directive('pageable', pageable);

    function storeList() {
        return {
            templateUrl: 'storeListTpl',
            restrict: 'EA',
            scope: {
                stores: '=',
                onShowProducts: '='
            }
        };
    }

    function productList() {

        return {
            templateUrl: 'productListTpl',
            restrict: 'EA',
            scope: {
                products: '='
            }
        };
    }

    function pageable() {
        var tpl = '<ul class="pagination mtm mbm" ng-show="pages.length>1">' +
                       '<li ng-repeat="page in pages" ng-class="getPageItemClass(page)">' +
                            '<a href="javascript:void(0);" ng-click="changePage(page.index)">{{page.name}}</a>' +
            '           </li>' +
                   '</ul>';
        return {
            template: tpl,
            restrict: 'EA',
            link: function (scope, el, attributes) {

                init();

                scope.changePage = function (index) {
                    scope.onPageChanged(index);
                }

                scope.getPageItemClass = function (page) {
                    return page.isCurrent ? 'active' : '';
                }

                scope.$watch('pageSize', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        init();
                    }
                });

                scope.$watch('pageNum', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        init();
                    }
                });

                scope.$watch('totalCount', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        init();
                    }
                });

                function init() {
                    var pages = [];
                    //debugger;
                    var totalPages = Math.ceil(scope.totalCount / scope.pageSize);
                    console.log(totalPages);
                    for (var i = 1; i < totalPages + 1; i++) {
                        pages.push({ index: i, name: i, isCurrent: i === scope.pageNum });
                    }

                    scope.pages = pages;
                }

            },
            scope: {
                pageSize: '=',
                pageNum: '=',
                totalCount: '=',
                onPageChanged: '='
            }
        };
    }

})();