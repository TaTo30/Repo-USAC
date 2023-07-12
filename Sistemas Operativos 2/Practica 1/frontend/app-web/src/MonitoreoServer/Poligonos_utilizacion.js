
import React from "react";
import { Card } from 'react-bootstrap';
import { Line } from 'react-chartjs-2';
import './monitor_server.css'


const options = {
    scales: {
        yAxes: [
            {
                gridLines: {
                    color: 'grey',
                    lineWidth: 1
                },
                ticks: {
                    fontColor: 'white'
                },
            }
        ],
        xAxes: [
            {
                gridLines: {
                    color: 'grey',
                    lineWidth: 1
                },
                ticks: {
                    fontColor: 'white'
                },
            }
        ],
    },
    legend: {
        labels: {
            fontColor: 'white'
        }
    }
};


export default class Poligonoutilizacion extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            data: this.obtenerData()
        }
    }

    increment = () => {
        const datasetsCopy = this.state.data.datasets.slice(0);
        const dataCopy = datasetsCopy[0].data.slice(0);
        dataCopy[0] = dataCopy[0] + 10;
        datasetsCopy[0].data = dataCopy;

        this.setState({
            data: Object.assign({}, this.state.data, {
                datasets: datasetsCopy
            })
        });
    }


    obtenerData = () => {
        const lista = {
            labels: ['','', '', '', '', '', '', '', '', '', '', '', '', '', '', ''],
            datasets: [
                {
                    label: 'Utilizacion de RAM (%)',
                    fill: true,
                    lineTension: 0.1,
                    backgroundColor: "rgba(75,192,192,0.4)", 
                    borderCapStyle: 'butt',
                    borderDash: [],
                    borderDashOffset: 0.0,
                    borderJoinStyle: 'miter',
                    pointBorderColor: '#00C7F8',
                    pointBackgroundColor: '#fff',
                    pointBorderWidth: 1,
                    pointHoverRadius: 5,
                    pointHoverBackgroundColor: 'rgba(75,192,192,1)',
                    pointHoverBorderColor: 'rgba(220,220,220,1)',
                    pointHoverBorderWidth: 2,
                    pointRadius: 0.5,
                    pointHitRadius: 10,
                    data: []
                }
            ]
        }
        return lista;
    }


    getPorcentajes = () => {
        var url = 'http://3.129.64.103:8000/memory';

        fetch(url, {
            mode: 'cors',
            method: 'GET',
            headers: { 'Content-Type': 'application/json' }
        }).then(res => res.json())
            .catch(error => console.error('Error:', error))
            .then(response => {
                try {
                    const datasetsCopy = this.state.data.datasets.slice(0);
                    const dataCopy = datasetsCopy[0].data.slice(0);
                    if(dataCopy.length>=16){ 
                        dataCopy.shift()
                    }
                    dataCopy.push(((response.use*100)/response.total).toFixed(2)); 
                    datasetsCopy[0].data = dataCopy;

                    this.setState({
                        data: Object.assign({}, this.state.data, {
                            datasets: datasetsCopy
                        })
                    }); 
                } catch (e) {
                    console.log(e)
                }

            }
            );
    }

    componentDidMount() {
        this.getPorcentajes()
        this.intervalo = setInterval(this.getPorcentajes,3000)
    }
 


    render() {
        return (
            <Card className="card text-white bg-dark" >
                <Card.Header><h5>Utilización de RAM</h5></Card.Header>
                <Card.Body>
                    <Line data={this.state.data} options={options} />
                </Card.Body> 
            </Card>
        );
    }



}