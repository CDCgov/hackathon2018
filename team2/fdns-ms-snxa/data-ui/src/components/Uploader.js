import React, { Component } from 'react'
import PropTypes from 'prop-types'
import Dropzone from 'react-dropzone'
import isofetch from 'isomorphic-fetch'

import './Uploader.css'
import { HOST } from '../Env.js'


// set the prop types from predefined shapes or standard types
const propTypes = {
  accept: PropTypes.string,
  onDrop: PropTypes.func,
}
// set the defaults
const defaultProps = {
  //accept: 'application/json', // TODO
}

class Uploader extends Component {

  // handle file drop or browse
  handleDrop = (acceptedFiles, rejectedFiles) => {
    //console.log("handleDrop...")

    console.log('file name:', acceptedFiles[0].name)

    let formData = new FormData()

    formData.append('updclienttime', Date.now().toString())

    formData.append('updfile', acceptedFiles[0])

    let fileWasUploadedResp = this.props.fileWasUploadedResp

    let fileWasUploaded = this.props.fileWasUploaded

    // let url = `${HOST}/submissions`
    // sending file through formData to server
    // isofetch(url, {
    //   method: 'POST',
    //   body: formData,
    //   //mode: "cors", // no-cors, cors, *same-origin
    // }).then(function(resp) {

    //   if ( resp.status === 200 ) {

    //   } else { 

    //     console.log("file upload error")
    //     fileWasUploadedResp('err') 
    //   }
      
    // }).catch(function(err) {

    //   console.log("file upload error"); console.log(err)
    //   fileWasUploadedResp('err')

    // })// ./isofetch
    fileWasUploaded()
    fileWasUploadedResp('ok')


  }// ./handleDrop


  renderLayout() {
    return (
      <div className="instructions">
        <figure className="files"></figure>
        <h5>Drag and Drop or Browse your files</h5>
        <p>{ 'Select add file or simply drag and drop a file anywhere on this page to start uploading.' }</p>
        <button className="btn btn-action btn-outline-primary">{ 'Add File' }</button>
      </div>
    )
  }// ./renderLayout

  render() {
    return (
      <div className="Uploader-container">
      <button onClick={this.props.updBtnDismiss} className="btn-dismiss-uploader btn btn-action btn-info text-right">{ 'Close' }</button>  
      <div className="Uploader">             

        <Dropzone onDrop={this.handleDrop}
                  multiple={false}
                  className="uploader-drop-box"
                  activeClassName="active"
                  rejectClassName="reject">
                  {this.renderLayout()}
        </Dropzone>

    </div></div>
    )
  }
}// ./Uploader

// set the props, defaults 
Uploader.propTypes = propTypes
Uploader.defaultProps = defaultProps

export default Uploader

