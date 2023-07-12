import React from 'react'


class DataCard extends React.Component {
    constructor(props){
        super(props);
        
        this.state = {
        };
    }

    render(){
        var {nombre, dato, comentario} = "";

        if(this.props.nombre != null) nombre = this.props.nombre;
        if(this.props.comentario != null) comentario = this.props.comentario;
        if(this.props.dato != null) dato = this.props.dato;

        return(                   
            <div class="col-sm-4">
                <div class="card mb-3">
                    <div class="row no-gutters">
                        <div class="col-md-6">
                            <div class="card-body">
                                <h5 class="card-title">{nombre}</h5>
                                <p class="card-text">{dato}</p>
                                <p class="card-text"><small class="text-muted"></small></p>
                            </div>
                        </div>
                        <div class="col-md-2" style={{position: 'absolute', top: 40, left: 300}}>                            
                            {this.props.icon}
                        </div>
                    </div>
                </div>
            </div>                 
        ) 
    }
}
  
  export default DataCard