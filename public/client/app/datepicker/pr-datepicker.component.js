/*eslint-env es6*/

(function() {
  'use strict';

  /**
   * Diretiva que exibe um spinner sempre que um broadcast, manualmente, é disparado
   */
  /** @ngInject */
  angular
    .module('app')
    .component('prDatepicker', {
      template: `
            <section id="spin-label-component" layout="row" ng-show="$ctrl.spinner && $ctrl.spinner.show">
              <span flex></span>
              <md-toolbar  class="md-whiteframe-z1" md-colors="::{background:'{{$ctrl.bgColor}}'}">
                <div class="md-toolbar-tools">
                  <span md-colors="::{color:'{{$ctrl.textColor}}'}">{{$ctrl.spinner.message}}</span>
                  <md-progress-circular md-mode="indeterminate" class="md-primary" md-diameter="15px"></md-progress-circular>
                </div>
              </md-toolbar>
            <section>
        `,
      bindings: {
        bgColor: '@',
        textColor: '@'
      },
      controller: function($scope) {
        var ctrl = this;

        ctrl.$onInit = function() {
          // Make a copy of the initial value to be able to reset it later
          ctrl.bgColor = angular.isDefined(ctrl.bgColor) ? ctrl.bgColor : 'accent';
          ctrl.textColor = angular.isDefined(ctrl.textColor) ? ctrl.textColor : 'primary';
        };
          //comportamento padrão
        ctrl.spinner = { show: false, message: '' };

          //Escuta as mensagens em broadcast emitidas as chaves a seguir
          //para exibir/esconder o componente
        $scope.$on('show-spinner', function(event, args) {
          ctrl.spinner = { show: true, message: args.message };
        });

        $scope.$on('hide-spinner', function() {
          ctrl.spinner = { show: false, message: '' };
        });
      }
  });

})();
