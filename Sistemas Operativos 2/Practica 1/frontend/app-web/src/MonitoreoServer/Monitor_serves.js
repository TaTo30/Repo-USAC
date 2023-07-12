import React from "react";
import { Container, Row, Col, Card } from "react-bootstrap";
import './monitor_server.css'
import { withRouter } from "react-router";

import Poligonosutilizacion from './Poligonos_utilizacion'
import NavbarVista from "../VistraMetrica/NavbarVista";
class MonitorServer extends React.Component {


    constructor(props) {
        super(props);
        this.state = {
            total:0,
            used:0,
            free:0,
            available:0
        }
    }

    getData = () => {
        var url = 'http://3.129.64.103:8000/memory';

        fetch(url, {
            mode: 'cors',
            method: 'GET',
            headers: { 'Content-Type': 'application/json' }
        }).then(res => res.json())
            .catch(error => console.error('Error:', error))
            .then(response => { 
                this.setState({total:response.total, used:response.use, free:response.free})

            }
            );
    }

    componentDidMount(){
        this.getData()
        this.intervalo = setInterval(this.getData,3000)
    }

    render() {
        return (

            <Container fluid={true} >
                <Row>
                    <NavbarVista />
                </Row>
                <br />
                <Row>
                    <Col>
                        <Container >
                            <Row>
                                <Col className="mb-3 col-lg-3 col-12">
                                    <Card bg="dark">
                                        <Card.Header>RAM servidor (MB)</Card.Header> 
                                        <Card.Body>{this.state.total}</Card.Body>
                                    </Card>
                                </Col>
                                <Col className="mb-3 col-lg-3 col-12">
                                    <Card bg="dark">
                                        <Card.Header>RAM consumida</Card.Header> 
                                        <Card.Body>{this.state.used}</Card.Body>
                                    </Card>
                                </Col>
                                <Col className="mb-3 col-lg-3 col-12">
                                    <Card bg="dark">
                                        <Card.Header>RAM libre</Card.Header> 
                                        <Card.Body>{this.state.free}</Card.Body>
                                    </Card>
                                </Col>
                                <Col className="mb-3 col-lg-3 col-12">
                                    <Card bg="dark">
                                        <Card.Header>Porcentaje consumo</Card.Header> 
                                        <Card.Body>{((this.state.used*100)/this.state.total).toFixed(2) + " % "}</Card.Body>
                                    </Card>
                                </Col> 
                            </Row>
                            <br />

                            <Row >
                                <Col className="informacion mb-3 col-lg-12 col-12" >
                                    <Poligonosutilizacion />
                                </Col>
                                <br /> 
                            </Row>
                            <br /> 
                        </Container>
                    </Col>
                </Row>
                <br />
                <br />
            </Container>

        );
    }
}
export default withRouter(MonitorServer);