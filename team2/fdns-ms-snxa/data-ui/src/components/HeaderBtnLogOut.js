import React, { Component } from 'react'
import styled from 'styled-components'

const StyledHeaderBtnLogOut = styled.button`
  border-radius:1px;
`

class HeaderBtnLogOut extends Component {

  handleClick = () => {
    
      window.location.href = '/'
  }// ./handleClick

  render() {
    return (
      <StyledHeaderBtnLogOut onClick={this.handleClick} className="btn btn-dark btn-sm p-2">LOG OUT</StyledHeaderBtnLogOut>
      )
  }
}// ./HeaderBtnLogOut

export default HeaderBtnLogOut

