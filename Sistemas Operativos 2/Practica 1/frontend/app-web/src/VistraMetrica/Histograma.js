import { Bar } from 'react-chartjs-2';
import React from "react";
import { Card } from 'react-bootstrap';


export default class Histograma extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            data: {
                labels: [1,1,1,1,1,1,1,1,1,1,1 ],
                datasets: [
                    {
                        label: 'Cantidad',
                        backgroundColor: '#113CFF',
                        borderColor: '#113CFF',
                        borderWidth: 1,
                        hoverBackgroundColor: '#5A78FF',
                        hoverBorderColor: '#113CFF',
                        data: [2, 5, 8, 7, 4, 1, 6, 3, 5] 
                    }
                ]
            },
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                }
            }
            
        }
    }


    obtenerData = () => {
        const data = {
            labels: [],
            datasets: [
                {
                    label: 'Cantidad',
                    backgroundColor: '#113CFF',
                    borderColor: '#113CFF',
                    beginAtZero: true,
                    hoverBackgroundColor: '#5A78FF',
                    hoverBorderColor: '#113CFF',
                    data: [2, 5, 8, 7, 4, 0, 6, 3, 5]
                }
            ]
        }
        return data;
    };
/*
    componentDidMount() { 
        this.getBarrasEdad()
        this.intervalo = setInterval(this.getBarrasEdad,3000)
    }
*/
    getBarrasEdad = () => {
        var url = 'http://35.223.180.83:5000/api/age-ranges';

        fetch(url, {
            mode: 'cors',
            method: 'GET', // or 'PUT' 
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(res => res.json())
            .catch(error => console.error('Error:', error))
            .then(response => {
                let labels = []
                let data = []
                for(let i = 0; i< response.length -1;i++){
                    let e = response[i]
                    labels.push(e.name)
                    data.push(e.count)
                } 
                let temp = this.obtenerData()
                temp.labels = labels
                temp.datasets[0].data = data
                this.setState({data:temp})
            }
        );
    }

    render() {
        return (
            <>
                <Card bg="dark">
                    <Card.Header><h5>Frecuencia Ejecucion Systema Call</h5></Card.Header>
                    <Card.Body>
                        <Bar data={this.state.data} options={this.state.options} />
                    </Card.Body> 
                </Card>

            </>
        );
    }
}