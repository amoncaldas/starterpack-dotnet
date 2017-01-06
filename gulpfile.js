/*eslint strict: 0, */
/*global require*/

var paths = require('./paths.json');
var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var sass = require('gulp-sass');
var babel = require('gulp-babel');
var changed = require('gulp-changed');
var sourcemaps = require('gulp-sourcemaps');
var plumber = require('gulp-plumber');
var ngAnnotate = require('gulp-ng-annotate');
var gulpif = require('gulp-if');
var cleanCSS = require('gulp-clean-css');
var inject = require('gulp-inject');
var lazypipe = require('lazypipe');
var browserSync = require('browser-sync');
var gutil = require('gulp-util');
var eslint = require('gulp-eslint');
var runSequence = require('run-sequence');
var argv = require('yargs').argv;

paths.client = '.';
paths.bower = 'bower_components';

paths.app = paths.client + '/app';
paths.destination = paths.client + '/build';
paths.angularScripts = [
  paths.bower + '/deep-diff/index.js',
  paths.bower + '/uri-templates/uri-templates.js',
  paths.bower + '/angular/angular.js',
  paths.bower + '/angular-aria/angular-aria.js',
  paths.bower + '/angular-sanitize/angular-sanitize.js',
  paths.bower + '/angular-animate/angular-animate.js',
  paths.bower + '/angular-resource/angular-resource.min.js',
  paths.bower + '/angular-translate/angular-translate.min.js',
  paths.bower + '/satellizer/satellizer.min.js',
  paths.bower + '/angular-i18n/angular-locale_pt-br.js',
  paths.bower + '/angular-input-masks/angular-input-masks-standalone.min.js',
  paths.bower + '/angular-ui-router/release/angular-ui-router.min.js',
  paths.bower + '/angular-material/angular-material.js',
  paths.bower + '/angular-material-data-table/dist/md-data-table.min.js',
  paths.bower + '/angular-model-factory/dist/angular-model-factory.js',
  paths.bower + '/angular-file-upload/dist/angular-file-upload.js',
  paths.bower + '/mdPickers/dist/mdPickers.min.js',
  paths.bower + '/ng-prodeb/dist/ng-prodeb.min.js'
];
paths.vendorsScripts = [
  paths.bower + '/lodash/dist/lodash.min.js',
  paths.bower + '/moment/min/moment.min.js',
  paths.bower + '/moment/min/locales.min.js'
];
paths.scripts = [
  paths.app + '/app.js',
  paths.app + '/app.*.js',
  paths.app + '/**/*.js'
];
paths.styles = [
  paths.bower   + '/ng-prodeb/dist/ng-prodeb.css',
  paths.bower   + '/angular-material-data-table/dist/md-data-table.min.css',
  paths.bower   + '/mdPickers/dist/mdPickers.min.css',
  paths.client  + '/styles/app.scss'
];

var filesNames = {
  vendors: (argv.production) ? 'vendors.min.js' : 'vendors.js',
  angular: (argv.production) ? 'angular-with-plugins.min.js' : 'angular-with-plugins.js',
  application: (argv.production) ? 'application.min.js' : 'application.js'
}

var globalRandom = Math.random().toString(36).substr(2, 15);

var minifierJSChannel = lazypipe()
  .pipe(uglify);

var minifierCSSChannel = lazypipe()
  .pipe(cleanCSS);

function scriptsVendors() {
  return gulp.src(paths.vendorsScripts)
    .pipe(plumber())
    .pipe(sourcemaps.init())
    .pipe(concat(filesNames.vendors))
    .pipe(gulp.dest(paths.destination))
    .pipe(gulpif(argv.production, minifierJSChannel()))
    .pipe(sourcemaps.write())
    .pipe(gulp.dest(paths.destination));
};

function scriptsAngular() {
  return gulp.src(paths.angularScripts)
    .pipe(plumber())
    .pipe(sourcemaps.init())
    .pipe(concat(filesNames.angular))
    .pipe(gulp.dest(paths.destination))
    .pipe(gulpif(argv.production, minifierJSChannel()))
    .pipe(sourcemaps.write())
    .pipe(gulp.dest(paths.destination));
};

