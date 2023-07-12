import React from "react";
import { Navbar, Nav } from "react-bootstrap"; 
import './nav.css'
export default class NavbarVista extends React.Component {

    render() {
        return ( 
        <Navbar collapseOnSelect expand="lg" bg="primary" variant="dark" >
        <Navbar.Brand >Practica 1</Navbar.Brand>
        <Navbar.Toggle aria-controls="responsive-navbar-nav" />
        <Navbar.Collapse id="responsive-navbar-nav">
          <Nav className="mr-auto">
            <Nav.Link href="/proceso">Procesos</Nav.Link>
            <Nav.Link href="/monitor">Monitor</Nav.Link>
          </Nav>  
        </Navbar.Collapse>
      </Navbar>
      );
    }
}