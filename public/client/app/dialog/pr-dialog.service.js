(function () {
  'use strict';

  angular.module('ngProdeb')
    .factory('PrDialog', dialogService);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function dialogService(Global, $mdMedia, $log, $mdDialog, $mdUtil, $rootScope, $animate, $document) {

    return {
      custom: custom,
      confirm: confirm,
      close: close
    };

    function confirm(config) {
      if (!angular.isObject(config)) {
        $log.error('Parãmentro inválido: é esperando um objeto como parãmentro.');
        return;
      }

      config.templateUrl = Global.clientPath + '/dialog/confirm.html';
      var defaultOptions = build(config);

      defaultOptions.locals = {
        title: (angular.isDefined(config.title) ? config.title : 'Title não declarado'),
        description: (angular.isDefined(config.description) ? config.description : 'Description não declarado'),
        yesBgColor: (angular.isDefined(config.yesBgColor) ? config.yesBgColor : 'primary'),
        noBgColor: (angular.isDefined(config.noBgColor) ? config.noBgColor : 'accent')
      };
      defaultOptions.clickOutsideToClose = false;
      defaultOptions.controller = function($mdDialog) {
        var vm = this;

        vm.noAction = vm.noAction;
        vm.yesAction = vm.yesAction;

        vm.noAction = function() {
          $mdDialog.cancel();
        }
        vm.yesAction = function() {
          $mdDialog.hide();
        }
      };
      defaultOptions.controllerAs = 'ctrl';
      defaultOptions.hasBackdrop = true;

      return $mdDialog.show(defaultOptions);
    }

    function build(options) {
      var defaultOptions = {
        hasBackdrop: false,
        escapeToClose: false,
        bindToController: true,
        clickOutsideToClose: true,
        autoWrap: true,
        skipHide: true,
        locals: {},
        fullscreen: ($mdMedia('sm') || $mdMedia('xs'))
      };

      //não fazemos merge da propriedade locals para não estourar o limite de recursividade
      if (angular.isDefined(options.locals)) {
        defaultOptions.locals = options.locals;
        delete options.locals;
      }

      angular.merge(defaultOptions, options);

      return defaultOptions;
    }

    /**
     * Método que exibe o modal na tela
     * @param {object} options - Objeto contendo as demais configurações
     * @returns {promisse} Retorna uma promisse para ser resolvida
     */
    function custom(config) {
      if (!angular.isObject(config)) {
        $log.error('Parãmentro inválido: é esperando um objeto como parãmentro.');
        return;
      }

      var defaultOptions = build(config);

      if (angular.isDefined(defaultOptions.templateUrl)) {
        defaultOptions.templateUrl = Global.clientPath + defaultOptions.templateUrl;
      } else {
        $log.error('templateUrl indefinido: é esperando um templateUrl como atributo.');
        return;
      }

      //Criado o backdrop manualmente para diminuir o z-index através de uma classe css
      //o z-index tem que ficar menor devido ao dialog.confirm usar o z-index original de 80
      if (defaultOptions.hasBackdrop) {
        var backdrop = $mdUtil.createBackdrop($rootScope, 'md-dialog-backdrop md-opaque md-backdrop-custom');

        $animate.enter(backdrop, angular.element($document.find('body')));

        defaultOptions.onRemoving = function () {
          backdrop.remove();
        }

        defaultOptions.onComplete = function (scope, element) {
          angular.element(document.querySelector('.md-backdrop-custom')).addClass('md-dialog-backdrop-custom');
          element.addClass('md-dialog-container-custom');
        }
      }

      defaultOptions.hasBackdrop = false;

      return $mdDialog.show(defaultOptions);

    }

    function close(response) {
      if (angular.isDefined(response)) {
        $mdDialog.hide(response);
      } else {
        $mdDialog.cancel();
      }
    }

  }

})();
