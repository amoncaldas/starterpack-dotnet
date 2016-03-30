(function() {
  'use strict';

  angular
    .module('app')
    .constant('Global', {
      homeState: 'home',
      loginState: 'login',
      notAuthorizedState: 'not-authorized',
      apiVersion: 'v1'
    });
}());