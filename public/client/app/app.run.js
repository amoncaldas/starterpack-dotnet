(function() {
  'use strict';

  angular
    .module('app')
    .run(run);

  run.$inject = ['$rootScope', '$state', '$stateParams', 'Auth', 'Global'];

  // eslint-disable-next-line max-params
  function run($rootScope, $state, $stateParams, Auth, Global) {

    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;
    $rootScope.auth = Auth;
    $rootScope.global = Global;

    Auth.retrieveUserFromLocalStorage();

  }
}());
