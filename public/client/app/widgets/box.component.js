(function() {
  'use strict';

   /** @ngInject */
  angular
    .module('app')
    .component('box', {
      templateUrl: function(Global) {
        return Global.clientPath + '/widgets/box.html'
      },
      replace: true,
      transclude: {
        toolbarButtons: '?boxToolbarButtons',
        footerButtons: '?boxFooterButtons'
      },
      bindings: {
        title: '@'
      },
      controller: function($transclude) {
        var ctrl = this;

        ctrl.transclude = $transclude;
      }
    });
}());
