# Front-End Build Process

## Prerequisites:
 - Node (v6 LTS will work just fine)

## Description

NPM is used for managing all front-end packages. Gulp is used to build both the SASS and the
Typescript. Neither Gulp nor Typescript is needed globally but will be installed when installing all dependencies:

```Powershell
> npm install
```

Since Gulp isn't installed globally we use NPM scripts to run different Gulp tasks:

```Powershell
> npm run build:css   # Will build all SASS
> npm run build:js    # Will build all Typescript
> npm run build:watch # Will build both SASS and Typescript and will rebuild on changes
> npm run build       # Will build both SASS and Typescript and then exit Gulp.
```

**Note:** The task for building the Typescript will start from 'main.ts' so everything must be import there in order to end up in the bundle.
It's really simple to import packages that is added to `package.json`:

```Javascript
import 'lazysizes'
```

**NOTE:** If you are using Visual Studio all tasks can be run from the `Task Runner Explorer`. When opening up the solution the `watch`-task will start to run automatically.
