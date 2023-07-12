import Axios from "axios";
import { Button } from "primereact/button";
import { InputText } from "primereact/inputtext";
import { Toast } from "primereact/toast";

import React, { useState, useRef } from "react";

const CrearAlbum = () => {
    const toast = useRef();

    const [baseUrl, setBaseUrl] = useState("http://application-load-balancer-1846011889.us-east-2.elb.amazonaws.com");
    const [newAlbumName, setNewAlbumName] = useState("");
    const [userData, setUserData] = useState(() => {
        // getting stored value
        const saved = window.localStorage.getItem("key");
        const initialValue = JSON.parse(saved);
        return initialValue || {};
    });

    const [buttonEnabled, setButtonEnabled] = useState(false);

    const handleInputTextChange = (e) => {
        setNewAlbumName(e.target.value);
        if ((newAlbumName !== undefined) & (newAlbumName !== "")) {
            setButtonEnabled(true);
        } else {
            setButtonEnabled(false);
        }
    };

    const handleClickCreateAlbum = () => {
        crearAlbum();
    };

    const showWarn = (causa, mensaje) => {
        toast.current.show({ severity: "warn", summary: causa, detail: mensaje, life: 6000 });
    };

    const showSuccessMessage = (causa, mensaje) => {
        toast.current.show({ severity: "success", summary: causa, detail: mensaje, life: 6000 });
    };

    const crearAlbum = async () => {
        if (newAlbumName === "") {
            showWarn("Creacion de Album", "Debe seleccionar un nombre valido para el album");
            return;
        }
        let body = {
            idUsuario: userData.id,
            nameAlbum: newAlbumName,
        };
        try {
            await Axios.post(`${baseUrl}/album`, body)
                .then((res) => {
                    console.dir(res);
                    showSuccessMessage("Creacion de Album", "El album:" + newAlbumName + " ha sido creado exitosamente!");
                })
                .catch((err) => {
                    showWarn("API Connection", "Se ha producido en error al hacer la peticion, " + err.toString());
                });
        } catch (error) {
            console.error(error);
            showWarn("API Connection", "Ha ocurrido un error al hacer la peticion, " + error);
        }
    };

    return (
        <div className="grid">
            <div className="col-12">
                <div className="card">
                    <div className="grid p-fluid">
                        <Toast ref={toast} />
                        <h5>Crear un Nuevo Album</h5>
                        <div className="col-12">
                            <div className="card">
                                <h5>Ingresa el nombre del nuevo Album</h5>
                                <span className="p-input-icon-left p-input-icon-right">
                                    <InputText onChange={handleInputTextChange} type="text" placeholder="Enter name"></InputText>
                                </span>
                            </div>
                            <div className="col-12 mb-2 lg:col-4 lg:mb-0">
                                <Button onClick={handleClickCreateAlbum} disabled={!buttonEnabled} label="Crear Album"></Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default CrearAlbum;
