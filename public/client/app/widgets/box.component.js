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
      transclude: true,
      bindings: {
        title: '@'
      }
    });
}());
