(function() {
  'use strict';

  angular
    .module('app')
    .factory('MailService', MailService);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function MailService(Global, serviceFactory) {
    var model = serviceFactory('mails', {});

    return model;
  }

}());
