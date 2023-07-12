import React from 'react'

class NavVar extends React.Component {
    constructor(props) {
        super(props);  
    }

    render(){
      return(
        <div>
            <nav class="navbar navbar-expand-lg navbar-dark bg-dark fixed-top" style={{border: 10, borderColor: 'black'}}>
                <a class="navbar-brand" href="/Principal" style={{marginLeft: 25}}>SQUID GAME</a>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div style={{marginLeft: 1000}}>
                    {this.props.switchComp}
                </div>
            </nav>
        </div>
      )
    }
  }

  
  
  export default NavVar