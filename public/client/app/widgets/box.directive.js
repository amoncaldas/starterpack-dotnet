(function() {
  'use strict';

  angular
    .module('app')
    .directive('box', box);

  /** @ngInject */
  function box(Global) {
    var directive = {
      templateUrl: Global.clientPath + '/widgets/box.html',
      restrict: 'E',
      replace: true,
      transclude: true,
      scope: {
        title: '@title',
        width: '@width'
      }
    };

    return directive;
  }

}());
