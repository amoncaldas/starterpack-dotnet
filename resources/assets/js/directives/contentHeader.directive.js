(function() {
  'use strict';

  angular
      .module('app')
      .directive('contentHeader', contentHeader);

  function contentHeader() {
      var directive = {
          link: link,
          templateUrl: '/templates/directives/contentHeader.html',
          restrict: 'E',
          replace: true,
          scope: {
            title: '@title',
            module: '@module',
            description: '@description'
          }
      };
      return directive;

      function link(scope, element, attrs) {

      }
  }

}());
