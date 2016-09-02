(function() {
  'use strict';

  angular
    .module('app')
    .config(routes);

  /**
   * Arquivo de configuração com as rotas específicas do recurso <%= resource_name %>
   *
   * @param {any} $stateProvider
   * @param {any} Global
   */
  /** @ngInject */
  function routes($stateProvider, Global, $translate) {
    $stateProvider
      .state('<%= resource_name %>', {
        url: '/<%= resource_name %>',
        templateUrl: Global.clientPath + '/<%= resource_name %>/<%= resource_name %>.html',
        controller: '<%= name_uppercase %>Controller as <%= alias_controller %>',
        data: { breadcrumbs: $translate.instant('breadcrumbs.<%= resource_name %>'), needProfile: ['admin'] }
      });
  }
}());
