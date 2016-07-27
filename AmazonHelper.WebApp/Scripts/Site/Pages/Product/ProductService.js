(function () {
    'use strict';

    angular
        .module('productManagementApp')
        .factory('productService', productService);

    productService.$inject = ['$http', 'logger'];

    function productService($http, logger) {
        return {
            getProducts: getProducts,
            saveProduct: saveProduct,
            removeProduct: removeProduct,
            getProduct: getProduct,
            getProductGroups: getProductGroups,
            customAction: customAction
        };

        function getProductGroups() {

            return $http.get(routings.getProductGroupsUrl)
                .then(function (response) {
                    return response.data;
                })
               .catch(function () {
                   logger.log('get stores request failed');
               });
        }

        function getProducts(pageNum, pageSize, criteria) {

            return $http.get(routings.getProductsUrl + '?pageNum=' + pageNum + '&pageSize=' + pageSize + '&asin=' + criteria.asin)
                .then(function (response) {
                    return response.data;
                })
               .catch(function () {
                   logger.log('get products request failed');
               });
        }

        function saveProduct(product) {
            return $http.post(routings.saveProductUrl, product)
                .then(function (response) {
                    return response.data;
                })
               .catch(function () {
                   logger.log('save product request failed');
               });
        }

        /*
            removing product call 
        */
        function customAction(productIds, action) {
            return $http.post(routings.getCustomActionUrl, { productIds: productIds, action: action })
                .then(function (response) {
                    return response.data;
                })
               .catch(function () {
                   logger.log('remove product request failed');
               });
        }

        /*
            removing product call 
        */
        function removeProduct(productId) {
            return $http.post(routings.removeProductUrl, { productId: productId })
                .then(function (response) {
                    return response.data;
                })
               .catch(function () {
                   logger.log('remove product request failed');
               });
        }

        function getProduct(productId) {
            return $http.get(routings.getProductUrl + '?productId=' + productId)
                .then(function (response) {
                    return response.data;
                })
               .catch(function () {
                   logger.log('get product request failed');
               });
        }
    }

})();