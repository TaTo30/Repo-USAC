import React from "react";
import NavVar from "../Components/NavVar";
import DataCard from "../Components/DataCard";
import Tabla from "../Components/Tabla";
import Barras from "../Components/Barras"
import Pastel from "../Components/Pastel"

import {Spinner} from "reactstrap"
import 'bootstrap/dist/css/bootstrap.min.css'
import BootstrapSwitchButton from 'bootstrap-switch-button-react'
import Like from '@material-ui/icons/ThumbUpRounded';
import Cat from '@material-ui/icons/Category';
import Book from '@material-ui/icons/MenuBookRounded';

const connection = require("../Recursos/Connection")



class Reportes extends React.Component {
    constructor(props){
        super(props);
        
        this.state = {
            principalData: [],
            cakeLabels: [],
            cakeData: [],
            barLabels: [],
            barData: [],
            columnas: ['NOMBRE', 'FECHA', 'COMENTARIO', 'UPVOTES', 'DOWNVOTES', 'HASHTAGS'],
            dataTable: [],
            report: "ambos",
            cargado: false,
            db: false
        };

        this.setAmbos = this.setAmbos.bind(this);
        this.setBarras = this.setBarras.bind(this);
        this.setPastel = this.setPastel.bind(this);
        this.changeDB = this.changeDB.bind(this);
        this.componentDidMount = this.componentDidMount.bind(this)
    }

    changeDB(){
        this.setState({
            principalData: [],
            cakeLabels: [],
            cakeData: [],
            barLabels: [],
            barData: [],
            columnas: ['NOMBRE', 'FECHA', 'COMENTARIO', 'UPVOTES', 'DOWNVOTES', 'HASHTAGS'],
            dataTable: [],
            cargado: false
        })  

        if(this.state.db===true) { 
            this.getMongo()   
        }
        else {
            this.getSQL()
        }
    }

    setAmbos(){
        this.setState({
            report: "ambos"
        })
    }

    setBarras(){
        this.setState({
            report: "bar"
        })
    }

    setPastel(){
        this.setState({
            report: "cake"
        })
    }

    getMongo(){        
        fetch(connection.getConnection()+'/getReportsMongo'
            , {
                method: 'POST', 
                headers: {
                'Content-Type': 'application/json',
                'Access-Control-Allow-Origin' : '*',
                'Access-Control-Allow-Methods' : 'GET, PUT, POST, DELETE',
                'Access-Control-Allow-Headers': ''
            },
            body: JSON.stringify({"fecha": "xd"}) 
            }).then(res => res.json()).then((data) => {
                let newData = data["data"]
                console.log(newData)
                this.setState({
                    principalData: [newData.twits, newData.hashtags, newData.upvotes],
                    cakeLabels: newData["tophash"],
                    cakeData: newData["tophashData"],
                    barLabels: newData["days"],
                    barData: [newData["upvotesByDay"], newData["downvotesByDay"]],
                    dataTable: newData["last100entrys"],
                    db:false,
                    cargado: true
                })    
            })
    }

    getSQL(){
        fetch(connection.getConnection()+'/getReportsMySQL'
            , {
                method: 'POST', 
                headers: {
                'Content-Type': 'application/json',
                'Access-Control-Allow-Origin' : '*',
                'Access-Control-Allow-Methods' : 'GET, PUT, POST, DELETE',
                'Access-Control-Allow-Headers': ''
            },
            body: JSON.stringify({"fecha": "xd"}) 
            }).then(res => res.json()).then((data) => {
                let newData = data["data"]
                console.log(newData)
                this.setState({
                    principalData: [newData.twits, newData.hashtags, newData.upvotes],
                    cakeLabels: newData["tophash"],
                    cakeData: newData["tophashData"],
                    barLabels: newData["days"],
                    barData: [newData["upvotesByDay"], newData["downvotesByDay"]],
                    dataTable: newData["last100entrys"],
                    db:true,
                    cargado: true
                })    
            })   
    }

    componentDidMount(){
        if(this.state.db===false)
            this.getMongo()
        else this.getSQL()
        /*socket.on('addTweet', data => {
            this.addTweet(data)
            console.log(JSON.stringify(data), "-----")
        })*/
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

        let ambos = [
            <div class="row" style={{marginRight: 25, marginLeft: 75, marginTop: 25, textAlign: "center"}}>
                <div class="col-sm-6">
                    <Barras altura={230} labels={this.state.barLabels} upvotes={this.state.barData[0]} downvotes={this.state.barData[1]}/>
                </div>,
                <div class="col-sm-5" style={{left: 70}}>
                    <Pastel labels={this.state.cakeLabels} data={this.state.cakeData}/>
                </div>
            </div>
        ]

        let onlyBars = [
            <div style={{marginRight: "auto", marginLeft: 25, marginTop: 25, left: 200}}>
                <Barras altura={150} labels={this.state.barLabels} upvotes={this.state.barData[0]} downvotes={this.state.barData[1]}/>
            </div>
        ]
        let onlyCake = [
            <div style={{marginLeft: 325, textAlign: "center", width: 800, marginTop: 25}}>
                <div class="col" style={{left: 250}}>
                    <Pastel labels={this.state.cakeLabels} data={this.state.cakeData}/>
                </div>
            </div>
        ]

        var report = ambos;

        if(this.state.report == "ambos"){
            report=ambos;
        }
        if(this.state.report == "bar"){
            report=onlyBars;
        }
        if(this.state.report == "cake"){
            report=onlyCake;
        }
        
        return (
            <form>
                <NavVar switchComp={switchC}/>
                <div class="row" style={{marginRight: 25, marginLeft: 25, marginTop: 75}}>
                    <DataCard nombre="NOTICIAS" dato={this.state.principalData[0]} icon = {<Book fontSize="large"/>} />
                    <DataCard nombre="HASHTAGS" dato={this.state.principalData[1]+" DIFERENTES"} icon={<Cat fontSize="large"/>}/>
                    <DataCard nombre="UPVOTES" dato={this.state.principalData[2]} icon={<Like fontSize="large"/>}/>
                </div>

                <div>
                    {report}      
                </div>

                <div class="row" style={{marginRight: 25, marginLeft: 25, marginTop: 25, marginBottom: 25, textAlign: "center"}}>
                    <div class="col-sm-4" id="divBar">
                        <button type="button" class="btn btn-outline-danger btn-block" onClick={this.setBarras}>Barras</button>
                    </div>
                    <div class="col-sm-4" id="divCake">
                        <button type="button" class="btn btn-outline-danger btn-block" onClick={this.setAmbos}>Ambos</button>
                    </div>
                    <div class="col-sm-4" id="divCake">
                        <button type="button" class="btn btn-outline-danger btn-block" onClick={this.setPastel}>Pastel</button>
                    </div>
                </div >         

                <div>
                    <Tabla columnas={this.state.columnas} tuplas={this.state.dataTable}/>
                </div>
            </form>
        );
    }
}

export default Reportes;