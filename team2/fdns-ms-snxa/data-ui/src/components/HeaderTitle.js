import React, { Component } from 'react'
import styled from 'styled-components'


const StyledHeaderTitle = styled.p`
  margin: 0;
  padding-left: 6px;
  text-transform: uppercase;
`

class HeaderTitle extends Component {

  render() {
    return (
      <StyledHeaderTitle className="mr-auto p-2">{this.props.HEADER_TITLE}</StyledHeaderTitle>
      )
  }
}// ./HeaderTitle

export default HeaderTitle


