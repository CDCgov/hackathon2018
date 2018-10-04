import React, { Component } from 'react'
import styled from 'styled-components'
import isofetch from 'isomorphic-fetch'
import { HOST } from '../Env.js'


const StyledBtnSharp = styled.button`
  border-radius:1px;
`

class UpdsTRow extends Component {


  render() {

    return (
 
      <tr>
        <td>{this.props.el.grantee}</td>
        <td>{this.props.el.updfilename}</td>
        <td>{this.props.el.updemail}</td>
        <td>{this.timeConverter(this.props.el.updservertime)}</td>

        <td><StyledBtnSharp className="btn btn-outline-info btn-sm">DATA PROFILER</StyledBtnSharp></td>
      </tr>
  
    )

  }// .render

}// ./UpdsTRow

export default UpdsTRow
