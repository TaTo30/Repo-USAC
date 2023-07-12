import { Bar } from 'react-chartjs-2';
import React from "react";
import { Card, Form } from 'react-bootstrap';


export default class CmdStrace extends React.Component {

    constructor(props) {
        super(props);
        this.state = {
            texto:"xd"

        }
    }


    
        componentDidMount() { 
            this.getBarrasEdad()
            this.intervalo = setInterval(this.getBarrasEdad,3000)
        }
    
    getBarrasEdad = () => {
        var url = 'http://3.129.64.103:8000/attach/';

        fetch(url, {
            mode: 'cors',
            method: 'POST', // or 'PUT' 
            headers: {
                'Content-Type': 'application/json'
            },
            body:{
                
            }
        }).then(res => res.json())
            .catch(error => console.error('Error:', error))
            .then(response => {
                try{

                    console.log(response)
                    this.setState({texto:response.res})
                }catch(e){

                }
            }
            );
    }

    render() {
        return (
            <>
                <Card.Header>Comando Strace</Card.Header>
                <Card.Body>
                    <Form>
                        <Form.Group className="mb-3" controlId="control">
                            <Form.Label>Salida</Form.Label>
                            <Form.Control style={{ overflow: 'auto' }} as="textarea" rows={10} className="text-light bg-dark" readOnly value={this.state.texto} />
                        </Form.Group>
                    </Form>
                </Card.Body>
            </>
        );
    }
}