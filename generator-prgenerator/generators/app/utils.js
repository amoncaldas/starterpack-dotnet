'use strict';

var path = require('path');
var slash = require('slash');
var chalk = require('chalk');
var pathExists = require('path-exists');

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

/**
 * Válida se o diretório ou o arquivo já existe
 * @params {String, String, String} path, value, structure - Caminho do arquivo ou diretório
 * nome do recurso e estrutura a ser criada
 */
function verifyDirAndFile(path, value, structure) {
  var response = {path: '', message: ''};

  switch(structure) {
    case 'complete':
      response.path = path + value;
      response.message = 'Diretório do recurso já existe, tente novamente.';
    break;
    case 'html':
      response.path = path + value + '/' + value + '.html';
      response.message = 'Diretório do recurso já existe e contém os arquivos html, tente novamente.';
    break;
    case 'controller':
      response.path = path + value + '/' + value + '.controller.js';
      response.message = 'Diretório do recurso já existe e contém o controller, tente novamente.';
    break;
    case 'service':
      response.path = path + value + '/' + value + '.service.js';
      response.message = 'Diretório do recurso já existe e contém o service, tente novamente.';
    break;
    case 'route':
      response.path = path + value + '/' + value + '.route.js';
      response.message = 'Diretório do recurso já existe e contém o route, tente novamente.';
    break;
  }

  return pathExists(response.path).then(exists => {
    if (exists) {
      return chalk.red(response.message);
    } else {
      return true;
    }
  });

}


module.exports = {
  normalizePath: normalizePath,
  formatNameResource: formatNameResource,
  verifyDirAndFile: verifyDirAndFile,
  verifyInput: verifyInput,
  logoProdeb: logoProdeb
}
