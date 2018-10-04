import React, { Component } from 'react'
import styled from 'styled-components'

import HeaderTitle from './HeaderTitle'
import HeaderImg from './HeaderImg'
import HeaderBtnLogOut from './HeaderBtnLogOut'


const StyledHeaderDiv = styled.div`
  background-color: #0F538E;
  color: white;
  padding: 2px;
  font-size: 1.1rem;
  width: 100%;
`

class HeaderRow extends Component {

  render() {
    return (
      <div className="row">

        <StyledHeaderDiv className="d-flex">

          <HeaderImg />
          <HeaderTitle HEADER_TITLE={this.props.HEADER_TITLE}/>


          <HeaderBtnLogOut />

        </StyledHeaderDiv>

      </div>

      )
  }// ./render

}// ./HeaderRow

export default HeaderRow

