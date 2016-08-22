(function() {

  'use strict';

  angular
    .module('app')
    .filter('prDatetime', prDatetime);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function prDatetime(moment) {
    return function(value) {
      return moment(value).format('DD/MM/YYYY HH:mm');
    }
  }

})();
