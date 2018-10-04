import React, { Component } from 'react'
import styled from 'styled-components'


const StyledUpdButton = styled.button.attrs({
  type: "file"
})`
  margin:10px;
  margin-bottom:15px;

  border-radius:1px;
  padding: 10px 20px;
  border: 0 none;
  font-weight: lighter;
  letter-spacing: 0.90px;
  text-transform: uppercase;
`

class UpdButton extends Component {

  render() {
    return (
      <StyledUpdButton disabled={this.props.uploadDisabled? 'disabled':null} onClick={this.props.updBtnClick} className="btn btn-primary">UPLOAD NEW DATA</StyledUpdButton>
    )
  }
}// ./UpdButton

export default UpdButton