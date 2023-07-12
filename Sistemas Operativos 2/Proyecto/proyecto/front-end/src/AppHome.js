import React from "react";
import { BrowserRouter as Router, Route, Switch } from "react-router-dom";
import App from "./App";
import Login from "./components/Login";
import RegistroUsuarios from "./components/RegistroUsuarios";

const AppHome = () => {

    return (
        <Router>
            <Switch>
                <Route path="/" exact>
                    <div className="layout-wrapper layout-static layout-theme-light">
                        <Login />
                    </div>
                </Route>
                <Route path="/registrar" exact>
                    <div className="layout-wrapper layout-static layout-theme-light">
                        <RegistroUsuarios></RegistroUsuarios>
                    </div>
                </Route>
                <Route>
                    <App></App>
                </Route>
            </Switch>
        </Router>
    );
};

export default AppHome;
