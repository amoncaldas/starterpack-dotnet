(function() {
  'use strict';

  /** @ngInject */
  angular
    .module('app')
    .component('boxButtonBar', {
      templateUrl: function(Global) {
        return Global.clientPath + '/widgets/box-button-bar.html'
      },
      replace: true,
      transclude: true
    });
}());
