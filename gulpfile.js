var argv = require('minimist')(process.argv.slice(2));
var IS_RELEASE_BUILD = argv._.indexOf('release') !== -1 || !!argv.release;
var gulp = require('gulp');
var msbuild = require('gulp-msbuild');
var jshint = require('gulp-jshint');
var gulpif = require('gulp-if');
var uglify = require('gulp-uglify');
var imagemin = require('gulp-imagemin');
var minifycss = require('gulp-minify-css');
var htmlmin = require('gulp-htmlmin');

gulp.task('compile', function() {
    return gulp.src('EU4.Savegame.sln')
        .pipe(msbuild({
            stdout: true,
            targets: ['Build']
        }));
});

gulp.task('backend', ['compile'], function() {
    return gulp.src('EU4.Stats.Web/server/bin/*')
        .pipe(gulp.dest('bin/bin/.'));
});

gulp.task('minifyhtml', ['backend'], function() {
    return gulp.src('bin/bin/template.html')
        .pipe(htmlmin({
            removeComments: true,
            collapseWhitespace: true,
            collapseBooleanAttributes: true,
            removeAttributeQuotes: true,
            removeAttributeQuotes: true,
            useShortDoctype: true,
            removeEmptyAttributes: true,
            removeOptionalTags: true
        }))
        .pipe(gulp.dest('bin/bin/.'));
});

gulp.task('js', function() {
    return gulp.src('EU4.Stats.Web/client/js/*')
        .pipe(jshint())
        .pipe(jshint.reporter('fail'))
        .pipe(gulpif(IS_RELEASE_BUILD, uglify()))
        .pipe(gulp.dest('bin/js/.'));
});

gulp.task('assets', function() {
    return gulp.src(['EU4.Stats.Web/client/index.html',
                     'EU4.Stats.Web/client/favicon.ico'])
        .pipe(gulp.dest('bin/.'));
});

gulp.task('images', function() {
    return gulp.src('EU4.Stats.Web/client/img/*')
        .pipe(gulpif(IS_RELEASE_BUILD, imagemin()))
        .pipe(gulp.dest('bin/img/.'));
});

gulp.task('css', function() {
    return gulp.src('EU4.Stats.Web/client/css/*')
        .pipe(gulpif(IS_RELEASE_BUILD, minifycss()))
        .pipe(gulp.dest('bin/css/.'));
});

gulp.task('frontend', [
    'js',
    'assets',
    'images',
    'css'
]);

gulp.task('default', ['frontend']);

gulp.task('release', [
    'frontend',
    'backend',
    'minifyhtml'
]);
