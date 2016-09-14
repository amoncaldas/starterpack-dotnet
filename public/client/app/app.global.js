(function() {
  'use strict';

  angular
    .module('app')
    .constant('Global', {
      appName: 'Base Laravel',
      clientPath: 'client/app',
      homeState: 'dashboard',
      loginState: 'login',
      resetPasswordState: 'password-reset',
      notAuthorizedState: 'not-authorized',
      apiVersion: 'v1'
    });
}());
