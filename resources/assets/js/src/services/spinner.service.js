(function() {
  'use strict';

  angular
    .module('app')
    .factory('Spinner', spinnerService);

  spinnerService.$inject = ['$rootScope'];

  function spinnerService($rootScope) {
    return {
      show: show,
      hide: hide
    };

    function show(message) {
      $rootScope.$broadcast('show-spinner', { message: message });
    }

    function hide() {
      $rootScope.$broadcast('hide-spinner');
    }
  }

}());
