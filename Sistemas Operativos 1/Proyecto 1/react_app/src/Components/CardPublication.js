import React from 'react'
import Like from '@material-ui/icons/ThumbUpRounded';
import DisLike from '@material-ui/icons/ThumbDownRounded';

class NavVar extends React.Component {
    constructor(props){
        super(props);
        
        this.state = {
        };
    }

    render(){
        var {nombre, comentario, fecha, hashtags} = "";
        var {upvotes} = 0;
        var {downvotes} = 0;

        if(this.props.nombre != null) nombre = this.props.nombre;
        if(this.props.comentario != null) comentario = this.props.comentario;
        if(this.props.fecha != null) fecha = this.props.fecha;
        if(this.props.hashtags != null){
            if(this.props.hashtags instanceof Array) hashtags = "#"+this.props.hashtags.join(' #')
            else hashtags = this.props.hashtags
        };
        if(this.props.upvotes != null) upvotes = this.props.upvotes;
        if(this.props.downvotes != null) downvotes = this.props.downvotes;

        return(
            <div class="card text" style={{marginRight: 25, marginLeft: 25, marginTop: 25}}>
                <div class="card-header" style={{backgroundColor: '#212529', color: 'White'}}>
                    {fecha}
                </div>
                <div class="card-body" >
                    <h5 class="card-title">{nombre}</h5>
                    <p class="card-text text-center">{comentario}</p>
                </div >
                <div class="card-footer text-muted">
                    {hashtags}
                </div>
                <div class="card-footer text-muted" style={{backgroundColor: '#212529', color: 'White'}}>
                    <button type="button" class="btn-dark btn-lg" disabled>{downvotes} <Like/></button>
                    <button type="button" class="btn-dark btn-lg" disabled>{upvotes} <DisLike/></button>
                </div>
            </div>                    
        ) 
    }
}
  
  export default NavVar