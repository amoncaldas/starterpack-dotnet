(function() {
  'use strict';

  angular
    .module('app')
    .config(routes);

  /**
   * Arquivo de configuração com as rotas específicas do recurso em questão
   *
   * @param {any} $stateProvider
   * @param {any} Global
   */
  /** @ngInject */
  function routes($stateProvider, Global) {
    $stateProvider
      .state('mail', {
        url: '/email',
        templateUrl: Global.clientPath + '/mail/mails-send.html',
        controller: 'MailsController as mailsCtrl',
        data: { breadcrumbs: 'Administração > Envio de e-mail', needProfile: ['admin'] }
      });

  }
}());
