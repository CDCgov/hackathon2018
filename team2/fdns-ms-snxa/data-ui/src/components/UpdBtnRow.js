import React, { Component } from 'react'

import UpdButton from './UpdButton'
import UpdMessage from './UpdMessage'
import Spinner from './Spinner'


class UploadBtnRow extends Component {


  render() {
    return (
      <div className="row" style={{position: "relative"}}>
        <UpdButton updBtnClick={this.props.updBtnClick} uploadDisabled={this.props.uploadDisabled} />
        { this.props.addedOneRow? <UpdMessage addedOneRowErr={this.props.addedOneRowErr}/> : null}
        { this.props.uploadDisabled? <Spinner /> : null }
      </div>
    )
  }// ./render
  
}// ./UploadBtnRow

export default UploadBtnRow
