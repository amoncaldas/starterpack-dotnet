(function() {
  'use strict';

  angular
    .module('app')
    .factory('GlobalService', GlobalService);

  GlobalService.$inject = [];

  function GlobalService() {
    return {
      homeState: 'home',
      loginState: 'login'
    };
  }

}());
