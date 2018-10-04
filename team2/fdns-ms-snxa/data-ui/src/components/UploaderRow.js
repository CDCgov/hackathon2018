import React, { Component } from 'react'

import Uploader from './Uploader';


class UploaderRow extends Component {

  render() {
    return (
      <div className="row" style={{position: "relative"}}>

        <Uploader fileWasUploaded={this.props.fileWasUploaded} fileWasUploadedResp={this.props.fileWasUploadedResp} updBtnDismiss={this.props.updBtnDismiss} />

      </div>
    )
  }// ./render()
  
}// ./UploaderRow

export default UploaderRow
