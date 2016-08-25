(function () {
  'use strict';

  angular.module('ngProdeb')
    .factory('PrDialog', dialogService);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function dialogService(Global, $mdMedia, $log, $mdDialog, $mdUtil, $rootScope, $animate, $document) {
    var defaultOptions = {};
    var buildCalled = false;

    return {
      build: build,
      custom: custom,
      confirm: confirm,
      close: close
    };

    /**
     * Método que configura e realiza o merge do objeto recebido via parâmetro
     * com a configuração padrão para a exibição do dialog
     * @param {object} config - Objeto contendo as configurações
     * @returns {dialogService} - Retorna o service
     */
    function build(config) {
      buildCalled = true;

      if (!angular.isObject(config)) {
        $log.error('PrDialog: Parãmentro inválido, é esperando um objeto como parãmentro.');
        return;
      }

      defaultOptions = {
        hasBackdrop: false,
        escapeToClose: false,
        bindToController: true,
        clickOutsideToClose: true,
        autoWrap: true,
        skipHide: true,
        locals: {},
        zIndex: 75,
        fullscreen: ($mdMedia('sm') || $mdMedia('xs'))
      };

      //não fazemos merge da propriedade locals para não estourar o limite de recursividade
      if (angular.isDefined(config.locals)) {
        defaultOptions.locals = config.locals;
        delete config.locals;
      }

      angular.merge(defaultOptions, config);

      return this;

    }

    /**
     * Método que exibe o dialog de confirmação na tela depois que o build e invocado
     * de uma determinada ação
     * @returns {promisse} - Retorna uma promisse que pode ou não ser resolvida
     */
    function confirm() {

      //Verifica se a função build() foi chamada
      if (!buildCalled) {
        $log.error('PrDialog: Função build() indefinida, a função build() deve ser chamada antes da função confirm().');
        return;
      };

      defaultOptions.templateUrl = Global.clientPath + '/dialog/confirm.html';
      defaultOptions.locals = {
        title: (angular.isDefined(defaultOptions.title) ? defaultOptions.title : ''),
        description: (angular.isDefined(defaultOptions.description) ? defaultOptions.description : ''),
        yesBgColor: (angular.isDefined(defaultOptions.yesBgColor) ? defaultOptions.yesBgColor : 'primary'),
        noBgColor: (angular.isDefined(defaultOptions.noBgColor) ? defaultOptions.noBgColor : 'accent')
      };
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
      defaultOptions.clickOutsideToClose = false;
      defaultOptions.hasBackdrop = true;

      buildCalled = false;

      return $mdDialog.show(defaultOptions);
    }

    /**
     * Método que exibe o dialog customizado na tela depois que o build e invocado
     * @returns {promisse} - Retorna uma promisse que pode ou não ser resolvida
     */
    function custom() {

      //Verifica se a função build() foi chamada
      if (!buildCalled) {
        $log.error('PrDialog: Função build() indefinida, a função build() deve ser chamada antes da função confirm().');
        return;
      };

      if (angular.isDefined(defaultOptions.templateUrl)) {
        defaultOptions.templateUrl = Global.clientPath + defaultOptions.templateUrl;
      } else {
        $log.error('PrDialog: templateUrl indefinido, é esperando um templateUrl como atributo.');
        return;
      }

      //Criado o backdrop manualmente para diminuir o z-index através de uma classe css
      //o z-index tem que ficar menor devido ao dialog.confirm usar o z-index original de 80
      addBackdrop();

      defaultOptions.hasBackdrop = false;

      buildCalled = false;

      return $mdDialog.show(defaultOptions);

    }

    /**
     * Método que cria o backdrop e mostra na tela com o z-index configuravél
     * para sobrepor os elementos da tela
     */
    function addBackdrop() {
      if (defaultOptions.hasBackdrop) {
        var backdrop = $mdUtil.createBackdrop($rootScope, 'md-dialog-backdrop md-opaque md-backdrop-custom');

        $animate.enter(backdrop, angular.element($document.find('body')));

        var originalOnRemoving = defaultOptions.onRemoving;

        //Método executado quando a animação de fechamento do dialog termina
        defaultOptions.onRemoving = function () {
          backdrop.remove();
          if (angular.isFunction(originalOnRemoving)) originalOnRemoving.call();
        }

        var originalOnComplete = defaultOptions.onComplete;

        //Método executado quando a animação de abertura do dialog termina
        defaultOptions.onComplete = function (scope, element) {
          var zIndex = parseInt(defaultOptions.zIndex, 10);

          angular.element($document[0].querySelector('.md-backdrop-custom')).css('z-index', zIndex);
          element.css('z-index', zIndex + 1);
          if (angular.isFunction(originalOnComplete)) originalOnComplete.call();
        }
      }
    }

    /**
     * Método que serve para fechar o dialog
     */
    function close() {
      $mdDialog.hide();
    }

  }

})();
