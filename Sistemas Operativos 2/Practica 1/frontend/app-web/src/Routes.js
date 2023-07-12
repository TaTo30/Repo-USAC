import React from "react";
import { Redirect, Route, Switch } from "react-router-dom";
import MonitorServer from "./MonitoreoServer/Monitor_serves";
import VistaMetrica from './VistraMetrica/vista_metrica'

export default function Routes() {
    return (
        <Switch>
            <Route exact path="/">
                <Redirect to="/monitor" />
            </Route>
            <Route exact path="/monitor">
                <MonitorServer />
            </Route>
            <Route exact path="/proceso">
                <VistaMetrica />
            </Route>
        </Switch>
    );
}