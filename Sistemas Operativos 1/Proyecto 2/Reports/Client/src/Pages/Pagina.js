import React from "react";
import NavVar from "../Components/NavVar";
import socket from "../Recursos/Socket";
import Collapse from "../Components/CollapseCard";
import Table from "../Components/Table";
import Pastel from "../Components/Pastel"
import 'bootstrap/dist/css/bootstrap.min.css'

const connection = require("../Recursos/Connection")

class Principal extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            cargado: false,
            reportes: [{title: "Tabla De Valores DB", id: "reporte1"}, {title: "Últimos 10 juegos.", id: "reporte2"}, {title: "Los 10 mejores jugadores.", id: "reporte3"}, {title: "Estadísticas del jugador en tiempo real.", id: "reporte4"}, {title: "Log de transacciones en MongoDB.", id: "reporteLog"}, {title: "Tabla con los logs almacenados.", id: "reporte5"}, {title: "Gráfica del top 3 de juegos.", id: "reporte6"}, {title: "Gráfica que compara a los 3 workers de go", id: "reporte7"}],              
            
            reporte1Columnas: {mongo: ["Reporte", "Mongo"], redis: ["Reporte", "Redis"]},
            reporte2Columnas: {mongo: ["Reporte", "Mongo"], redis: ["Reporte", "Redis"]},
            reporte3Columnas: {mongo: ["Reporte", "Mongo"], redis: ["Reporte", "Redis"]},
            reporte4Columnas: {mongo: ["Reporte", "Mongo"], redis: ["Reporte", "Redis"]},
            reporte5Columnas: {mongo: ["Reporte", "Mongo"], redis: ["Reporte", "Redis"]},
            reporte6Labels: {mongo: ["Reporte", "Mongo"], redis: ["Reporte", "Redis"]},
            reporte7Labels: {mongo: ["Reporte", "Mongo"], redis: ["Reporte", "Redis"]},

            reporte1Data: {mongo: [["R", "M"]], redis: [["R", "M"]]},
            reporte2Data: {mongo: [["R", "M"]], redis: [["R", "M"]]},
            reporte3Data: {mongo: [["R", "M"]], redis: [["R", "M"]]},
            reporte4Data: {mongo: [["R", "M"]], redis: [["R", "M"]]},
            reporte5Data: {mongo: [["R", "M"]], redis: [["R", "M"]]},
            reporte6Data: {mongo: [1, 1], redis: [1, 1]},
            reporte7Data: {mongo: [1, 1], redis: [1, 1]}
        }  
        this.componentDidMount = this.componentDidMount.bind(this);
    }   

    componentDidMount(){
        (async () =>{
            socket.on('verificado', data=>{
                this.setState({
                    cargado: true,
                    reporte1Columnas: data.reporte1Columnas,
                    reporte2Columnas: data.reporte2Columnas,
                    reporte3Columnas: data.reporte3Columnas,
                    reporte4Columnas: data.reporte4Columnas,
                    reporte5Columnas: data.reporte5Columnas,
                    reporte6Labels: data.reporte6Labels,
                    reporte7Labels: data.reporte7Labels,

                    reporte1Data: data.reporte1Data,
                    reporte2Data: data.reporte2Data,
                    reporte3Data: data.reporte3Data,
                    reporte4Data: data.reporte4Data,
                    reporte5Data: data.reporte5Data,
                    reporte6Data: data.reporte6Data,
                    reporte7Data: data.reporte7Data
                })
            })

            socket.emit('verificar', "")

            setInterval(() => {
                var player = document.getElementById('idPlayer').value
                socket.emit('verificar', player)
            }, 5000);
            
        })();
    }

    render() {        

        if(this.state.cargado===false){
            return(
                <form>
                    <NavVar/>
                    <div style={{textAlign: "center", marginTop: 250}}>
                        <div class="spinner-border text-light" role="status">
                            <span class="sr-only"></span>
                        </div>
                    </div>           
                </form>
            )
        }       

        let reporte1 =  <div class="row">
                            <div class="col">
                                <Table columnas={this.state.reporte1Columnas.mongo} tuplas={this.state.reporte1Data.mongo} title={"Mongo"}/>
                            </div>
                            <div class="col">
                                <Table columnas={this.state.reporte1Columnas.redis} tuplas={this.state.reporte1Data.redis} title={"Redis"}/>
                            </div>
                        </div>
                
        let reporte2 =  <div class="row">
                            <div class="col">
                                <Table columnas={this.state.reporte2Columnas.mongo} tuplas={this.state.reporte2Data.mongo} title={"Mongo"}/>
                            </div>
                            <div class="col">
                                <Table columnas={this.state.reporte2Columnas.redis} tuplas={this.state.reporte2Data.redis} title={"Redis"}/>
                            </div>
                        </div>

        let reporte3 =  <div class="row">
                            <div class="col">
                                <Table columnas={this.state.reporte3Columnas.mongo} tuplas={this.state.reporte3Data.mongo} title={"Mongo"}/>
                            </div>
                            <div class="col">
                                <Table columnas={this.state.reporte3Columnas.redis} tuplas={this.state.reporte3Data.redis} title={"Redis"}/>
                            </div>
                        </div>
                        
        let reporte4 =  <div>
                            <div class="row" style={{marginLeft: 150, marginRight: 150, marginBottom: 15}}>
                                <input class="form-control" type="text" placeholder="Numero Jugador" aria-label="default input example" id="idPlayer"/>
                            </div>
                            
                            <div class="row">
                                <div class="col">
                                    <Table columnas={this.state.reporte4Columnas.mongo} tuplas={this.state.reporte4Data.mongo} title={"Mongo"}/>
                                </div>
                                <div class="col">
                                    <Table columnas={this.state.reporte4Columnas.redis} tuplas={this.state.reporte4Data.redis} title={"Redis"}/>
                                </div>
                            </div>
                        </div>

        let reporte5 =  <div class="row">
                            <div class="col">
                                <Table columnas={this.state.reporte5Columnas.mongo} tuplas={this.state.reporte5Data.mongo} title={"Mongo"}/>
                            </div>
                            <div class="col">
                                <Table columnas={this.state.reporte5Columnas.redis} tuplas={this.state.reporte5Data.redis} title={"Redis"}/>
                            </div>
                        </div>

        let reporte6 =  <div class="form-row">
                            <div class="form-group col-sm-5" style={{marginLeft: 75, marginTop: 25}}>
                                <Pastel labels={this.state.reporte6Labels.mongo} data={this.state.reporte6Data.mongo}/>
                            </div>,
                            <div class="form-group col-sm-5" style={{marginLeft: 75, marginTop: 25}}>
                                <Pastel labels={this.state.reporte6Labels.redis} data={this.state.reporte6Data.redis}/>
                            </div>
                        </div>

        let reporte7 =  <div class="form-row">
                            <div class="form-group col-sm-5" style={{marginLeft: 75, marginTop: 25}}>
                                <Pastel labels={this.state.reporte7Labels.mongo} data={this.state.reporte7Data.mongo}/>
                            </div>,
                            <div class="form-group col-sm-5" style={{marginLeft: 75, marginTop: 25}}>
                                <Pastel labels={this.state.reporte7Labels.redis} data={this.state.reporte7Data.redis}/>
                            </div>
                        </div>

        let subDesplegables = [<Collapse target={this.state.reportes[5].id} title={this.state.reportes[5].title} component={reporte5} type={"btn-outline-success"}/>
                    , <Collapse target={this.state.reportes[6].id} title={this.state.reportes[6].title} component={reporte6} type={"btn-outline-success"}/>
                    , <Collapse target={this.state.reportes[7].id} title={this.state.reportes[7].title} component={reporte7} type={"btn-outline-success"}/>
                    ]

        let desplegables = [<Collapse target={this.state.reportes[0].id} title={this.state.reportes[0].title} component={reporte1} type={"btn-outline-info"}/>
                    , <Collapse target={this.state.reportes[1].id} title={this.state.reportes[1].title} component={reporte2} type={"btn-outline-info"}/>
                    , <Collapse target={this.state.reportes[2].id} title={this.state.reportes[2].title} component={reporte3} type={"btn-outline-info"}/>
                    , <Collapse target={this.state.reportes[3].id} title={this.state.reportes[3].title} component={reporte4} type={"btn-outline-info"}/>
                    , <Collapse target={this.state.reportes[4].id} title={this.state.reportes[4].title} component={subDesplegables} type={"btn-outline-info"}/>
                    ]

        return (
            <form>                
                <NavVar/>
                <div style={{marginTop: 75}}>
                {   
                    desplegables
                }    
                </div>         
            </form>
        );
    }
}

export default Principal;