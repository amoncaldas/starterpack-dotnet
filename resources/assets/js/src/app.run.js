(function() {
  'use strict';

  angular
    .module('app')
    .run(run);

  run.$inject = ['$rootScope', '$state', '$stateParams', 'GlobalService'];

  function run($rootScope, $state, $stateParams, GlobalService) {

    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;

    // $stateChangeStart is fired whenever the state changes. We can use some parameters
    // such as toState to hook into details about the state as it is changing
    $rootScope.$on('$stateChangeStart', function(event, toState) {
      var user = JSON.parse(localStorage.getItem('user'));

      // If there is any user data in local storage then the user is quite
      // likely authenticated. If their token is expired, or if they are
      // otherwise not actually authenticated, they will be redirected to
      // the auth state because of the rejected request anyway
      if(user) {
        $rootScope.authenticated = true;
        $rootScope.currentUser = user;

        // If the user is logged in and we hit the auth route we don't need
        // to stay there and can send the user to the main state
        if(toState.name === GlobalService.loginState ) {
          // Preventing the default behavior allows us to use $state.go
          // to change states
          event.preventDefault();

          $state.go( GlobalService.homeState );
        }
      } else {
        if(toState.name !== GlobalService.loginState) {
          event.preventDefault();
          $state.go(GlobalService.loginState);
        }
      }
    });
  }
}());
