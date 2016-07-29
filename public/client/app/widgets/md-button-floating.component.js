(function() {
  'use strict';

  /** @ngInject */
  angular
    .module('app')
    .component('mdButtonFloating', {
      templateUrl: function(Global) {
        return Global.clientPath + '/widgets/md-button-floating.html'
      },
      replace: true,
      bindings: {
        theme: '@',
        icon: '@',
        title: '@',
        uiSref: '@',
        onClick: '&'
      },
      controller: function() {
        var ctrl = this;

        ctrl.$onInit = function() {
          // Make a copy of the initial value to be able to reset it later
          ctrl.theme = angular.isDefined(ctrl.theme) ? ctrl.theme : 'md-primary';

        };
      }
    });

}());
