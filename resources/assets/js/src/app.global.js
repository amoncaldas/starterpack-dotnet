(function() {
  'use strict';

  angular
    .module('app')
    .constant('Global', {
      homeState: 'home',
      loginState: 'login',
      apiVersion: 'v1'
    });
}());