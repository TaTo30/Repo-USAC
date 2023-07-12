import Axios from "axios";
import { Button } from "primereact/button";
import { Checkbox } from "primereact/checkbox";
import { InputText } from "primereact/inputtext";
import { Toast } from "primereact/toast";
import React, { useState, useRef } from "react";
import { useHistory } from "react-router-dom";
import { AuthService } from "../service/AuthService";
import "./FormDemo.css";
import "../assets/demo/flags/flags.css";
import "../assets/demo/Demos.scss";
import "../assets/layout/layout.scss";
import "../App.scss";

const Login = () => {
    const authService = new AuthService();
    const toast = useRef();
    let history = useHistory();

    const [checked, setChecked] = useState(false);
    const [name, setName] = useState("");
    const [userName, setUserName] = useState("");
    const [password, setPassword] = useState("");

    const showWarn = (causa, mensaje) => {
        toast.current.show({ severity: "warn", summary: causa, detail: mensaje, life: 6000 });
    };

    const handleCreateUser = () => {
        history.push("/registrar");
    };

    const loginUser = () => {
        if ((userName === "") | (password === "") | (name === "")) {
            showWarn("Datos de Usuario", "¡Debe introducir todos los datos para ingresar a su cuenta!");
        } else {
            let body = {
                username: userName,
                password: password,
                name: name,
            };
            try {
                authService
                    .login(body)
                    .then((res) => {
                        console.dir(res.data);
                        window.localStorage.setItem("key", JSON.stringify(res.data.data));
                        history.push("/home");
                    })
                    .catch((err) => {
                        console.log(err.response);
                        let errRes = err.response !== undefined ? err.response.data.error : "No message";
                        showWarn("Login Usuario", "Ha ocurrido un error al hacer la peticion: " + errRes);
                    });
            } catch (error) {
                console.error(error);
            }
        }
    };

    return (
        <div className="layout-wrapper layout-static layout-theme-light">
        <div className="surface-ground px-4 py-8 md:px-6 lg:px-8 flex align-items-center justify-content-center">
            <div className="surface-card p-4 shadow-2 border-round w-full lg:w-6">
                <div className="text-center mb-5">
                    <img src="images/blocks/logos/hyper.svg" alt="hyper" height="50" className="mb-3" />
                    <div className="text-900 text-3xl font-medium mb-3">Bienvenido de Regreso</div>
                    <span className="text-600 font-medium line-height-3">¿Aun no tienes una cuenta?</span>
                    <button onClick={handleCreateUser} className="p-link font-medium no-underline ml-2 text-blue-500 cursor-pointer">
                        ¡Crear ahora!
                    </button>
                    <br />
                    <span className="text-600 font-medium line-height-3">¡Version V2 para Updates y Rollback!</span>
                </div>

                <div>
                    <Toast ref={toast} />

                    <label htmlFor="name" className="block text-900 font-medium mb-2">
                        Nombre
                    </label>
                    <InputText id="name" type="text" className="w-full mb-3" onChange={(e) => setName(e.target.value)} />

                    <label htmlFor="email1" className="block text-900 font-medium mb-2">
                        Nombre de Usuario
                    </label>
                    <InputText id="email1" type="text" className="w-full mb-3" onChange={(e) => setUserName(e.target.value)} />

                    <label htmlFor="password1" className="block text-900 font-medium mb-2">
                        Password
                    </label>
                    <InputText id="password1" type="password" className="w-full mb-3" onChange={(e) => setPassword(e.target.value)} />

                    <div className="flex align-items-center justify-content-between mb-6">
                        <div className="flex align-items-center">
                            <Checkbox inputId="rememberme1" binary className="mr-2" onChange={(e) => setChecked(e.checked)} checked={checked} />
                            <label htmlFor="rememberme1">Remember me</label>
                        </div>
                    </div>

                    <Button onClick={loginUser} label="Sign In" icon="pi pi-user" className="w-full" />
                </div>
            </div>
            </div>
        </div>
    );
};

export default Login;
