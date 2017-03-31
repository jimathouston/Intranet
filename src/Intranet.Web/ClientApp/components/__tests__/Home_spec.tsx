import * as React from 'react'
import {snap} from '../../testHelpers'

import Home from '../Home'

describe('Home', () => {
  it('should render with a default message', () => {
    snap(<Home />)
  })
})
