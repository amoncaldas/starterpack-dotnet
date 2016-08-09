/*eslint angular/file-name: 0*/
(function() {
  'use strict';

  angular
    .module('app')
    .factory('serviceFactory', serviceFactory);

  /** @ngInject */
  function serviceFactory($modelFactory) {
    var service = function(url, options) {
      var model;
      var defaultOptions = {
        actions: {
          paginate: {
            method: 'GET',
            isArray: false,
            wrap: false,
            afterRequest: function(response) {
              if (response['items']) {
                response['items'] = model.List(response['items']);
              }

              return response;
            }
          }
        }
      }

      model = $modelFactory(url, angular.merge(defaultOptions, options))

      return model;
    }

    return service;
  };
})();
