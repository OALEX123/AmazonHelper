(function () {
    'use strict';

    angular.module('commomServices', []);

    angular.module('commomServices')
        .service('logger', logger);

    angular.module('commomServices')
        .service('notifier', notifier);

    function logger() {
        return {
            log: log
        };

        function log(message) {
            alert(message);
        }
    }

    function notifier() {
        return {
            nofityFromResponse: nofityFromResponse
        };

        function nofityFromResponse(response) {
            if (response.errors && Array.isArray(response.errors)) {
                var message = response.errors.join('\n');
                alert(message);
            }
            else {
                alert(response.message);
            }
        }
    }

})();