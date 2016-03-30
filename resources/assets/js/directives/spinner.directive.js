(function() {
  'use strict';

  angular
    .module('app')
    .directive('spinner', spinnerDirective);

  spinnerDirective.$inject = [];

  function spinnerDirective() {
    return {
      restrict: 'E',
      template: '<style> #spin-label-component { min-width: 10%; padding: 10px; top: 0; right: 0; position: fixed; z-index:10000; }</style><label id="spin-label-component" class="label label-info" ng-show="spinner && spinner.show"> <span id="spin-label-text">{{spinner.message}}</span> <i class="fa fa-spinner fa-spin"></i></label>',
      link: function(scope, elem, attrs) {
        scope.spinner = { show: false, message: "" };

        scope.$on('show-spinner', function(event, args) {
          scope.spinner = { show: true, message: args.message };
        });

        scope.$on('hide-spinner', function(event, args) {
          scope.spinner = { show: false, message: "" };
        });
      }
    };
  }

}());
