import React from "react";
import { Button, Card, CardDeck, Container, Form, Row, Table } from "react-bootstrap";
import './vista_metrica.css'
import { Col } from 'react-bootstrap';
import NavbarVista from './NavbarVista';
import { withRouter } from "react-router";
import TreeView from '@mui/lab/TreeView';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';
import TreeItem from '@mui/lab/TreeItem';
import Histograma from "./Histograma";
import CmdStrace from "./cmdstrace";
;
class VistaMetrica extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            foto: '',
            values: {},
            fila: [],
            registrados: 0,
            running: 0,
            sleeping: 0,
            stopped: 0,
            zombie: 0,
            selected: "",
            texto:"",
            data: {
                id: 'root',
                name: 'Parent',
                children: [
                    {
                        id: '1',
                        name: 'Child - 1',
                    },
                    {
                        id: '2',
                        name: 'Parent'
                    }
                    // …
                ],
            }
        }
    }
    list_to_tree(list) {
        try {
            var map = {}, node, roots = [], i;

            for (i = 0; i < list.length; i += 1) {
                map[list[i].id] = i; // initialize the map
                list[i].children = []; // initialize the children
            }

            for (i = 0; i < list.length; i += 1) {
                node = list[i];
                if (node.father !== null) {
                    // if you have dangling branches check that map[node.parentId] exists
                    list[map[node.father]].children.push(node);
                } else {
                    roots.push(node);
                }
            }
            return roots;
        } catch (e) {

        }
    }


    renderTree = (nodes) => (
        <TreeItem key={nodes.id} nodeId={nodes.id} label={nodes.name}>
            {Array.isArray(nodes.children)
                ? nodes.children.map((node) => this.renderTree(node))
                : null}
        </TreeItem>
    );

    kill_process() {
        var url = 'http://3.129.64.103:8000/killprocess';
        var data = { "pid": parseInt(this.state.selected) }

        fetch(url, {
            mode: 'cors',
            method: 'POST', // or 'PUT'
            body: JSON.stringify(data), // data can be `string` or {object}!
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(res => res.json())
            .catch(error => console.error('Error:', error))
            .then(response => {
                console.log(response)
                alert(response.res)
                this.setState({selected:""})
            }
            );
    }

    strace() {
        var url = 'http://3.129.64.103:8000/attach/';
        var data = { "pid": parseInt(this.state.selected) }

        fetch(url, {
            mode: 'cors',
            method: 'POST', // or 'PUT'
            body: JSON.stringify(data), // data can be `string` or {object}!
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(res => res.json())
            .catch(error => console.error('Error:', error))
            .then(response => { 
                this.setState({texto:response.res})
            }
            );
    }

    masInfectado = () => {
        var url = 'http://3.129.64.103:8000/process';

        fetch(url, {
            mode: 'cors',
            method: 'GET',
            headers: { 'Content-Type': 'application/json' }
        }).then(res => res.json())
            .catch(error => console.error('Error:', error))
            .then(response => { 
                this.setState({ registrados: response.registered })
                this.setState({ running: response.running })
                this.setState({ sleeping: response.sleeping })
                this.setState({ stopped: response.stopped })
                this.setState({ zombie: response.zombie })


                let x = this.list_to_tree(response.data) 
                const grade = {
                    id: "root"
                };
                const finalResult = Object.assign(response.data[0], grade);

                for (let i = 1; i < response.data[0].children.length; i++) {
                    const grade = {
                        id: response.data[i].id + "root"
                    };

                    const finalResult = Object.assign(response.data[i], grade);
                } 
                
                this.recorrer(response.data[0].children,null)

                this.setState({ data: response.data[0] }) 
                 
                let array = this.state.data
                for(let e of response.data){
                    if(typeof e.id == 'number'){ 
                        array.children.push({
                            id: e.id+"",
                            name: e.name + "  -  " + e.estate + "  -  " + e.user + "  -  " + e.id,
                        })
                    } 
                }
                this.setState({ data: array }) 
            }
            );
    }

    recorrer(lista,nodo) {
        
        if (lista.length == 0) { 
            return
            
        }
        for (let i = 0; i < lista.length; i++) {
            const grade = {
                id: lista[i].id + ""
            };
            const grade2 = {
                name: lista[i].name + "  -  " + lista[i].estate + "  -  " + lista[i].user + "  -  " + lista[i].id
            };
            const finalResult = Object.assign(lista[i], grade);
            const finalResult2 = Object.assign(lista[i], grade2);
            this.recorrer(lista[i].children,lista[i])

        }
    }
    fetchChildNodes(id) {
        return new Promise(resolve => {
            setTimeout(() => {
                resolve({
                    children: [
                        {
                            id: "2",
                            name: "Calendar"
                        },
                        {
                            id: "3",
                            name: "Settings"
                        },
                        {
                            id: "4",
                            name: "Music"
                        }
                    ]
                });
            }, 1000);
        });
    }

    handleSelect = (event, nodeIds) => {
        this.setState({ selected: nodeIds });
        console.log(this.state.selected)
    };

    handleSelectClick = () => {
        this.setState({ selected: 'oldSelected' });
        console.log(this.state.selected)
    };




    componentDidMount() {
        this.masInfectado()
        this.intervalo = setInterval(this.masInfectado, 3000)
    }

    render() {
        return (
            <>
                <Container fluid={true} >
                    <Row>
                        <NavbarVista />
                    </Row>
                    <br />
                    <Row>
                        <Col>
                            <Container >
                                <Row>
                                    <Col className="mb-3 col-lg-4 col-12">
                                        <Card bg="dark">
                                            <Card.Header>Procesos Registrados</Card.Header>
                                            <Card.Body>{this.state.registrados}</Card.Body>
                                        </Card>
                                    </Col>
                                    <Col className="mb-3 col-lg-4 col-12">
                                        <Card bg="dark">
                                            <Card.Header>Procesos Running</Card.Header>
                                            <Card.Body>{this.state.running}</Card.Body>
                                        </Card>
                                    </Col>
                                    <Col className="mb-3 col-lg-4 col-12">
                                        <Card bg="dark">
                                            <Card.Header>Porcentaje Sleeping</Card.Header>
                                            <Card.Body>{this.state.sleeping}</Card.Body>
                                        </Card>
                                    </Col>
                                </Row>
                            </Container>
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                            <Container >
                                <Row>
                                    <Col className="mb-3 col-lg-6 col-12 primera">
                                        <Card bg="dark">
                                            <Card.Header>Procesos Stoped</Card.Header>
                                            <Card.Body>{this.state.stopped}</Card.Body>
                                        </Card>
                                    </Col>
                                    <Col className="mb-3 col-lg-6 col-12">
                                        <Card bg="dark">
                                            <Card.Header>Procesos Zombie</Card.Header>
                                            <Card.Body>{this.state.zombie}</Card.Body>
                                        </Card>
                                    </Col>
                                </Row>
                            </Container>
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                            <Container >
                                <Row>
                                    <Col className="mb-3 col-lg-6 col-12">
                                        <Card bg="dark">
                                        <Card.Header>Comando Strace</Card.Header>
                <Card.Body>
                    <Form>
                        <Form.Group className="mb-3" controlId="control">
                            <Form.Label>Salida</Form.Label>
                            <Form.Control style={{ overflow: 'auto' }} as="textarea" rows={10} className="text-light bg-dark" readOnly value={this.state.texto} />
                        </Form.Group>
                    </Form>
                </Card.Body>
                                        </Card>
                                    </Col>
                                    <Col className="mb-3 col-lg-6 col-12">
                                        <Histograma></Histograma>

                                    </Col>
                                </Row>
                            </Container>
                        </Col>
                    </Row>
                    <Row>
                        <Col>
                            <Container >
                                <Row>
                                    <Col className="mb-3 col-lg-12 col-12">
                                        <Card bg="dark">
                                            <Card.Header>Arbol de Procesos
                                                <Form>
                                                    <Form.Group className="mb-3" controlId="formBasicEmail">
                                                        <Form.Label> PID de proceso </Form.Label>
                                                        <Form.Control type="text" value={this.state.selected}
                                                            onChange={(e) => this.setState({ selected: e.target.value })} />
                                                    </Form.Group>

                                                </Form>
                                            </Card.Header>
                                            <Card.Body>
                                                <TreeView
                                                    aria-label="file system navigator"
                                                    defaultCollapseIcon={<ExpandMoreIcon />}
                                                    defaultExpandIcon={<ChevronRightIcon />}
                                                    sx={{ height: 240, flexGrow: 1, overflowY: 'auto' }}
                                                    expandIcons={true}
                                                >
                                                    {this.renderTree(this.state.data)}
                                                </TreeView>
                                                <Row>
                                                    <Col><Button variant="success" onClick={() => { this.strace() }}>
                                                        Strace
                                                    </Button></Col>
                                                    <Col><Button variant="danger" onClick={() => { this.kill_process() }}>
                                                        Kill
                                                    </Button></Col>
                                                </Row>
                                            </Card.Body>
                                        </Card>
                                    </Col>
                                </Row>
                            </Container>
                        </Col>
                    </Row>
                </Container>
            </>
        );
    }
}

export default withRouter(VistaMetrica);