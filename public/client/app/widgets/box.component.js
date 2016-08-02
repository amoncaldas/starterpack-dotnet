(function() {
  'use strict';

   /** @ngInject */
  angular
    .module('app')
    .component('box', {
      replace: true,
      templateUrl: function(Global) {
        return Global.clientPath + '/widgets/box.html'
      },
      transclude: {
        toolbarButtons: '?boxToolbarButtons',
        footerButtons: '?boxFooterButtons'
      },
      bindings: {
        boxTitle: '@'
      },
      controller: function($transclude) {
        var ctrl = this;

        ctrl.transclude = $transclude;
      }
    });
}());
