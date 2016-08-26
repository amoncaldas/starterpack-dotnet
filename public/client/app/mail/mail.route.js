(function() {
  'use strict';

  angular
    .module('app')
    .config(routes);

  /**
   * Arquivo de configuração com as rotas específicas do recurso user
   *
   * @param {any} $stateProvider
   * @param {any} Global
   */
  /** @ngInject */
  function routes($stateProvider, Global) {
    $stateProvider
      .state('mail', {
        url: '/email',
        templateUrl: Global.clientPath + '/mail/sender-mail-user.html',
        controller: 'MailController as mailCtrl',
        data: { breadcrumbs: 'Administração > Envio de e-mail', needProfile: ['admin'] }
      });

  }
}());
