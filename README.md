# Intranet

Certaincy's intranet, built with .Net Core and React/Redux

## Development

### Client App

_Note: You must be in `src/Intranet.Web` to use the npm scripts._

#### Lint

The client app can be linted by running:

```
> npm run lint
```

Some linting issues can be fixed automatically by `tslint`:

```
> npm run lint:fix
```

#### Unit Testing

Jest is used for unit testing.

Run the unit tests with:

```
> npm test
```

You can also run Jest in watch-mode by running:

```
> npm run test:watch
```

#### Test Helper

The test helper for React snapshot testing from [tscomp](https://github.com/beanloop/tscomp) is included:

```
import * as React from 'react'
import {snap} from './testHelpers'
import Component from '../component'

describe('component', () => {
  it('works', () => {
    snap(<Component />)
  })
})
```
