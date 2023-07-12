import { Button } from "primereact/button";
import React, { useState, useRef, useEffect } from "react";
import { Toast } from "primereact/toast";
import { ListBox } from "primereact/listbox";
import { AuthService } from "../service/AuthService";

const AceptarSolicitud = () => {
    const toast = useRef();

    const [userData, setUserData] = useState(() => {
        // getting stored value
        const saved = window.localStorage.getItem("key");
        const initialValue = JSON.parse(saved);
        return initialValue || {};
    });

    const [listboxUserSelected, setListboxUserSelected] = useState(null);
    const [listaUsuarios, setListaUsuarios] = useState([{ username: "No users, currently" }]);
    const [acceptRequestAsDisabled, setAcceptSendRequestAsDisabled] = useState(true);

    const handleOnClickAcceptRequest = () => {
        console.log("Aceptando solicitud...");
        try {
            
            new AuthService()
                .accept_request(userData._id,listboxUserSelected.idRequest )
                .then((res) => {
                    showNotification("Solicitud Aceptada", "Tu solicitud ha sido aceptada");
                    getRequestList();
                 })
                .catch((err) => {
                    console.log(err.response.data);
                    showNotification("API Connection: Aceptar solicitud", "Algo ha ocurrido al hacer la peticion", "error");
                });
        } catch (error) {
            console.error(error);
        }
    };
    const handleOnClickRejectRequest = () => {
        try {
            
            new AuthService()
                .reject_request(listboxUserSelected.idRequest)
                .then((res) => {
                    showNotification("Solicitud Rechazada", "Tu solicitud ha sido rechazada");
                    getRequestList();
                 })
                .catch((err) => {
                    console.log(err.response.data);
                    showNotification("API Connection: Rechazar solicitud", "Algo ha ocurrido al hacer la peticion", "error");
                });
        } catch (error) {
            console.error(error);
        }
    }

    /**
     * Muestra una notificacion
     * @param {string} causa : (String) encabezado que acciona la notificacion
     * @param {string} mensaje : (String) mensaje que se mostrara en la notificacion
     * @param {string} severity : (String) severidad (info|warn|error|sucess)
     */
    const showNotification = (causa, mensaje, severity = "info") => {
        toast.current.show({ severity: severity, summary: causa, detail: mensaje, life: 6000 });
    };

    const handleListBoxValueSelected = (e) => {
        setListboxUserSelected(e.value);
    };

    const getRequestList = () => {
        try {
            new AuthService().get_Requests(userData.username)
                .then(res => {
                    let listaDeUsuariosRequest = res.data.requests.filter(x => x.from != userData.username && x.status == 'pending').map((request) => {

                        return {
                            username: request.from,
                            name: request.from,
                            idRequest: request._id
                        }

                    });
                    listaDeUsuariosRequest.length > 0 ? setListaUsuarios(listaDeUsuariosRequest) : setListaUsuarios([]);
                })
        } catch (error) {
            console.error(error);
        }
    }

    useEffect(() => {
        getRequestList();
    }, []);

    useEffect(() => {
        if (listboxUserSelected !== null) {
            setAcceptSendRequestAsDisabled(false);
        } else {
            setAcceptSendRequestAsDisabled(true);
        }
    }, [listboxUserSelected]);

    return (
        <div className="grid">
            <div className="col-12">
                <div className="card">
                    <Toast ref={toast} />
                    <h2>Solicitudes Pendientes de Aprobacion</h2>
                    <div className="col-12">
                        <ListBox value={listboxUserSelected} onChange={handleListBoxValueSelected} options={listaUsuarios} optionLabel={"username"} filter />
                    </div>
                    <Button onClick={handleOnClickAcceptRequest} disabled={acceptRequestAsDisabled} label="Aceptar solicitud" />
                    <Button onClick={handleOnClickRejectRequest} disabled={acceptRequestAsDisabled} label="Rechazar solicitud" />
                </div>
            </div>
        </div>
    );
};

export default AceptarSolicitud;
