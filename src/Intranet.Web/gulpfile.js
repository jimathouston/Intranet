/// <binding BeforeBuild='default' />
'use strict'

var gulp = require('gulp')
var browserify = require('browserify')
var source = require('vinyl-source-stream')
var tsify = require('tsify')
var uglify = require('gulp-uglify')
var sourcemaps = require('gulp-sourcemaps')
var buffer = require('vinyl-buffer')
var del = require('del')
var sass = require('gulp-sass')

gulp.task('clean-typescript', function () {
    return del([
        'Assets/scripts/**/*.js',
        'Assets/scripts/**/*.map'
    ])
})

gulp.task('clean-sass', function () {
    // Don't remove TinyMCE skins!
    return del([
        'Assets/css/*.css'
    ])
})

gulp.task('sass', ['clean-sass'], function () {
    return gulp.src(['scss/main.scss', 'scss/login.scss', 'scss/materialize.scss'])
        .pipe(sourcemaps.init({ loadMaps: true }))
        .pipe(sass({ outputStyle: 'compressed' }))
        .pipe(sourcemaps.write())
        .pipe(gulp.dest('Assets/css'))
})

gulp.task('typescript', ['clean-typescript'], function () {
    return browserify({
        basedir: '.',
        debug: true,
        entries: ['scripts/main.ts'],
        cache: {},
        packageCache: {}
    })
        .plugin(tsify)
        .bundle()
        .pipe(source('bundle.min.js'))
        .pipe(buffer())
        .pipe(sourcemaps.init({ loadMaps: true }))
        .pipe(uglify())
        .pipe(sourcemaps.write('./'))
        .pipe(gulp.dest('Assets/scripts'))
})

gulp.task('default', ['typescript', 'sass'])

gulp.task('watch', function () {
    gulp.watch('scripts/**/*.ts', ['typescript'])
    gulp.watch('scripts/**/*.js', ['typescript'])
    gulp.watch('scss/**/*.scss', ['sass'])
})