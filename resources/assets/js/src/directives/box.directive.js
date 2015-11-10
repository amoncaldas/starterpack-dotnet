(function() {
  'use strict';

  angular
      .module('app')
      .directive('box', box);

  function box() {
      var directive = {
          templateUrl: '/templates/directives/box.html',
          restrict: 'E',
          replace: true,
          transclude: true,
          scope: {
            title: '@title'
          }
      };
      return directive;
  }

}());
