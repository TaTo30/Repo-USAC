import React, { useState, useRef, useEffect } from "react";
import { AuthService } from "../service/AuthService";
import Publicacion from "./Publicacion";

const ListaPublicaciones = () => {
    const toast = useRef();

    const [userData, setUserData] = useState(() => {
        // getting stored value
        const saved = window.localStorage.getItem("key");
        const initialValue = JSON.parse(saved);
        return initialValue || {};
    });
    let publicacionTest = {
        date: "11/05/2021",
        username: "Luis47",
        name: "Luis Anibal Mendez",
        image: "https://www.pngmart.com/files/13/Attack-On-Titan-Logo-PNG-Free-Download.png",
        text: "Esta es una publicaion nueva de prueba",
    };
    const [listaPublicaciones, setListaPublicaciones] = useState([]);

    /**
     * Muestra una notificacion
     * @param {string} causa : (String) encabezado que acciona la notificacion
     * @param {string} mensaje : (String) mensaje que se mostrara en la notificacion
     * @param {string} severity : (String) severidad (info|warn|error|sucess)
     */
    const showNotification = (causa, mensaje, severity = "info") => {
        toast.current.show({ severity: severity, summary: causa, detail: mensaje, life: 6000 });
    };

    useEffect(() => {
        console.log("Obteneniendo publicaciones...");
        new AuthService()
            .get_publications(userData.username)
            .then((res) => {
                let listaPublicaciones = []
                listaPublicaciones = res.data.posts.map(x => {
                    return {
                        date: x.date,
                        image: x.image,
                        text: x.text,
                        username: userData.username,
                        name: userData.name
                    }
                })

                new AuthService()
                    .get_profile(userData._id).then(profile => {
                        profile.data.friends.forEach(friend => {
                            let listaPublicacionesFriend = []
                            listaPublicacionesFriend = friend.posts.map(x => {
                                return {
                                    date: x.date,
                                    image: x.image,
                                    text: x.text,
                                    username: friend.username,
                                    name: friend.name
                                }
                            });
                            listaPublicaciones.push(...listaPublicacionesFriend)
    
                       
                    })
                        setListaPublicaciones(listaPublicaciones);

                    })

            })
            .catch((err) => {
                console.log(err.response.data);
                showNotification("Publicaciones Recientes", "Ha ocurrido un error: " + err.response.data.error, "error");
            });
    }, []);

    return (
        <div className="gird">
            <div className="col-12">
                <div className="card">
                    <h2>Publicaciones recientes</h2>
                    {listaPublicaciones.length !== 0 ? (
                        <div>
                            {listaPublicaciones.map((publicacion) => (
                                <Publicacion data={publicacion} />
                            ))}
                        </div>
                    ) : (
                        <div>
                            <h6>Sin publicaciones</h6>
                            <p>;'(</p>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default ListaPublicaciones;
