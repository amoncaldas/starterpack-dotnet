(function() {
  'use strict';

  angular
    .module('app')
    .constant('Global', {
      appName: 'Starter Pack',
      clientPath: 'client/app',
      homeState: 'dashboard',
      loginState: 'login',
      resetPasswordState: 'password-reset',
      notAuthorizedState: 'not-authorized',
      apiVersion: 'v1'
    });
}());
