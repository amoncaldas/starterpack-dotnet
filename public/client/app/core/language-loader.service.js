(function() {

  'use strict';

  angular
    .module('app')
    .factory('languageLoader', LanguageLoader);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function LanguageLoader($q, SupportService) {
    // return loaderFn
    return function() {
      var deferred = $q.defer();
      // do something with $http, $q and key to load localization files

      SupportService.langs().then(function(langs) {
        var data = {
          yes: 'sim',
          no: 'não',
          all: 'todos',
          models: {
            user: 'Usuário'
          }
        }

        //Merge com os langs definidos no servidor
        data = angular.merge(data, langs);

        return deferred.resolve(data);
      });

      return deferred.promise;
    }
  }

})();
