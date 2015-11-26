(function () {
  'use strict';

  angular
    .module('app')
    .config(config);

  config.$inject = ['$authProvider', 'Global', 'RestangularProvider'];

  function config($authProvider, Global, RestangularProvider) {
    // Satellizer configuration that specifies which API
    // route the JWT should be retrieved from
    RestangularProvider.setBaseUrl(Global.apiVersion);
    $authProvider.loginUrl = Global.apiVersion + '/authenticate';

  }
}());
