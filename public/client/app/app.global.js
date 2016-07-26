(function() {
  'use strict';

  angular
    .module('app')
    .constant('Global', {
      clientPath: 'client/app',
      homeState: 'dashboard',
      loginState: 'login',
      notAuthorizedState: 'not-authorized',
      apiVersion: 'v1',
      layout: {
        menuTop: true
      }
    });
}());
