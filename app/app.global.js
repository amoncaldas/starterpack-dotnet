(function() {
  'use strict';

  angular
    .module('app')
    .constant('Global', {
      appName: 'Starter Pack',
      homeState: 'dashboard',
      loginState: 'login',
      resetPasswordState: 'password-reset',
      notAuthorizedState: 'not-authorized',
      clientPath: 'app',
      apiPath: 'v1',
      imagePath: 'images'
    });
}());
