import React from "react";
import NavVar from "../Components/NavVar";
import Card from "../Components/CardPublication"
import socket from "../Recursos/Socket";
import BootstrapSwitchButton from 'bootstrap-switch-button-react'
import { Spinner } from "reactstrap"
import 'bootstrap/dist/css/bootstrap.min.css'

const connection = require("../Recursos/Connection")

class Principal extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            twits: [],
            db: false,
            add: false,
            cargado: false              
        }  
        this.changeDB = this.changeDB.bind(this);
        this.componentDidMount = this.componentDidMount.bind(this);
    }   

    componentDidMount(){
        fetch(connection.getConnection()+'/getTweetsMongo')
        .then(res => res.json()).then((data) => {
            if (data["null"]===false){      
                console.log(data["data"])
                this.setState({
                    twits: JSON.parse(data["data"]),      
                    cargado: true 
                })
            }
        }) 
        
        socket.emit('connection');   
        
        socket.emit('iniciar', 'mongo')

        socket.on('addTweetSQL', data => {
            this.setState({twits: data.data})
            socket.emit('verificarSQL', "")
        })
        
        socket.on('iniciadoSQL', data => {            
            socket.emit('verificarSQL', "")
        })

        socket.on('alert', data => {            
            alert(data)
        })
    }

    getSQL(){
        fetch(connection.getConnection()+'/getTweets')
            .then(res => res.json()).then((data) => {
                if (data["null"]===false){      
                    console.log(data["data"])                    
                    this.setState({
                        twits: JSON.parse(data["data"]),      
                        cargado: true, 
                        db: true
                    })
                }
            })
    }

    getCosmos(){
        fetch(connection.getConnection()+'/getTweetsMongo')
            .then(res => res.json()).then((data) => {
                if (data["null"]===false){      
                    console.log(data["data"])                    
                    this.setState({
                        twits: JSON.parse(data["data"]),      
                        cargado: true,
                        db: false
                    })
                }
            })
    }

    addTweet(tweet){
        let array = this.state.twits;
        array.unshift({"nombre": tweet.nombre, "comentario": tweet.comentario, "fecha": tweet.fecha, "upvotes": tweet.upvotes, "downvotes": tweet.downvotes, "hashtags": tweet.hashtags})
        this.setState({
            twits: array,
            add: true
        })
    }

    changeDB(){
        this.setState({
            cargado: false
        })
        if(this.state.db==true) { 
            socket.emit('iniciar', "mongo");
            this.getCosmos()     
        }else{            
            socket.emit('iniciar', "sql");
            this.getSQL()
        }  
    }

    render() {        

        let switchC =   <div style={{marginLeft: 25, marginRight:25}}>                                                                                   
                            <BootstrapSwitchButton checked={this.state.db} onstyle="outline-primary" offstyle="outline-success" onlabel='MySql' offlabel='Mongo' width={100} onChange={this.changeDB}/>
                        </div>

        if(this.state.cargado===false){
            return(
                <form>
                    <NavVar switchComp={switchC}/>
                    <div style={{textAlign: "center", marginTop: 250}}>
                        <div class="spinner-border text-light" role="status">
                            <span class="sr-only"></span>
                        </div>
                    </div>           
                </form>
            )
        }       

        return (
            <form>                
                <NavVar switchComp={switchC}/>
                <div style={{marginTop: 75}}>
                {                    
                    this.state.twits.map(tupla => (
                        <Card nombre={tupla["nombre"]} comentario={tupla["comentario"]} fecha={tupla["fecha"]} hashtags={tupla["hashtags"]} downvotes={tupla["upvotes"]} upvotes={tupla["downvotes"]}/>
                    ))
                }    
                </div>         
            </form>
        );
    }
}

export default Principal;