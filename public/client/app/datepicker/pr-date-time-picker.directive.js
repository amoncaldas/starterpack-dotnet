/*eslint-env es6*/

(function() {
  'use strict';

  /**
   * Diretiva que exibe um spinner sempre que um broadcast, manualmente, é disparado
   */
  /** @ngInject */
  angular
    .module('app')
    .directive('prDateTimePicker', function(moment) {

      function buildDatePicker(attr, openOnClick) {
        var format = attr.formatDate || 'DD/MM/YYYY';

        return `
          <mdp-date-picker
            ng-model="ctrl.model"
            mdp-placeholder="${attr.placeholderDate || ''}"
            ${openOnClick}
            ng-attr-mdp-format="${format}"
            ng-attr-mdp-min-date="${ angular.isDefined(attr.minDate) ? moment(attr.minDate, format) : '' }"
            ng-attr-mdp-max-date="${ angular.isDefined(attr.maxDate) ? moment(attr.maxDate, format) : '' }"
            ng-attr-mdp-date-filter="ctrl.dateFilter"
            ${(angular.isDefined(attr.disabledDate) ? 'mdp-disabled' : '')}>
          </mdp-date-picker>
        `
      }

      function buildTimePicker(attr, openOnClick) {
        return `
          <mdp-time-picker
            ng-if="ctrl.withTime"
            ng-model="ctrl.model"
            mdp-placeholder="${attr.placeholderTime || ''}"
            ${openOnClick}
            ng-attr-mdp-format="${attr.formatTime || 'HH:mm'}"
            ng-attr-mdp-auto-switch="ctrl.autoSwitch"
            ${(angular.isDefined(attr.disabledTime) ? 'mdp-disabled' : '')}>
          </mdp-time-picker>
        `
      }

      return {
        template: function(element, attr) {
          var openOnClick = angular.isDefined(attr.openOnClick) ? 'mdp-open-on-click' : '';

          var template = `
            <div layout="${attr.layout || 'row'}">
              ${buildDatePicker(attr, openOnClick)}
              ${buildTimePicker(attr, openOnClick)}
            </div>`

          return template;
        },
        replace: true,
        require: {
          ngModel: '^ngModel'
        },
        controller: angular.noop,
        controllerAs: 'ctrl',
        bindToController: {
          model: '=ngModel',
          layout: '=',
          openOnClick: '=?',
          withTime: '=?',
          formatDate: '=?',
          formatTime: '=?',
          autoSwitch: '=?',
          disabledDate: '=?',
          disabledTime: '=?',
          minDate: '=?',
          maxDate: '=?',
          required: '=?',
          dateFilter: '=?'
        },
        scope: {
        }
      }
    });
})();
