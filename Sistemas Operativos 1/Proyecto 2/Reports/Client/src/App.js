import React from "react";
import { BrowserRouter, Route, Redirect } from "react-router-dom";
import Pagina from "./Pages/Pagina";

function App() {

  return(
    <BrowserRouter>
        <Route path="/principal" component={Pagina} />
        <Route path="/" render={() => <Redirect to="/principal" />} exact={true} />
    </BrowserRouter> 
  ); 
}

export default App;