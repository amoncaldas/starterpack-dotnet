(function() {

  'use strict';

  angular
    .module('app')
    .filter('prDate', prDate);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function prDate(moment) {
    return function(value) {
      return moment(value).format('DD/MM/YYYY');
    }
  }

})();
