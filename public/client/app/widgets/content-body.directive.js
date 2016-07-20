(function() {
  'use strict';

  angular
    .module('app')
    .directive('contentBody', contentBody);

  /** @ngInject */
  function contentBody(Global) {
    var directive = {
      templateUrl: Global.clientPath + '/widgets/content-body.html',
      restrict: 'E',
      replace: true,
      transclude: true
    };

    return directive;
  }

}());
