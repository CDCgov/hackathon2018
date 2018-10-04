import React, { Component } from 'react'
import styled from 'styled-components'

const StyledSpinner = styled.div`

  border: 4px solid #f3f3f3; /* Light grey */
  border-top: 4px solid #007bff; /* Blue */
  border-bottom: 4px solid pink; /* Blue */

  border-radius: 50%;
  width: 37px;
  height: 37px;
  animation: spin 2s linear infinite;
  margin-top: 15px;
  display: inline-block;

  @keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}
`

class Spinner extends Component {

  render() {
    return (
          <div>
            <StyledSpinner />
          </div>
      )
  }// ./render

}// ./Spinner

export default Spinner

