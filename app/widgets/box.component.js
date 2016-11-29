(function() {
  'use strict';

   /** @ngInject */
  angular
    .module('app')
    .component('box', {
      replace: true,
      templateUrl: ['Global', function(Global) {
        return Global.clientPath + '/widgets/box.html'
      }],
      transclude: {
        toolbarButtons: '?boxToolbarButtons',
        footerButtons: '?boxFooterButtons'
      },
      bindings: {
        boxTitle: '@'
      },
      controller: ['$transclude', function($transclude) {
        var ctrl = this;

        ctrl.transclude = $transclude;
      }]
    });
}());
