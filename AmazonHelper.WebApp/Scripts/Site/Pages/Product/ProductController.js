(function () {
    'use strict';

    angular.module('productManagementApp').controller('ProductController', ProductController);

    ProductController.$inject = ['$scope', '$timeout', '$uibModal', 'notifier', 'productService'];

    function ProductController($scope, $timeout, $uibModal, notifier, productService) {
        var vm = this;
        // array to store products
        vm.products = [];
        // array to store product groupes
        vm.productGroups = [];
        vm.loading = false;
        // product paging settings
        vm.productsPageNum = 1;
        vm.productsPageSize = '20';
        vm.productsTotalCount = 0;
        vm.productsPagingOptions = ['10', '20', '50'];

        vm.customActions = {
            activateSelected: 1,
            deactivateSelected: 2
        };

        vm.selection = {
            selectAll: 1,
            selectNone: 2,
            selectActive: 3,
            selectStopped: 4
        };

        vm.productFilter = {
            asin: ''
        };

        vm.isAllSelected = false;

        // pageable notifies page is changed
        //vm.onProductsPageChanged = onProductsPageChanged;
        // add new product button click
        vm.addProduct = addProduct;
        // edit existing product button click
        vm.editProduct = editProduct;
        // remove existing product button click
        vm.removeProduct = removeProduct;
        // reload button click
        vm.reloadProducts = reloadProducts;
        // active selected / decativate selected products button click
        vm.performCustomAction = performCustomAction;
        vm.selectProducts = selectProducts;

        vm.triggerProductsLoading = triggerProductsLoading;

        // loading products on page load
        triggerProductsLoading();

        // loading product groupes on page load
        getProductGroupes();


        function triggerProductsLoading() {
            console.log('getProductsPaged(' + 'productsPageNum: ' + vm.productsPageNum + ' productsPageSize: ' + vm.productsPageSize + ')');
            getProductsPaged(vm.productsPageNum, vm.productsPageSize, vm.productFilter);
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
            return vm.productsPageNum;
        }, function (current, original) {
            if (current !== original) {
                triggerProductsLoading();
            }
        });

        // watching store page num is changed
        $scope.$watch(function () {
            return vm.productsPageSize;
        }, function (current, original) {
            if (current !== original) {
                triggerProductsLoading();
            }
        });

        $scope.openManageProductModal = function (product) {

            var modalInstance = $uibModal.open({
                templateUrl: 'manageProductModal',
                controller: 'ManageProductModalCtrl',
                size: 'lg',
                resolve: {
                    product: product,
                    productGroups: function () {
                        return vm.productGroups;
                    }
                }
            });

            modalInstance.result.then(function (product) {
                saveProduct(product);
            }, function () {
                //$log.info('Modal dismissed at: ' + new Date());
            });
        };

        function addProduct() {
            $scope.openManageProductModal({});
        }

        function editProduct(product) {
            getProduct(product.productId).then(function (data) {
                $scope.openManageProductModal(data);
            });
        }

        function reloadProducts() {
            if (vm.productsPageNum === 1) {
                triggerProductsLoading();
            }
            else {
                vm.productsPageNum = 1;
            }
        }

        function performCustomAction(action) {
            var selectedProducts = [];
            angular.forEach(vm.products, function (product) {
                if (product.isSelected) {
                    selectedProducts.push(product.productId);
                }
            });

            if (selectedProducts.length > 0) {
                customAction(selectedProducts, action).then(function (respoonse) {
                });
            }
            else {
                alert('At least one item shoud be selected');
            }
        }

        function selectProducts(action) {

            if (!action) {
                if (vm.isAllSelected) {
                    action = vm.selection.selectNone;
                }
                else {
                    action = vm.selection.selectAll;
                }

                vm.isAllSelected = !vm.isAllSelected;
            }

            angular.forEach(vm.products, function (product) {
                switch (action) {
                    case vm.selection.selectAll:
                        product.isSelected = true;
                        break;
                    case vm.selection.selectNone:
                        product.isSelected = false;
                        break;
                    case vm.selection.selectActive:
                        product.isSelected = product.isActive;
                        break;
                    case vm.selection.selectStopped:
                        product.isSelected = !product.isActive;
                        break;
                    default:
                        break;
                }
            });
        }

        /*
       * @name getStores
       * @desc retrives list of stores from service
       * @returns {Array of stores}
        */
        function getProductsPaged(pageNum, pageSize, productFilter) {

            productService.getProducts(pageNum, pageSize, productFilter)
                .then(function (response) {
                    if (response.success) {
                        var products = response.data.products;
                        initProducts(products);
                        vm.products = products;
                        vm.productsTotalCount = response.data.productsTotalCount;
                        return vm.products;
                    }
                    else {
                        notifier.nofityFromResponse(response);
                    }
                });
        }

        function initProducts(products) {
            angular.forEach(products, function (product) {
                product.isSelected = false;
            });
        }

        function getProductGroupes() {
            productService.getProductGroups()
                .then(function (response) {
                    if (response.success) {
                        vm.productGroups = response.data;
                        return vm.productGroups;
                    }
                    else {
                        notifier.nofityFromResponse(response);
                    }
                });
        }

        function getProduct(productId) {
            return productService.getProduct(productId)
                .then(function (response) {
                    if (response.success) {
                        //vm.productGroups = response.data;
                        return response.data;
                    }
                    else {
                        notifier.nofityFromResponse(response);
                    }
                });
        }

        function saveProduct(product) {
            productService.saveProduct(product)
                .then(function (response) {
                    if (response.success) {
                        reloadProducts();
                    }
                    else {
                        notifier.nofityFromResponse(response);
                    }
                });
        }

        function removeProduct(product) {
            if (confirm('You sure you want to delete selected product with all statictics?')) {
                productService.removeProduct(product.productId)
                .then(function (response) {
                    if (response.success) {
                        reloadProducts();
                    }
                    else {
                        notifier.nofityFromResponse(response);
                    }
                });
            }

        }

        function customAction(productIds, action) {
            return productService.customAction(productIds, action)
            .then(function (response) {
                if (response.success) {
                    reloadProducts();
                }
                else {
                    notifier.nofityFromResponse(response);
                }
            });
        }
    }

    angular.module('productManagementApp').controller('ManageProductModalCtrl', function ($scope, $uibModalInstance, product, productGroups) {
        //debugger;
        $scope.product = product;
        $scope.productGroups = productGroups;

        $scope.submitProductForm = function () {
            //alert('submitProductForm');
            //debugger;
            if ($scope.productForm.$valid) {
                $uibModalInstance.close($scope.product);
            }
        };

        $scope.ok = function () {
            $uibModalInstance.close($scope.product);
        };

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    });

})();