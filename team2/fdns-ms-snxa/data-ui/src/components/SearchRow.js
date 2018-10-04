import React, { Component } from 'react'
import styled from 'styled-components'


const SearchDiv = styled.div`
  margin: 0;
  padding: 0;
`

class SearchRow extends Component {

  filterByText = (e) => {
    //console.log(e.target.value)
  }// ./filterByText

  render() {
    return(
      <div className="row">
        <SearchDiv className="col-lg-4 col-md-6 col-sm-6">
          <div className="input-group mb-3">
            <div className="input-group-prepend">
              <span className="input-group-text" id="filter1">Filter</span>
            </div>
            <input type="text" className="form-control" onChange={this.filterByText} placeholder="e.g. file name" aria-label="Filter" aria-describedby="filter1"/>
          </div>
        </SearchDiv>
      </div>
    )
  }// ./render
  
}// ./SearchRow

export default SearchRow

