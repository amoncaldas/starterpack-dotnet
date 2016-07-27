(function() {
  'use strict';

  /** @ngInject */
  angular
    .module('app')
    .component('contentBody', {
      templateUrl: function(Global) {
        return Global.clientPath + '/widgets/content-body.html'
      },
      replace: true,
      transclude: true
    });

}());