function scriptsApplication() {
  var stream = gulp.src(paths.scripts)
    .pipe(plumber())
    .pipe(sourcemaps.init())
    .pipe(changed(paths.destination))
    .pipe(babel({
      presets: ['es2015']
    }))
    .pipe(concat(filesNames.application))
    .pipe(ngAnnotate({
      add: true
    }))
    .pipe(gulp.dest(paths.destination))
    .pipe(gulpif(argv.production, minifierJSChannel()))
    .pipe(sourcemaps.write())
    .pipe(gulp.dest(paths.destination));

  return stream;
};

function injectFiles() {
  var random = Math.random().toString(36).substr(2, 15);

  gulp.src(paths.client + '/index.html')
    .pipe(inject(gulp.src([
      paths.destination + '/' + filesNames.application
    ], { read: false }), {
      starttag: '<!-- inject:application:script -->',
      endtag: '<!-- end:inject:application:script -->',
      ignorePath: 'public',
      addRootSlash: false,
      addPrefix: paths.serverClientPath,
      addSuffix: '?version=' + random
    }))
    .pipe(inject(gulp.src([
      paths.destination + '/*.css'
    ], { read: false }), {
      starttag: '<!-- inject:all:css -->',
      endtag: '<!-- end:inject:all:css -->',
      ignorePath: 'public',
      addRootSlash: false,
      addPrefix: paths.serverClientPath,
      addSuffix: '?version=' + globalRandom
    }))
    .pipe(inject(gulp.src([
      paths.destination + '/*.js',
      '!' + paths.destination + '/' + filesNames.application
    ], { read: false }), {
      starttag: '<!-- inject:vendors:script -->',
      endtag: '<!-- end:inject:vendors:script -->',
      ignorePath: 'public',
      addRootSlash: false,
      addPrefix: paths.serverClientPath,
      addSuffix: '?version=' + globalRandom
    }))
    .pipe(gulp.dest(paths.client));
}

function styles() {
  return gulp.src(paths.styles)
    .pipe(plumber())
    .pipe(sourcemaps.init())
    .pipe(changed(paths.destination))
    .pipe(sass())
    .pipe(concat('application.css'))
    .pipe(gulpif(argv.production, minifierCSSChannel()))
    .pipe(sourcemaps.write())
    .pipe(gulp.dest(paths.destination));
};

gulp.task('scriptsVendors', scriptsVendors);
gulp.task('scriptsAngular', scriptsAngular);
gulp.task('scriptsApplication', scriptsApplication);
gulp.task('styles', styles);
gulp.task('injectFiles', injectFiles);

/**
 * Task to sync the browser with changes in the
 * source code.
 */
gulp.task('browser-sync', function() {
  if (argv.sync && !argv.production) {
    browserSync({
      port: 5005
    });
  }
});

// Rerun the task when a file changes
gulp.task('watch', function() {
  if (!argv.production) {
    gulp.watch(paths.scripts, ['scriptsApplication', 'injectFiles']).on('change', browserSync.reload);
    gulp.watch(paths.app + '/**/*.html').on('change', browserSync.reload);
    gulp.watch(paths.styles, ['styles']).on('change', browserSync.reload);
  }
});

/**
 * Build js files and inject into index.html
 */
gulp.task('build', function() {
  runSequence(['styles', 'scriptsVendors', 'scriptsAngular', 'scriptsApplication'], 'injectFiles');
});

/**
 * Check all .js files using eslint
 * --fix can be passed to fix possible problems
 */
gulp.task('check', function() {
  gutil.log(gutil.colors.blue('Executando a analise do eslint'));

  return gulp.src([paths.app + '/*.js', paths.app + '/**/*.js'])
    .pipe(eslint({
      fix: ((argv.fix) ? true : false)
    }))
    .pipe(eslint.format());
});

gulp.task('default', ['browser-sync', 'watch', 'build'], function() {});
gulp.task('minifier', ['build-production'], function() {});
