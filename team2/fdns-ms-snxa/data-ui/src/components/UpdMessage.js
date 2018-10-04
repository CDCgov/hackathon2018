import React, { Component } from 'react'
import styled from 'styled-components'


const StyledDiv = styled.div`
  border-radius:1px;
  margin:14px;
  padding:7px;
  color: #2d2e2e;
  background-color: ${props => props.color};
`

class UpdMessage extends Component {

  render() {
    let userMessage = this.props.addedOneRowErr?'Error Uploading File':'File Uploded Success'
    let backColor = this.props.addedOneRowErr?'palevioletred':'palegreen'
    return (
      <StyledDiv color={backColor}>{userMessage}</StyledDiv>
    )

  }// .render

}// ./UpdMessage

export default UpdMessage
