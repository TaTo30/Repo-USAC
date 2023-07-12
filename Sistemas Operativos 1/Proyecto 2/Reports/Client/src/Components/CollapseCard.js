import React from 'react'

class Collapse extends React.Component {
    constructor(props){
        super(props);
        
        this.state = {
        };
    }
    
    render(){
      return(
            <div>
                <p>
                    <button class={"btn "+this.props.type+" btn-lg btn-block"} type="button" data-toggle="collapse" data-target={"#"+this.props.target} aria-expanded="false" aria-controls={this.props.target}>
                        {this.props.title}
                    </button>
                </p>
                <div class="collapse" id={this.props.target}>
                    {this.props.component}
                </div>
            </div>
      )
    }
  }
  
  export default Collapse