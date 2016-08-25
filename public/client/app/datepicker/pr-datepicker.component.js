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

            <mdp-date-picker
              ng-model="$ctrl.ngModel"
              ng-attr-mdp-placeholder="{{$ctrl.placeholderDate}}"
              {{$ctrl.name}}
              {{$ctrl.openOnClick}}
              {{$ctrl.required}}
              {{$ctrl.minDate}}
              {{$ctrl.maxDate}}
              {{$ctrl.formatDate}}
              {{$ctrl.dateFilter}}
              {{$ctrl.disabledDate}}>
            </mdp-date-picker>
            <mdp-time-picker
              ng-model="$ctrl.ngModel"
              name="{{$ctrl.name}}"
              {{$ctrl.formatTime}}
              {{$ctrl.autoSwitch}}
              {{$ctrl.disabledTime}}
              ng-if="$ctrl.withTime">
            </mdp-time-picker>`,
      require: {
        ngModel: 'ngModel'
      },
      replace: true,
      bindings: {
        ngModel: '@',
        nameDate: '@',
        nameTime: '@',
        withTime: '<',
        formatDate: '@',
        formatTime: '@',
        autoSwitch: '@',
        disabledDate: '@',
        disabledTime: '@',
        placeHolderDate: '@',
        placeHolderTime: '@',
        minDate: '@',
        maxDate: '@',
        openOnClick: '@',
        required: '@',
        dateFilter: '@'
      },
      controller: function() {
        var ctrl = this;

        ctrl.$onInit = function() {

          // Make a copy of the initial value to be able to reset it later
          ctrl.nameDate = 'name="'+ angular.isDefined(ctrl.nameDate) ? ctrl.nameDate : 'datePicker';
          ctrl.withTime = angular.isDefined(ctrl.withTime) ? ctrl.withTime : false;
          ctrl.formatDate = angular.isDefined(ctrl.formatDate) ? 'mdp-format="' + ctrl.formatDate + '"' : '';
          ctrl.placeholderDate = angular.isDefined(ctrl.placeholderDate) ? ctrl.placeholderDate : 'Data';
          ctrl.openOnClick = angular.isDefined(ctrl.openOnClick) ? 'mdp-open-on-click' : '';
          ctrl.required = angular.isDefined(ctrl.required) ? ctrl.required : '';
          ctrl.minDate = angular.isDefined(ctrl.minDate) ? 'mdp-min-date="' + ctrl.minDate  + '"' : '';
          ctrl.maxDate = angular.isDefined(ctrl.maxDate) ? 'mdp-max-date="' + ctrl.maxDate + '"' : '';
          ctrl.dateFilter = angular.isDefined(ctrl.dateFilter) ? 'mdp-date-filter="' + ctrl.dateFilter + '"' : '';
          ctrl.disabledDate = angular.isDefined(ctrl.disabledDate) ? 'mdp-disabled="' + ctrl.disabledDate + '"' : '';

          if (ctrl.withTime) {
            ctrl.nameTime = angular.isDefined(ctrl.nameTime) ? ctrl.nameTime : 'timePicker';
            ctrl.placeholderTime = angular.isDefined(ctrl.placeholderTime) ? ctrl.placeholderTime : 'Hora';
            ctrl.formatTime = angular.isDefined(ctrl.formatTime) ? 'mdp-format="' + ctrl.formatTime + '"' : '';
            ctrl.disabledTime = angular.isDefined(ctrl.disabledTime) ? 'mdp-disabled="' + ctrl.disabledTime + '"' : '';
            ctrl.autoSwitch = angular.isDefined(ctrl.autoSwitch) ? 'mdp-auto-switch=="' + ctrl.autoSwitch + '"' : '';
          }

        };

      }
    });

})();
