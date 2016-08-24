(function () {
  'use strict';

  angular.module('ngProdeb')
    .factory('PrDialog', dialogService);

  /** @ngInject */
  // eslint-disable-next-line max-params
  function dialogService(Global, $mdMedia, $mdPanel, $q, $log) {

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
      if (angular.isUndefined(config.options)) config.options = {};
      var defaultOptions = build(config.options);

      defaultOptions.locals.attrs = {
        title: (angular.isDefined(config.title) ? config.title : 'Title não declarado'),
        description: (angular.isDefined(config.description) ? config.description : 'Description não declarado'),
        yesAction: (angular.isDefined(config.yesAction) ? config.yesAction : null),
        noAction: (angular.isDefined(config.noAction) ? config.noAction : null)
      };

      /** @ngInject */
      defaultOptions.controller = function(mdPanelRef, attrs) {
        var vm = this;

        vm.attrs = attrs;
        vm._mdPanelRef = mdPanelRef;

        vm.closeDialog = function(yes) {
          close(this._mdPanelRef).then(function() {
            if (yes && attrs.yesAction !== null) attrs.yesAction();
            if (!yes && attrs.noAction !== null) attrs.noAction();
          });
        }
      };
      defaultOptions.locals = defaultOptions.locals;
      defaultOptions.controllerAs = 'ctrl';
      defaultOptions.templateUrl = Global.clientPath + '/dialog/confirm.html';
      defaultOptions.clickOutsideToClose = false;
      defaultOptions.zIndex = 90;

      return $mdPanel.open(defaultOptions);
    }

    function close(panelRef) {
      var deferred = $q.defer();

      panelRef && panelRef.close().then(function() {
        panelRef.destroy();

        return deferred.resolve();
      });

      return deferred.promise;
    }

    function build(options) {
      var defaultOptions = {
        hasBackdrop: true,
        escapeToClose: false,
        bindToController: true,
        clickOutsideToClose: true,
        focusOnOpen: true,
        locals: {},
        fullscreen: ($mdMedia('sm') || $mdMedia('xs'))
      };

      //não fazemos merge da propriedade locals para não estourar o limite de recursividade
      if (angular.isDefined(options.locals)) {
        defaultOptions.locals = options.locals;
        delete options.locals;
      }

      angular.merge(defaultOptions, options);

      if (angular.isUndefined(defaultOptions.position)) {
        var position = $mdPanel.newPanelPosition()
          .centerHorizontally();

        defaultOptions.position = position;
      }

      return defaultOptions;
    }

    /**
     * Método que exibe o modal na tela
     * @param {object} options - Objeto contendo as demais configurações
     * @returns {promisse} Retorna uma promisse para ser resolvida
     */
    function custom(options) {
      var defaultOptions = build(options);

      if (angular.isDefined(defaultOptions.templateUrl)) {
        defaultOptions.templateUrl = Global.clientPath + defaultOptions.templateUrl;
      }

      return $mdPanel.open(defaultOptions);
    }

  }

})();
