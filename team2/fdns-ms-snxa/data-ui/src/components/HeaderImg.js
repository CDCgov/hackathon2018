import React, { Component } from 'react'

import Img from '../img/cdcimg.png'


class HeaderImg extends Component {

  render() {
    return (
      <img src={Img} alt='CDC logo' style={{width:'73px',height:'43px'}} />
      )
  }// ./render
}// ./HeaderImg

export default HeaderImg


