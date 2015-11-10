(function() {
  'use strict';

  angular
      .module('app')
      .directive('contentBody', contentBody);

  function contentBody() {
      var directive = {
          templateUrl: '/templates/directives/contentBody.html',
          restrict: 'E',
          replace: true,
          transclude: true
      };
      return directive;
  }

}());
