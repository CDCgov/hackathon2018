import React, { Component } from 'react'

import UpdsTRow from './UpdsTRow'

class UpdsTable extends Component {


  render() {

    let thead = []
    let rows = Object.keys(this.props.updsMetaData).map(e => this.props.updsMetaData[e]) 

    rows.sort(function(a, b) {
      return new Date(parseInt(b['updservertime'], 10)) - new Date(parseInt(a['updservertime'], 10))
    })


    return (
            <div className="row">
              <div className="table-responsive">
      
                <table className="table table-hover">

                  <thead className="thead-light">
                    <tr>
                    
                      <th scope="col">Zone</th>
                      <th scope="col">File Name</th>
                      <th scope="col">Uploaded By</th>
                      <th scope="col">Uploaded Time</th>

                      <th scope="col">Data Profiler</th>

                    </tr>
                  </thead>
    
                  <tbody>
   
                    {rows.map((el,key)=>{
                          return (
                            <UpdsTRow fileWasDeletedResp={this.props.fileWasDeletedResp} el={el} key={key}></UpdsTRow>
                            )
                    })}
                  </tbody>

                </table>

              </div>
            </div>
          )// ./return
  }// .render

}// ./UpdsTable

export default UpdsTable
