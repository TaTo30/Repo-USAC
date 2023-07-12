import { Doughnut } from 'react-chartjs-2';
import React from "react";
import { Card } from 'react-bootstrap';

export default class DonaRam extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            data: this.obtenerData()
        }
    }

    obtenerData = () => {
        const data = {
            datasets: [{
                data: [],
                backgroundColor: [
                    '#8B0000',
                    '#1CFA66'
                ],
            }],
            labels: [ 
            ]
        }
        return data;
    }
/*
    getPorcentajes = () => {
        var url = 'http://35.223.180.83:5000/api/ram-status';

        fetch(url, {
            mode: 'cors',
            method: 'GET',
            headers: { 'Content-Type': 'application/json' }
        }).then(res => res.json())
            .catch(error => console.error('Error:', error))
            .then(response => {
                console.log(response)
                let datos = []
                let labels = []
                datos.push(response['PercentageUsed'].toFixed(2))
                datos.push(response['PercentageFree'].toFixed(2))
                labels.push("UsedPercentage")
                labels.push("FreePercentage")
                let temp = this.obtenerData()
                temp.datasets[0].data = datos
                temp.labels = labels;
                this.setState({ data: temp })
            }
            );
    }

    componentDidMount() {
        this.getPorcentajes()
        this.intervalo = setInterval(this.getPorcentajes,1500)
    } 
*/
    render() {
        return (

            <>
                <Card bg="dark">
                    <Card.Header><h5>Porcentaje de Utilización RAM</h5></Card.Header>
                    <Card.Body>
                        <Doughnut data={this.state.data} />
                    </Card.Body>
                </Card>

            </>
        );
    }
}
