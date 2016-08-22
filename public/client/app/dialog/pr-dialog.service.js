(function() {
  'use strict';

  angular.module('ngProdeb')
    .factory('PrDialog', dialogService);

  /** @ngInject */
  function dialogService(Global, $mdDialog, $mdMedia) {

    return {
      show: show
    };

    function alert(defaultOptions) {

      return $mdDialog.alert()
              .hasBackdrop(defaultOptions.hasBackdrop)
              .escapeToClose(defaultOptions.escapeToClose)
              .clickOutsideToClose(defaultOptions.clickOutsideToClose)
              .title(defaultOptions.title)
              .textContent(defaultOptions.textContent)
              .ariaLabel(defaultOptions.textContent)
              .ok(defaultOptions.ok)
              .targetEvent(defaultOptions.targetEvent)
              .fullscreen(defaultOptions.fullscreen);;
    }

    function confirm(defaultOptions) {

      return $mdDialog.confirm()
              .hasBackdrop(defaultOptions.hasBackdrop)
              .escapeToClose(defaultOptions.escapeToClose)
              .clickOutsideToClose(defaultOptions.clickOutsideToClose)
              .title(defaultOptions.title)
              .textContent(defaultOptions.textContent)
              .ariaLabel(defaultOptions.textContent)
              .ok(defaultOptions.ok)
              .targetEvent(defaultOptions.targetEvent)
              .fullscreen(defaultOptions.fullscreen);;
    }

    function prompt(defaultOptions) {

      return $mdDialog.prompt()
              .hasBackdrop(defaultOptions.hasBackdrop)
              .escapeToClose(defaultOptions.escapeToClose)
              .clickOutsideToClose(defaultOptions.clickOutsideToClose)
              .title(defaultOptions.title)
              .placeholder(defaultOptions.placeholder)
              .textContent(defaultOptions.textContent)
              .ariaLabel(defaultOptions.textContent)
              .initialValue(defaultOptions.initialValue)
              .ok(defaultOptions.ok)
              .cancel(defaultOptions.cancel)
              .targetEvent(defaultOptions.targetEvent)
              .fullscreen(defaultOptions.fullscreen);
    }

    function custom(defaultOptions) {

      return {
        hasBackdrop: defaultOptions.hasBackdrop,
        escapeToClose: defaultOptions.escapeToClose,
        locals: defaultOptions.locals,
        parent: defaultOptions.parent,
        controller: defaultOptions.controller,
        controllerAs: defaultOptions.controllerAs,
        bindToController: defaultOptions.bindToController,
        templateUrl: Global.clientPath + defaultOptions.templateUrl,
        clickOutsideToClose: defaultOptions.clickOutsideToClose,
        targetEvent: defaultOptions.targetEvent,
        fullscreen: defaultOptions.fullscreen
      };

    }

    function show(type, options) {
      var defaultOptions = {
        hasBackdrop: true,
        escapeToClose: false,
        bindToController: true,
        clickOutsideToClose: true,
        title: 'This is an alert title',
        textContent: 'You can specify some description text in here.',
        ariaLabel: 'Alert Dialog Demo',
        ok: 'Got it!',
        cancel: 'Cancel it!',
        fullscreen: ($mdMedia('sm') || $mdMedia('xs'))
      };

      //não fazemos merge da propriedade locals para não estourar o limite de recursividade
      if (angular.isDefined(options.locals)) {
        defaultOptions.locals = options.locals;
        delete options.locals;
      }

      angular.merge(defaultOptions, options);

      /**
       * Verifica o tipo do dialog e o exibe
       * retornando ao fechar uma promisse
       * resolvida no metodo hide() que pode
       * ou não aceitar parâmetros ou no metodo
       * cancel() que não aceita parâmetros
       */
      switch (type) {
        case 'alert':
          return $mdDialog.show(alert(defaultOptions));
        case 'confirm':
          return $mdDialog.show(confirm(defaultOptions));
        case 'prompt':
          return $mdDialog.show(prompt(defaultOptions));
        case 'custom':
          return $mdDialog.show(custom(defaultOptions));
        default:
          break;
      }
    }
  }

})();
