/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    rename = require("gulp-rename"),
    sourcemaps = require('gulp-sourcemaps'),
    plumber = require('gulp-plumber'),
    autoprefixer = require('gulp-autoprefixer'),
    gulpSequence = require('gulp-sequence');

var paths = {
    webroot: "./wwwroot/"
};

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/{site.css,pace.css}";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDestMin = paths.webroot + "css/cssbundle.min.css";
paths.concatCssDest = paths.webroot + "css/cssbundle.css";
paths.allCssBundles = paths.webroot + "css/cssbundle.*";
paths.bower = './bower_components/';
paths.lib = './' + paths.webroot + 'lib/';


gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) { 
    rimraf(paths.allCssBundles, cb);
});

gulp.task("clean:lib", function (cb) {
    rimraf(paths.lib + '**/*', cb);
});

gulp.task("clean", ["clean:js", "clean:css", "clean:lib"]);

gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], {base: "." })
      .pipe(plumber())
      .pipe(sourcemaps.init())
      .pipe(concat(paths.concatJsDest))
      .pipe(uglify())
      .pipe(sourcemaps.write('.'))
      .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss], {base: "." })
      .pipe(plumber())
      .pipe(sourcemaps.init())
      .pipe(autoprefixer())
      .pipe(concat(paths.concatCssDestMin))
      .pipe(cssmin())
      .pipe(sourcemaps.write('.'))
      .pipe(gulp.dest("."));
});

gulp.task("regular:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss], { base: "." })
      .pipe(plumber())      
      .pipe(autoprefixer())
      .pipe(concat(paths.concatCssDest))            
      .pipe(gulp.dest("."));
});

gulp.task("copy:bower", function () {
    
    return gulp.src([
            paths.bower + "bootstrap/dist/**/*.{js,map,css,ttf,svg,woff,eot}*", "!**/npm.js",
            paths.bower + "flexslider/*flexslider*.{css,js,map}",
            paths.bower + "flexslider/fonts/*",
            paths.bower + "flexslider/images/*",
            paths.bower + "font-awesome/css/*",
            paths.bower + "font-awesome/fonts/*",
            paths.bower + "gmaps/gmaps*.{js,map}",
            paths.bower + "html5shiv/dist/*",
            paths.bower + "jquery/dist/jquery.*{js,map}", "!**/jquery.slim.*",
            paths.bower + "jquery-validation/dist/jquery.validate.*{js,map}",
            paths.bower + "jquery.nicescroll/dist/*",
            paths.bower + "pace/*.js",
            paths.bower + "respond/dest/respond.min.js",
            paths.bower + "respond/dest/respond.src.js"
            //paths.bower + "*.{css,js}"
    ], { base: paths.bower })

            .pipe(gulp.dest(paths.lib));

});

/*gulp.task("min:ie8", function () {
    return gulp.src([paths.webroot + 'lib/ie_8.css'])
      .pipe(rename(paths.webroot + 'lib/ie_8.min.css'))
      .pipe(cssmin()      )
      .pipe(gulp.dest("."));
});*/

//gulp.task("deploy", ["min:js", "min:css", "copy:bower"]);


gulp.task("Debug", gulpSequence("clean", "regular:css", "copy:bower"));
gulp.task("Release", gulpSequence("clean", "min:js", "min:css", "copy:bower"));
gulp.task("deploy", gulpSequence("clean", "min:js", "min:css", "copy:bower"));

