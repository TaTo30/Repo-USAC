import React, { useState, useRef } from "react";
import { Button } from "primereact/button";
import { InputText } from "primereact/inputtext";
import { Toast } from "primereact/toast";
import Axios from "axios";
import { useHistory } from "react-router-dom";
import { AuthService } from "../service/AuthService";

const RegistroUsuarios = () => {
    const toast = useRef();
    let history = useHistory();

    const [userName, setUserName] = useState("");
    const [name, setName] = useState("");
    const [password, setPassword] = useState("");
    const [password2, setPassword2] = useState("");
    const [foto, setFoto] = useState("");

    const showWarn = (causa, mensaje) => {
        toast.current.show({ severity: "warn", summary: causa, detail: mensaje, life: 6000 });
    };

    const showSuccess = (causa, mensaje) => {
        toast.current.show({ severity: "success", summary: causa, detail: mensaje, life: 6000 });
    };

    const handleLogInClick = () => {
        history.push("/");
    };

    const setNewUser = () => {
        if ((userName !== "") & (name !== "") & (password !== "") & (password2 !== "") & (foto !== "")) {
            if (password === password2) {
                let user = {
                    username: userName,
                    name: name,
                    password: password,
                    photo: foto,
                };
                //console.log(foto);
                try {
                   new AuthService().register(user)
                        .then((res) => {
                            console.dir(res.data);
                            setFoto("");
                            setName("");
                            setUserName("");
                            setPassword("");
                            setPassword2("");
                            showSuccess("Creacion Usuarios", `Su usuario ${res.data.data} ha sido creado correctamente!`);
                            
                        })
                        .catch((err) => {
                            console.log(err.response)
                            showWarn("Crear Usuario", "Ha ocurrido un error al hacer la peticion, " + err.response.data.error);
                        });
                } catch (error) {
                    console.error(error);
                    showWarn("API Connection", "Ha ocurrido un error al hacer la peticion, " + error);
                }
            } else {
                showWarn("User Password", "¡Las contraseñas deben coincidir, intentelo de nuevo!");
            }
        } else {
            console.log({ name, userName, password, password2 });
            showWarn("Datos de Usuario", "Debe completar todos los datos para registro");
        }
    };

    const onUpload = (archivos) => {
        Array.from(archivos).forEach((archivo) => {
            let reader = new FileReader();
            reader.readAsDataURL(archivo);
            reader.onload = function () {
                let base64 = reader.result;
                setFoto(base64.split(",")[1]);
                //console.log(foto);
            };
        });
    };

    return (
        <div className="surface-ground px-4 py-8 md:px-6 lg:px-8 flex align-items-center justify-content-center">
            <div className="surface-card p-4 shadow-2 border-round w-full lg:w-6">
                <div className="text-center mb-5">
                    <img src="images/blocks/logos/hyper.svg" alt="hyper" height="50" className="mb-3" />
                    <div className="text-900 text-3xl font-medium mb-3">Registrate</div>
                    <span className="text-600 font-medium line-height-3">Ingresa tus datos</span>
                </div>

                <div>
                    <Toast ref={toast} />
                    <label htmlFor="username" className="block text-900 font-medium mb-2">
                        Username (usuario para ingresar a tu cuenta)
                    </label>
                    <InputText id="username" type="text" className="w-full mb-3" onChange={(e) => setUserName(e.target.value)} />

                    <label htmlFor="name" className="block text-900 font-medium mb-2">
                        Nombre (infomracion de tu cuenta, tu nombre)
                    </label>
                    <InputText id="name" type="text" className="w-full mb-3" onChange={(e) => setName(e.target.value)} />

                    <label htmlFor="password1" className="block text-900 font-medium mb-2">
                        Ingresa Password
                    </label>
                    <InputText id="password1" type="password" className="w-full mb-3" onChange={(e) => setPassword(e.target.value)} />

                    <label htmlFor="password2" className="block text-900 font-medium mb-2">
                        Confirmar Password
                    </label>
                    <InputText id="password2" type="password" className="w-full mb-3" onChange={(e) => setPassword2(e.target.value)} />

                    <div className="flex align-items-center justify-content-between mb-6">
                        <div className="flex align-items-center">
                            <span className="text-600 font-medium line-height-3">¿Ya tienes una cuenta?</span>
                            <button onClick={handleLogInClick} className="p-link font-medium no-underline ml-2 text-blue-500 cursor-pointer">
                                Iniciar sesion
                            </button>
                        </div>
                    </div>

                    <div className="card">
                        <h5>Escoge tu archivo</h5>
                        <input type="file" onChange={(e) => onUpload(e.target.files)} multiple={true} accept="image/*"></input>
                    </div>

                    <Button onClick={setNewUser} label="Completar Registro" icon="pi pi-user" className="w-full" />
                </div>
            </div>
        </div>
    );
};

export default RegistroUsuarios;
