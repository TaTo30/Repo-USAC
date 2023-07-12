import React from 'react'

class NavVar extends React.Component {
    constructor(props) {
        super(props);  
    }

    render(){
      return(
        <div>
            <nav class="navbar navbar-expand-lg navbar-dark bg-dark fixed-top" style={{border: 10, borderColor: 'black'}}>
                <a class="navbar-brand" href="/Principal" style={{marginLeft: 25}}>Inicio</a>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="collapse navbar-collapse" id="navbarSupportedContent">
                    <ul class="navbar-nav mr-auto">
                        <li class="nav-item active">
                            <a class="nav-link" href="/Principal">Linea De Tiempo</a>
                        </li>                        
                    </ul>
                </div>                      
                <div class="collapse navbar-collapse" id="navbarSupportedContent">
                    <ul class="navbar-nav mr-auto">
                        <li class="nav-item">
                            <a class="nav-link" href="/Reportes">Reportes</a>
                        </li>           
                    </ul>
                </div>
                <div>
                    {this.props.switchComp}
                </div>
            </nav>
        </div>
      )
    }
  }

  
  
  export default NavVar