import React, { Component } from 'react'

import './styled.js'

import isofetch from 'isomorphic-fetch'

import HeaderRow from './components/HeaderRow'
import UpdBtnRow from './components/UpdBtnRow'
import UploaderRow from './components/UploaderRow'
import SearchRow from './components/SearchRow'
import UpdsTable from './components/UpdsTable'

import { HOST } from './Env.js'

class App extends Component {

  state = {
    showUploader: false,
    updsMetaData: {},
    addedOneRow: false,
    addedOneRowErr: false,
    uploadDisabled: false,
  }// ./state

  componentWillMount(){

  }// ./componentWillMount

  updBtnClick = () => {
    this.setState({
      showUploader:true,
    })
  }// ./updBtnClick
  updBtnDismiss = () => {
    this.setState({
      showUploader: false,
    })
  }// ./updBtnDismiss

  fileWasUploaded = () => {
    this.setState({
      uploadDisabled: true,
      showUploader:false,
    })
  }// ./fileWasUploaded

  fileWasUploadedResp = (resp) => {
    //console.log(resp)
    if (resp==='ok') { 

      this.fetchMetaData() 
      this.setState({
        addedOneRow: true,
        addedOneRowErr: false,
        uploadDisabled: false,
      })
      
    } else {
      //resp is err
      this.setState({
        addedOneRow: true,
        addedOneRowErr: true,
        uploadDisabled: false,
      })
    }
    let that = this
    setTimeout(() => {
      that.setState({addedOneRow:false})
    },2700)

  }// ./fileWasUploadedResp

  fileWasDeletedResp = (resp) => {
    if ( resp === 'ok' ) {
      this.fetchMetaData()
    }
  }// ./fileWasDeletedResp

  fetchMetaData() {

    // let that = this

    // let url = `/submissions`

    // isofetch( HOST + url, {
    //   method: "GET",
    // }).then(function(resp) {
 
    //   resp.json().then(data => { 
    //     that.setState( {updsMetaData :data} ) 
    //   })
    // }).catch(err => { console.log(err) })// ./isofetch

  }// ./fetchMetaData

  modalBackClick() {
    this.setState({
      showUploader:false,
    })
  }// ./modalBackClick

  render() {

    return (
    	<div className="container-fluid">

        <HeaderRow HEADER_TITLE='STREET NETWORK ANALISYS'/>

        <UpdBtnRow uploadDisabled={this.state.uploadDisabled} addedOneRow={this.state.addedOneRow} addedOneRowErr={this.state.addedOneRowErr} updBtnClick={this.updBtnClick}/>

        { this.state.showUploader? <UploaderRow fileWasUploaded={this.fileWasUploaded} fileWasUploadedResp={this.fileWasUploadedResp} updBtnDismiss={this.updBtnDismiss} /> : null }  
        { this.state.showUploader? <div onClick={ (e)=>this.modalBackClick() } className="modal-backdrop fade show"></div> : null }

        <br />
        <h4>Previous Uploads</h4>
        <SearchRow />

        <UpdsTable fileWasDeletedResp={this.fileWasDeletedResp} updsMetaData={this.state.updsMetaData}  />
    
	     </div>
    )
  }// ./render

}// ./App

export default App
