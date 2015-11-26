(function() {

  'use strict';

  angular
    .module('app')
    .controller('HomeController', HomeController);

  HomeController.$inject = ['$http', '$auth', '$rootScope', '$state', 'Global'];

  function HomeController($http, $auth, $rootScope, $state, Global) {
    var vm = this;

  }

})();