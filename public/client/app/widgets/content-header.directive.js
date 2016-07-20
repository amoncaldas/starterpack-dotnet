(function() {
  'use strict';

  angular
    .module('app')
    .directive('contentHeader', contentHeader);

  /** @ngInject */
  function contentHeader(Global) {
    var directive = {
      templateUrl: Global.clientPath + '/widgets/content-header.html',
      restrict: 'E',
      replace: true,
      scope: {
        title: '@title',
        module: '@module',
        description: '@description'
      }
    };

    return directive;
  }

}());
