/// <reference types="jest" />

import { ReactElement } from 'react'
import * as ReactTestRenderer from 'react-test-renderer'

export const snap = (children: ReactElement<any>) => {
  const component = ReactTestRenderer.create(children)
  expect(component).toMatchSnapshot()
}
