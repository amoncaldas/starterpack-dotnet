(function() {

  'use strict';

  angular
    .module('app')
    .filter('auditDetailTitle', auditDetailTitle);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function auditDetailTitle() {
    return function(auditDetail, status) {
      switch (auditDetail.type) {
        case 'created': return 'Informações do Cadastro';
        case 'updated':
          if (status === 'before') {
            return 'Antes da Atualização';
          } else {
            return 'Depois da Atualização';
          }
        case 'deleted': return 'Informações antes de remover';
      }
    }
  }

})();
