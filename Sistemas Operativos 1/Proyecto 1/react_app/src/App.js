import React from "react";
import { BrowserRouter, Route, Redirect } from "react-router-dom";
import Principal from "./Pages/Principal";
import Reportes from "./Pages/Reportes";

function App() { 
  return(
    <BrowserRouter>
        <Route path="/Principal" component={Principal} />
        <Route path="/Reportes" component={Reportes} />
        <Route path="/" render={() => <Redirect to="/Principal" />} exact={true} />
    </BrowserRouter> 
  ); 
}

export default App;