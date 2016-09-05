'use strict';

var path = require('path');
var slash = require('slash');
var chalk = require('chalk');
var Promise = require('promise');

/**
 * Turn str into simplest form, remove trailing slash
 * example:
 * './path/to//some/folder/' (unix) will be normalized to 'path/to/some/folder'
 * 'path\\to\\some\\folder\\' (windows) will be normalized to 'path/to/some/folder'
 * @param  {String} str, can be unix style path ('/') or windows style path ('\\')
 * @return {String} normalized unix style path
 */
function normalizePath(path, str) {
  var trailingSlash;
  if (path.sep === '/') {
    trailingSlash = new RegExp(path.sep + '$');
  } else {
    trailingSlash = new RegExp(path.sep + path.sep + '$');
  }
  return slash(path.normalize(str).replace(trailingSlash, ''));
}

/**
 * Formata o nome do recurso para os parâmetros esperados
 * nos templates
 * @param {String} resource, Nome do recurso a ser formatado
 * @return {Object} retorna um objeto de contexto para ser usado nos templates
 */
function formatNameResource(resource) {

  var resourceNameLowerCase = resource.toLowerCase();
  var char = resourceNameLowerCase.charAt(0).toUpperCase();
  var nameFistCharUpperCase = char + resourceNameLowerCase.substr(1);

  var context = {
      resource_name: resourceNameLowerCase,
      name_uppercase: nameFistCharUpperCase,
      alias_controller: resourceNameLowerCase + "Ctrl",
  };

  return context;

}

/**
 * Desenha o logo no console
 * @return {String} retorna uma string personalizada da logo
 */
function logoProdeb() {
  var prodeb =
    chalk.blue.bold("                                             ,,,,,,;;;;;;;;;;;;:") + '\n' +
    chalk.blue.bold("                                                ´´´´´´;;;;;;;;;;") + '\n' +
    chalk.blue("  ++++++'                                        ") + chalk.red.bold("`''''  ") + chalk.blue.bold(";;;;;;;") + '\n' +
    chalk.blue("  ++++++++                                          ") + chalk.red.bold(";''; ") + chalk.blue.bold("::;;;;") + '\n' +
    chalk.blue("  ++' `+++`                                          ") + chalk.red.bold("`''; ") + chalk.blue.bold(";;;;") + '\n' +
    chalk.blue(" `++,  .++, +++++.    :+++.    ;+++.   ;;;;;;  ;;;;:   ") + chalk.red.bold("'. ") + chalk.blue.bold(";;;") + '\n' +
    chalk.blue(" :++   ,++` ++++++.  ++++++   ++++++`  ++++++  ++++++  ") + chalk.red.bold(".'' ") + chalk.blue.bold(":;") + '\n' +
    chalk.blue(" +++   +++` ++  ++'  ++  ++:  ++  ++'  ++      ++  ++,  ") + chalk.red.bold("'' ") + chalk.blue.bold(":") + '\n' +
    chalk.blue(" +++;;+++; ,++  '+: '+;  ;+; `++  ,++  ++     .++  ++`  ") + chalk.red.bold("''") + '\n' +
    chalk.blue(" +++++++;  '+,  ++  ++   ++: ;+;  :+' ,+++++  ++; .++   ") + chalk.red.bold(";") + '\n' +
    chalk.blue(" +++```    +++++'   ++   ++. ++,  ++, ;+++++  +++++'") + '\n' +
    chalk.blue(" ++:       ++++++;  ++   ++  ++   ++` ++++++  ++  '++") + '\n' +
    chalk.blue(",++`       ++  +++  ++   ++  ++   ++  ++`     ++   ++") + '\n' +
    chalk.blue("'++        ++  +++  ++  ,++  ++  ,++  ++      ++  ,++") + '\n' +
    chalk.blue("+++       ,++  ++:  ++,.++  `++:;++   ++++++. ++++++,") + '\n' +
    chalk.blue("+++       ;+'  ++:   ++++    +++++   `++++++: +++++:") + '\n' +

    chalk.white.bgBlue('Você está usando o gerador automático de estrutura de arquivos.');

  return prodeb;
}

/**
 * Valida se o nome do recurso foi digitado
 * @params {String, String} path, value - Caminho do arquivo e nome do arquivo
 * @return {boolean} Retorna true se o nome foi digitado
 */
function verifyInput(value) {
  if (value === undefined || value === '') {
    return chalk.red('O nome do recurso é obrigratório.');
  }
  return true;
}

module.exports = {
  normalizePath: normalizePath,
  formatNameResource: formatNameResource,
  verifyInput: verifyInput,
  logoProdeb: logoProdeb
}
