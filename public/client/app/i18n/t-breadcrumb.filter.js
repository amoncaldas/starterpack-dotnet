(function() {

  'use strict';

  angular
    .module('app')
    .filter('tBreadcrumb', tBreadcrumb);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function tBreadcrumb($filter) {
    /**
     * Filtro para tradução do breadcrumb (titulo da tela com rastreio)
     * 
     * @param {any} id chave com a identificação da tela
     * @returns a tradução caso encontre se não o id passado por parametro
     */      
    return function(id) {
      var key = 'breadcrumbs.' + id;
      var translate = $filter('translate')(key);

      return (translate === key) ? id : translate;
    }
  }

})();
