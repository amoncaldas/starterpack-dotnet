(function() {
  'use strict';

  /** @ngInject */
  angular
    .module('app')
    .component('mdFabFloat', {
      templateUrl: function(Global) {
        return Global.clientPath + '/widgets/md-fab-float.html'
      },
      replace: true,
      bindings: {
        theme: '@',
        icon: '@',
        visible: '@'
      },
      controller: function() {
        var ctrl = this;

        ctrl.$onInit = function() {
          // Make a copy of the initial value to be able to reset it later
          ctrl.theme = angular.isDefined(ctrl.theme) ? ctrl.theme : 'md-primary';
          ctrl.visible = angular.isDefined(ctrl.visible) ? true : false;

        };
      }
    });

}());
