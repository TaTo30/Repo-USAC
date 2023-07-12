import React, { useState, useRef, useEffect } from "react";
import { Toast } from "primereact/toast";
import { AuthService } from "../service/AuthService";
import { Button } from "primereact/button";
import { ListBox } from "primereact/listbox";
import { Chip } from "primereact/chip";

const CrearSolicitud = () => {
    const toast = useRef();

    const [userData, setUserData] = useState(() => {
        // getting stored value
        const saved = window.localStorage.getItem("key");
        const initialValue = JSON.parse(saved);
        return initialValue || {};
    });

    const [listboxUserSelected, setListboxUserSelected] = useState(null);
    const [listaUsuarios, setListaUsuarios] = useState([{ username: "No users, currently" }]);
    const [sendRequestAsDisabled, setSendRequestAsDisabled] = useState(true);

    /**
     * Muestra una notificacion
     * @param {string} causa : (String) encabezado que acciona la notificacion
     * @param {string} mensaje : (String) mensaje que se mostrara en la notificacion
     * @param {string} severity : (String) severidad (info|warn|error|sucess)
     */
    const showNotification = (causa, mensaje, severity = "info") => {
        toast.current.show({ severity: severity, summary: causa, detail: mensaje, life: 6000 });
    };

    const handleOnClickSendRequest = () => {
        console.log("Enviando Solicitud...");
        let body = {
            from: userData.username,
            to: listboxUserSelected.username,
        };
        try {
            new AuthService()
                .send_FriendshipRequest(body)
                .then((res) => {
                    console.log(res);
                    showNotification("Enviar solicitud", "Tu solicitud ha sido enviada:" + res.data.message);
                    getNotFriendsList();
                })
                .catch((err) => {
                    console.log(err);
                    let errRes = err.response !== undefined ? err.response.data.message : "Error: no-message";
                    showNotification("API Connection: Lista usuarios", "Ha ocurrido algo al hacer peticion, " + errRes, "warn");
                });
        } catch (error) {
            console.error(error);
        } finally {
            console.log("... Fin creacion de solicitud");
        }
    };

    const handleListBoxValueSelected = (e) => {
        setListboxUserSelected(e.value);
    };

    const getNotFriendsList = () => {
        console.log("Getting users list...");
        try {
            new AuthService()
                .get_notFriendsUsers(userData._id)
                .then((res) => {
                    //console.log(res.data);
                    let listaUsuariosNoAmigos = res.data.map((usuario) => {
                        return {
                            username: usuario.username,
                            name: usuario.name,
                            photo: usuario.photo,
                            _id: usuario._id,
                        };
                    });
                    //console.log({listaUsuariosNoAmigos})
                    setListaUsuarios(listaUsuariosNoAmigos);
                })
                .catch((err) => {
                    console.log(err.response.data);
                    showNotification("API Connection: Lista usaurios", "Ha ocurrido algo al hacer peticion de lista de usaurios", "warn");
                });
        } catch (error) {
            console.error(error);
        }
    };

    useEffect(() => {
        getNotFriendsList();
    }, []);

    useEffect(() => {
        if (listboxUserSelected !== null) {
            setSendRequestAsDisabled(false);
        } else {
            setSendRequestAsDisabled(true);
        }
    }, [listboxUserSelected]);

    return (
        <div className="grid">
            <div className="col-12">
                <div className="card">
                    <Toast ref={toast} />
                    <h2>Enviar Solicitud de Amistad</h2>
                    <div className="col-12">
                        <ListBox value={listboxUserSelected} onChange={handleListBoxValueSelected} options={listaUsuarios} optionLabel={"username"} filter />
                    </div>
                    {listboxUserSelected !== null ? (
                        <div className="card">
                            <div className="product-item-content">
                                <div className="mb-3">
                                    <img width={300} src={listboxUserSelected.photo} alt={listboxUserSelected.name} className="product-image" />
                                </div>
                                <div>
                                    <h4 className="p-mb-1">Nombre: {listboxUserSelected.name}</h4>
                                    <span className={`product-badge status-instock`}>Username: {listboxUserSelected.username}</span>
                                    <div className="car-buttons mt-5">
                                        <Button type="button" className="p-button p-button-rounded mr-2" icon="pi pi-search"></Button>
                                        <Button type="button" className="p-button-success p-button-rounded mr-2" icon="pi pi-star"></Button>
                                        <Button type="button" className="p-button-help p-button-rounded" icon="pi pi-cog"></Button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    ) : (
                        <>
                            <div>
                                <h5>No user selected...</h5>
                            </div>
                        </>
                    )}
                    <Button onClick={handleOnClickSendRequest} disabled={sendRequestAsDisabled} label="Enviar Solicitud" icon="pi pi-bolt" />
                </div>
            </div>
        </div>
    );
};

export default CrearSolicitud;
