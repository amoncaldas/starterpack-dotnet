(function() {
  'use strict';

  angular.module('ngProdeb')
    .factory('PrDialog', dialogService);

  /** @ngInject */
  function dialogService($mdDialog) {

    return {
      show: show
    };

    function alert(defaultOptions) {

      return $mdDialog.alert()
              .clickOutsideToClose(defaultOptions.clickOutsideToClose)
              .title(defaultOptions.title)
              .textContent(defaultOptions.textContent)
              .ariaLabel(defaultOptions.textContent)
              .ok(defaultOptions.ok)
              .targetEvent(defaultOptions.targetEvent);
    }

    function confirm(defaultOptions) {

      return $mdDialog.confirm()
              .clickOutsideToClose(defaultOptions.clickOutsideToClose)
              .title(defaultOptions.title)
              .textContent(defaultOptions.textContent)
              .ariaLabel(defaultOptions.textContent)
              .ok(defaultOptions.ok)
              .targetEvent(defaultOptions.targetEvent);
    }

    function prompt(defaultOptions) {

      return $mdDialog.prompt()
              .clickOutsideToClose(defaultOptions.clickOutsideToClose)
              .title(defaultOptions.title)
              .placeholder(defaultOptions.placeholder)
              .textContent(defaultOptions.textContent)
              .ariaLabel(defaultOptions.textContent)
              .initialValue(defaultOptions.initialValue)
              .ok(defaultOptions.ok)
              .cancel(defaultOptions.cancel)
              .targetEvent(defaultOptions.targetEvent);
    }

    /*function custom(controller, defaultOptions) {

      return $mdDialog.prompt()
              .clickOutsideToClose(defaultOptions.clickOutsideToClose)
              .title(defaultOptions.title)
              .placeholder(defaultOptions.placeholder)
              .textContent(defaultOptions.textContent)
              .ariaLabel(defaultOptions.textContent)
              .initialValue(defaultOptions.initialValue)
              .ok(defaultOptions.ok)
              .cancel(defaultOptions.cancel)
              .targetEvent(defaultOptions.targetEvent);
    }*/

    function show(type, options) {

      var defaultOptions = {
        clickOutsideToClose: true,
        title: 'This is an alert title',
        textContent: 'You can specify some description text in here.',
        ariaLabel: 'Alert Dialog Demo',
        ok: 'Got it!',
        cancel: 'Cancel it!'
      };

      angular.merge(defaultOptions, options);

      var dialog;

      switch (type) {
        case 'alert':
          dialog = $mdDialog.show(alert(defaultOptions));
          break;
        case 'confirm':
          dialog = $mdDialog.show(confirm(defaultOptions));
          break;
        case 'prompt':
          dialog = $mdDialog.show(prompt(defaultOptions));
          break;
        default:
          dialog = $mdDialog.show(defaultOptions);
          break;
      }

      return dialog;

    }

    /*var PrDialog = function(controller, targetEvent, options) {

      var defaultOptions = {

      };

      angular.merge(defaultOptions, options);

      $mdDialog.show()
      .then(function(result) {

      });
    }*/

    /**
     * Exibe o spinner com a mensagem informada
     *
     * @param {string} message - mensagem a ser exibida

    function alert(message) {
      //emite o sinal para a diretiva informando que o componente spinner deve ser exibido
      $rootScope.$broadcast('show-spinner', { message: message });
    }

    function alert() {
      $rootScope.$broadcast('hide-spinner');
    }*/
  }

})();
