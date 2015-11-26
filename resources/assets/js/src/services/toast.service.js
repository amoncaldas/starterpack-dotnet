(function() {
  'use strict';

  angular
    .module('app')
    .factory('Toast', Toast);

  Toast.$inject = ['$window', 'lodash'];

  function Toast($window, _) {
    var toastr = $window.toastr;

    var obj = {
      success: success,
      error: error,
      errorValidation: errorValidation,
      info: info,
      warning: warning
    };

    function success(msg) {
      toastr.success(msg);
    }

    function error(errors) {
      if(angular.isObject(errors)) {
        var errorStr = "";

        //iterate over a object keys. Laravel send the errors in a object like:
        //{"name": ["o campo nome é obrigatório"], "password":["A confirmação de senha não confere.","senha deve ter no mínimo 6 caracteres."]}
        _.forIn(errors, function(keyErrors, key) {

          //itera sobre os erros de um atributo
          keyErrors.forEach(function(error) {
            errorStr += error + "<br/>";
          });
        });

        toastr.error(errorStr);
      } else {
        toastr.error(errors);
      }

    }

    function errorValidation(errors, msg) {
      obj.error(angular.isObject(errors) ? errors : msg);
    }

    function info(msg) {
      toastr.info(msg);
    }

    function warning(msg) {
      toastr.warning(msg);
    }

    return obj;
  }

}());
