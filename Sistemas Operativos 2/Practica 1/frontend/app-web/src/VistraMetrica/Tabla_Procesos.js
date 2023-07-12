import React from "react";
import { Card, Table } from "react-bootstrap";
export default class Tablaprocesos extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            tabla: <tr></tr>,
            elementos: []
        }
    }
/*
    componentDidMount() {
        this.getDatosRecopilados(this.state.filtro)
        this.intervalo = setInterval(this.getDatosRecopilados,3000)
    } 
*/
    getDatosRecopilados = (tipo) => {
        this.setState({ filtro: tipo })
        var url = 'http://35.223.180.83:5000/api/services-status';

        fetch(url, {
            mode: 'cors',
            method: 'GET', // or 'PUT' 
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(res => res.json())
            .catch(error => console.error('Error:', error))
            .then(response => {
                let datos = []
                let contador=0  
                for (let e of response) {
                    datos.push(
                        (
                            <tr key={contador}>
                                <td>{e.PID}</td>
                                <td>{e.Name}</td>
                                <td>{e.Parent_ID}</td>
                                <td>{e.State}</td> 
                            </tr>
                        )
                    )
                    contador++
                }
                this.setState({ elementos: datos })
            });
    } 

    render() {
        return (
            <Card bg="dark">
                <Card.Header><h5>Lista de Procesos</h5></Card.Header>
                <Card.Body>
                    <Table striped bordered hover size="sm" variant="dark">
                        <thead>
                            <tr>
                                <th>PID</th>
                                <th>Nombre</th>
                                <th>PID de padre</th>
                                <th>Estado</th>
                            </tr>
                        </thead>
                        <tbody>
                            {this.state.elementos}
                        </tbody>
                    </Table>
                </Card.Body>
            </Card>

        );
    }
}

