import React from 'react'

class Tabla extends React.Component {
  constructor(props){
    super(props);
    
    this.state = {
    };
  } 

  render(){
    var {cols} = []
    if(this.props.columnas != null){
        cols = this.props.columnas;
    }
    var {tuplas} = [];
    if(this.props.tuplas != null && this.props.tuplas != undefined)
        tuplas = this.props.tuplas;
        
    return(
      <form >  
                <div class="container">
                    <div style={{textAlign: "center"}}>{this.props.title}</div>    
                    <table class="table" style={{color: '#AFAFAF'}}>
                        <thead>                                                
                        <tr>
                            {cols.map(col => (
                                <th>{col}</th>
                            ))}
                        </tr>
                        </thead>
                        <tbody>
                        {tuplas.map(tupla => (
                        <tr>
                              {tupla.map(t => (
                                  <td>{t}</td>
                              ))}
                        </tr>
                        ))}
                        </tbody>
                    </table>
                   </div>         
      </form>
    )
  }
}

export default Tabla;