import React, { useState, useRef, useEffect } from "react";
import { AuthService } from "../service/AuthService";
import { ListBox } from "primereact/listbox";

const ListaAmigos = () => {
    const [listaAmigos, setListaAmigos] = useState([]);

    const [userData, setUserData] = useState(() => {
        // getting stored value
        const saved = window.localStorage.getItem("key");
        const initialValue = JSON.parse(saved);
        return initialValue || {};
    });

    useEffect(() => {
        console.log("Geting friends list...");
        try {
            new AuthService()
                .get_profile(userData._id)
                .then((res) => {
                    setListaAmigos(res.data.friends)
                })
                .catch((err) => {
                    console.log(err.response.data);
                });
        } catch (error) {
            console.error(error);
        }
    }, []);

    return (
        <div className="grid">
            <div className="col-12">
                <div className="card">
                    <h2>Mis Amigos</h2>
                    {listaAmigos.length !== 0 ? (
                        <div className="col-12">
                            <ListBox options={listaAmigos} optionLabel={"name"} filter />
                        </div>
                    ) : (
                        <div>
                            <p>Aun no tienes amigos...</p>
                            <h4>ᕙ(^▿^-ᕙ)</h4>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default ListaAmigos;
