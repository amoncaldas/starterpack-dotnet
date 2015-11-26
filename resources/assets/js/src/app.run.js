(function() {
  'use strict';

  angular
    .module('app')
    .run(run);

  run.$inject = ['$rootScope', '$state', '$stateParams', 'Auth'];

  function run($rootScope, $state, $stateParams, Auth) {

    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;
    $rootScope.auth = Auth;

  }
}());
